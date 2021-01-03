using System;
using System.ServiceProcess;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using FileOperations;
using Datamanager;
using System.Configuration;
using System.Data.SqlClient;
using DataAccessLayer;
using ServiceLayer;
using Models;
using System.Threading.Tasks;

namespace ETL
{
    public class obj
    {
        public string sourceDirectory { get; set; }
        public string targetPath { get; set; }
        public string regex { get; set; }
        public string extension { get; set; }
        public string key { get; set; }
    }
public partial class Service1 : ServiceBase
    {
        Logger logger;
        public Service1()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        internal void TestConsole(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            this.OnStop();
        }

        protected override void OnStart(string[] args)
        {
            ThreadPool.QueueUserWorkItem(async state =>
            {
                logger = new Logger();
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                try
                {
                    var repositories = new UnitOfWork(connectionString);
                    OrderService orderService = new OrderService(repositories);
                    var ordersInfo = await orderService.GetOrders();

                    //making proper file name
                    DateTime now = DateTime.Now;
                    var currTime = now.ToString("yyyy_MM_dd_HH_mm_ss");

                    XmlGenerator<Order> orders = new XmlGenerator<Order>(logger.parsedOptions.options.PathsOptions.SourceDirectory + "Sales_" + currTime + ".txt");
                    await orders.XmlGenerate(ordersInfo);
                    this.OnStop();
                }
                catch (Exception trouble)
                {
                    var repositories2 = new UnitOfWork(connectionString);
                    ErrorService service = new ErrorService(repositories2);
                    service.AddErrors(new Error(trouble.GetType().Name, trouble.Message, DateTime.Now));
                }
                Thread loggerThread = new Thread(new ThreadStart(logger.Start));
                loggerThread.Start();
            });
        }

        protected override void OnStop()
        {
            logger.Stop();
            Thread.Sleep(1000);
        }

        class Logger
        {
            FileSystemWatcher watcher;
            public Parse parsedOptions = new Parse();
            object obj = new object();
            bool enabled = true;

            public Logger()
            {
                watcher = new FileSystemWatcher(parsedOptions.options.PathsOptions.SourceDirectory);
                watcher.Created += Watcher_Created;
            }

            public void Start()
            {
                watcher.EnableRaisingEvents = true;
                while (enabled)
                {
                    Thread.Sleep(1000);
                }
            }
            public void Stop()
            {
                watcher.EnableRaisingEvents = false;
                enabled = false;
            }
            // создание файлов
            private void Watcher_Created(object sender, FileSystemEventArgs e)
            {
                ThreadPool.QueueUserWorkItem(async state =>
                {
                    string fileName = e.Name;
                    string filePath = e.FullPath;
                    Console.WriteLine(fileName);

                    string fileEvent = "создан";
                    Regex regex = new Regex(parsedOptions.options.PathsOptions.Regex);
                    Console.WriteLine(fileName + " ||| " + regex.IsMatch(fileName));
                    if (regex.IsMatch(fileName))
                    {
                        RecordEntry("вошли", fileName);
                        await MyFile.EncryptFile(fileName, filePath, parsedOptions.options.EncryptingOptions.Key);

                        await MyFile.CompressAndMove(fileName, filePath, parsedOptions.options.PathsOptions.TargetPath, parsedOptions.options.CompressOptions.Extension);

                        await MyFile.DecompressFileToTargetDir(fileName, filePath, parsedOptions.options.CompressOptions.Extension);

                        string newPath = MyFile.GetPathOfFileInTargetDir(fileName);
                        newPath += fileName;

                        await MyFile.DecryptFile(fileName, newPath, parsedOptions.options.EncryptingOptions.Key);
                    }

                    RecordEntry(fileEvent, fileName);
                });
            }
            private void RecordEntry(string fileEvent, string filePath)
             {
                lock (obj)
                {
                    using (StreamWriter writer = new StreamWriter("D:\\Trash\\templog.txt", true))
                    {
                        writer.WriteLine(String.Format("{0} файл {1} был {2}",
                            DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), filePath, fileEvent));
                        writer.Flush();
                    }
                }
            }
        }
    }
}
