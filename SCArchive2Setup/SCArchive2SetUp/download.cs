using Ionic.Zip;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SCArchive2SetUp
{
    public partial class MainWindow : MetroWindow
    {
        private void downloadZipFile(string link)
        {
            //temp폴더 생성
            if (!System.IO.Directory.Exists(updatetemppath)) System.IO.Directory.CreateDirectory(updatetemppath);
            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
                wc.DownloadFileAsync(
                    // Param1 = Link of file
                    new System.Uri(link),
                    // Param2 = Path to save
                    zipfilepath
                );
            }
        }

        private void Wc_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if(e.Error != null)
            {
                ErrorMesage.Text = "다운로드에 실패했습니다.\n" + e.Error.Message;
                statusChange(status.installerror);
                return;
            }

            statusChange(status.unzip);
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }



        Dictionary<string, string> addresslist = new Dictionary<string, string>();
        private void downloadVersion(string link)
        {

            using (WebClient wc = new WebClient())
            {
                string siteaddress = wc.DownloadString(new System.Uri("https://raw.githubusercontent.com/Buizz/SCArchive2/main/siteaddress"));

                string[] address = siteaddress.Split('\n');
                foreach (var item in address)
                {
                    string[] vals = item.Trim().Split('\\');
                    addresslist.Add(vals.First().Trim(), vals.Last().Trim());
                }
            }

            using (WebClient wc = new WebClient())
            {
                wc.DownloadStringCompleted += Wc_DownloadVersion;
                wc.DownloadStringAsync(new System.Uri(link));
            }
        }

        private void Wc_DownloadVersion(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ErrorMesage.Text = "버전 확인에 실패했습니다.\n" + e.Error.Message;
                statusChange(status.installerror);
                return;
            }
            string[] r = e.Result.Trim().Split('\n');


            version = r.First();
            onlinepath = r.Last();
            StatusMessage.Text = string.Format("현재 위치에 SCArchive2 {0}를 설치합니다.", version);

            Background.Visibility = Visibility.Collapsed;
        }

    }
}
