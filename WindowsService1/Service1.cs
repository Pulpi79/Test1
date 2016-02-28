using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        public void TestMethod()
        {
            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
            fileSystemWatcher.Created += new FileSystemEventHandler(FileSystemWatcher_Created);
            fileSystemWatcher.Path = @"C:\test";
            fileSystemWatcher.Filter = "*.txt";

            // Filesystemwatcher aktivieren
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        protected override void OnStart(string[] args)
        {
            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
            fileSystemWatcher.Created += new FileSystemEventHandler(FileSystemWatcher_Created);
            fileSystemWatcher.Path = @"C:\test";
            fileSystemWatcher.Filter = "*.txt";

            // Filesystemwatcher aktivieren
            fileSystemWatcher.EnableRaisingEvents = true;

        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnStop()
        {
        }
    }
}
