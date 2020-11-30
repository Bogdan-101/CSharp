using System;
using System.ServiceProcess;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using FileOperations;
using Newtonsoft.Json.Linq;

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
            logger = new Logger();
            Thread loggerThread = new Thread(new ThreadStart(logger.Start));
            loggerThread.Start();
        }

        protected override void OnStop()
        {
            logger.Stop();
            Thread.Sleep(1000);
        }

        class Logger
        {
            FileSystemWatcher watcher;
            obj ob;
            object obj = new object();
            bool enabled = true;

            public Logger()
            {
                JObject data = JObject.Parse(File.ReadAllText(@"C:\Users\Lenovo\Documents\GitHub\CSharp\ETL project\appsettings.json"));
                ob = data.ToObject<obj>();
                watcher = new FileSystemWatcher(ob.sourceDirectory);
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
                Thread.Sleep(1000);

                string fileName = e.Name;
                string filePath = e.FullPath;
                Console.WriteLine(fileName);

                string fileEvent = "создан";
                Regex regex = new Regex(ob.regex);

                if (regex.IsMatch(fileName))
                {
                    RecordEntry("вошли", fileName);
                    MyFile.EncryptFile(fileName, filePath, ob.key);

                    MyFile.CompressAndMove(fileName, filePath, ob.targetPath, ob.extension);

                    MyFile.DecompressFileToTargetDir(fileName, filePath, ob.extension);

                    string newPath = MyFile.GetPathOfFileInTargetDir(fileName);
                    newPath += fileName;

                    MyFile.DecryptFile(fileName, newPath, ob.key);
                }

                RecordEntry(fileEvent, fileName);
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
