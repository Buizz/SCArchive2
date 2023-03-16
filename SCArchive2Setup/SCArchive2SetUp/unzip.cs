using Ionic.Zip;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SCArchive2SetUp
{
    public partial class MainWindow : MetroWindow
    {
        List<string> ZipFileList = new List<string>();

        public void UnZip(string zipFilePath, string unZipPath, string password = null)
        {
            BackgroundWorker bg = new BackgroundWorker();

            bg.DoWork += Bg_DoWork;
            bg.RunWorkerCompleted += Bg_RunWorkerCompleted;

            string[] arg = { zipFilePath, unZipPath, password };
            bg.RunWorkerAsync(arg);
        }

        private void Bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Error != null)
            {
                ErrorMesage.Text = e.Error.Message;
                statusChange(status.installerror);
                return;
            }


            ZipFileList.Sort((x, y) => x.Length.CompareTo(y.Length));
            statusChange(status.movefile);
        }

        private void Bg_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] arg = (string[])e.Argument;
            string zipFilePath = arg[0];
            string unZipPath = arg[1];
            string password = arg[2];

            ZipFileList.Clear();
            using (ZipFile zip = new ZipFile(zipFilePath))
            {
                zip.Password = password;
                ZipFileList.AddRange(zip.EntryFileNames);
                zip.ExtractAll(unZipPath, ExtractExistingFileAction.OverwriteSilently);
            }
        }

        private void unZipFile()
        {
            UnZip(zipfilepath, updatetemppath, "scarchive");
        }

       
    }
}
