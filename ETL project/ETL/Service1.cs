﻿using System;
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
            logger.StartServices();
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
            Parse parsedOptions = new Parse();
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
            public void StartServices()
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                string sqlExpression = "SELECT TOP 10 [OrderID], [CustomerID], [EmployeeID], [OrderDate], [RequiredDate], [ShippedDate], [ShipVia], [Freight], [ShipName], [ShipAddress] ,[ShipCity], [ShipRegion], [ShipPostalCode], [ShipCountry] FROM[NORTHWND].[dbo].[Orders]";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        Console.WriteLine("{0}\t{1}\t{2}", reader.GetName(0), reader.GetName(1), reader.GetName(2));

                        while (reader.Read())
                        {
                            var order = new Order();
                            order.OrderId = reader.GetInt32(0);
                            order.customer = reader.GetString(1);
                            order.shipName = reader.GetString(8);
                            order.shipAddress = reader.GetString(9);
                            order.shipCity = reader.GetString(10);
                            order.shipCountry = reader.GetString(13);
                            order.shippedDate = reader.GetDateTime(5);
                            Console.WriteLine("{0} \t{1} \t{2}", order.OrderId, order.customer, order.shippedDate);
                        }
                    }
                    reader.Close();
                }
                try
                {
                    var repositories = new UnitOfWork(connectionString);
                    OrderService orderService = new OrderService(repositories);
                    var ordersInfo = orderService.GetListOfOrders();
                    Console.WriteLine("succ");

                    XmlGenerator<Order> orders = new XmlGenerator<Order>(AppDomain.CurrentDomain.BaseDirectory + "Orders.xml");
                    orders.XmlGenerate(ordersInfo);
                }
                catch (Exception trouble)
                {
                    var repositories2 = new UnitOfWork(connectionString);
                    ErrorService service = new ErrorService(repositories2);
                    service.AddErrors(new Error(trouble.GetType().Name, trouble.Message, DateTime.Now));
                }
            }
            // создание файлов
            private void Watcher_Created(object sender, FileSystemEventArgs e)
            {
                Thread.Sleep(1000);

                string fileName = e.Name;
                string filePath = e.FullPath;
                Console.WriteLine(fileName);

                string fileEvent = "создан";
                Regex regex = new Regex(parsedOptions.options.PathsOptions.Regex);

                if (regex.IsMatch(fileName))
                {
                    RecordEntry("вошли", fileName);
                    MyFile.EncryptFile(fileName, filePath, parsedOptions.options.EncryptingOptions.Key);

                    MyFile.CompressAndMove(fileName, filePath, parsedOptions.options.PathsOptions.TargetPath, parsedOptions.options.CompressOptions.Extension);

                    MyFile.DecompressFileToTargetDir(fileName, filePath, parsedOptions.options.CompressOptions.Extension);

                    string newPath = MyFile.GetPathOfFileInTargetDir(fileName);
                    newPath += fileName;

                    MyFile.DecryptFile(fileName, newPath, parsedOptions.options.EncryptingOptions.Key);
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
