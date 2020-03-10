using EBusCopyApp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
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
            //while (true)
            //{
                #region logTrigger
                List<string> files = helper.DirSearch(Constants.InputFilePath);
                if (files.Any())
                {
                    files.ForEach(x =>
                    {
                        string fileName = Path.GetFileName(x);

                        if (!helper.IsFileLocked(x))
                        {
                            string newFileName = Path.GetFileNameWithoutExtension(x) + "_" + Guid.NewGuid();
                            string todayDate = DateTime.Now.ToString("dd_MM_yyyy");
                            helper.MoveFile(x, Constants.OutPutFilePath, newFileName);
                            helper.MoveFile(x, Constants.BackUpFilePath + "//" + todayDate, newFileName);
                            if (File.Exists(x))
                            {
                                File.Delete(x);
                            }
                        }
                    });
                }
                #endregion
            //}
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
