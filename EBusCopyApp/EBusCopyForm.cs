using EBusCopyApp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EBusCopyApp
{
    public partial class EBusCopyForm : Form
    {
        public EBusCopyForm()
        {
            InitializeComponent();
        }

        private BackgroundWorker worker;
        public static object thisLock = new object();
        private ReaderWriterLockSlim fileLock = new ReaderWriterLockSlim();
        private Helper helper = new Helper();

        private void EBusCopyForm_Load(object sender, EventArgs e)
        {
            try
            {
                worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(StartProcess);
                InitializeRefreshTimer();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void StartProcess(object sender, EventArgs e)
        {
            List<string> sources = Constants.SourcePathKeys.Split(',').ToList();
            sources.ForEach(x =>
            {
                string inputFilePath = Constants.GetConfiguration(x);
                string outputPath = Constants.GetConfiguration($"{x}MoveTo");
                string backupPath = Constants.GetConfiguration($"{x}BackUpTo");

                new Task(() =>
                {
                    ProcessFiles(inputFilePath, outputPath, backupPath);
                }).Start();

            });
        }

        private void ProcessFiles(string inputFilePath, string outputPath, string backupPath)
        {
            List<string> files = helper.DirSearch(inputFilePath);
            if (files.Any())
            {
                files.ForEach(x =>
                {
                    string fileName = Path.GetFileName(x);

                    if (!helper.IsFileLocked(x))
                    {
                        string newFileName = Path.GetFileNameWithoutExtension(x) + "_" + Guid.NewGuid();
                        string todayDate = DateTime.Now.ToString("dd_MM_yyyy");
                        helper.MoveFile(x, outputPath, newFileName);
                        helper.MoveFile(x, backupPath + "//" + todayDate, newFileName);
                        if (File.Exists(x))
                        {
                            File.Delete(x);
                        }
                    }
                });
            }
        }

        public void InitializeRefreshTimer()
        {
            System.Timers.Timer timer = new System.Timers.Timer(1000 * Convert.ToInt32(Constants.RefreshTimer) * 60);
            timer.Elapsed += TriggerStartProcess;
            timer.Start();
        }

        private void TriggerStartProcess(object sender, EventArgs e)
        {
            if (!worker.IsBusy)
            {
                worker.RunWorkerAsync();
            }
        }
    }
}
