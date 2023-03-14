using Ionic.Zip;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCArchive2SetUp
{
    public partial class MainWindow : MetroWindow
    {
        List<string> ZipFileList = new List<string>();
        public void Zip(string directoryPath, string zipPath, string password = null)
        {
            using (ZipFile zip = new ZipFile(zipPath))
            {
                zip.Password = password;
                zip.AddFile(directoryPath, string.Empty);
                zip.Save(zipPath);
            }
        }

        public List<string> UnZip(string zipFilePath, string unZipPath, string password = null)
        {
            List<string> rstr = new List<string>();
            using (ZipFile zip = new ZipFile(zipFilePath))
            {
                zip.Password = password;
                rstr.AddRange(zip.EntryFileNames);
                zip.ExtractAll(unZipPath, ExtractExistingFileAction.OverwriteSilently);
            }
            return rstr;
        }

        private void unZipFile()
        {
            try
            {
                ZipFileList = UnZip(zipfilepath, updatetemppath, "scarchive");
                //ZipFileList = UnZip(exepath + @"RatiborusKMSTools01.03.2023.ii.taiwebs.com.zip", updatetemppath, "taiwebs.com");
            }
            catch (Exception e)
            {
                ErrorMesage.Text = e.Message;
                statusChange(status.installerror);
                return;
            }

            ZipFileList.Sort((x, y) => x.Length.CompareTo(y.Length));
            statusChange(status.movefile);
        }

       
    }
}
