using Ionic.Zip;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
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
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private bool IsUpdate;

        private string exepath;
        private string exeparetpath;
        private string scaexepath;
        private string updatetemppath;
        private string zipfilepath;

        private string onlinepath;
        private string version;
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string[] Args = Environment.GetCommandLineArgs();

            exepath = AppDomain.CurrentDomain.BaseDirectory;
            System.IO.DirectoryInfo topDir = new System.IO.DirectoryInfo(exepath);
            exeparetpath = topDir.Parent.FullName;
            scaexepath = topDir.Parent.FullName + @"\SCArchive 2.exe";
            updatetemppath = AppDomain.CurrentDomain.BaseDirectory + @"updatetemp";
            zipfilepath = updatetemppath + @"\updatefile";

            //https://raw.githubusercontent.com/Buizz/SCArchive2/main/version



            //SCA가 존재할 경우는 업데이트 아닐 경우 새로설치
            if (System.IO.File.Exists(scaexepath))
            {
                //업데이트로 실행한 경우
                IsUpdate = true;
            }
            else
            {
                //새로 설치B
                IsUpdate = false;
            }

            if (Args.Last() == "SETUP")
            {
                Background.Visibility = Visibility.Collapsed;
                IsUpdate = true;
                updatetemppath = exeparetpath + @"\updatetemp";
                zipfilepath = updatetemppath + @"\updatefile";


                try
                {
                    string setupfile = topDir.Parent.FullName + @"\SCArchive2SetUp.exe";
                    string setupconfigfile = topDir.Parent.FullName + @"\SCArchive2SetUp.exe.config";

                    if(System.IO.File.Exists(setupfile))
                    {
                        System.IO.File.Delete(setupfile);
                    }
                    if (System.IO.File.Exists(setupconfigfile))
                    {
                        System.IO.File.Delete(setupconfigfile);
                    }
                }
                catch (Exception)
                {

                }


                ZipFileList.Clear();
                using (ZipFile zip = new ZipFile(zipfilepath))
                {
                    zip.Password = "scarchive";
                    ZipFileList.AddRange(zip.EntryFileNames);
                }
                statusChange(status.movefile);
            }
            else
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                    | SecurityProtocolType.Tls11
                                    | SecurityProtocolType.Tls12
                                    | SecurityProtocolType.Ssl3;
                downloadVersion("https://raw.githubusercontent.com/Buizz/SCArchive2/main/version");
            }
        }


        private bool IsForceExit = false;
        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (IsForceExit) return;

            MessageBoxResult result = MessageBox.Show("설치를 종료하겠습니까?", "SCArchive2 Setup", MessageBoxButton.OKCancel);

            if(result != MessageBoxResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void Install_Click(object sender, RoutedEventArgs e)
        {
            statusChange(status.download);
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ExcetionSetting_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(addresslist["vaccinesetting"]);
            //Process.Start("https://scarchive.kr/bugreport/vaccinesetting.php");
        }

        private void startSCA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(scaexepath);
                Close();
            }
            catch (Exception ex)
            {
                StatusMessage.Text = "SCA를 실행 할 수 없습니다. \n백신 예외 설정을 하고 실행해주세요.";
                ErrorMesage.Text = ex.Message;
                excetionSettingWindow.Visibility = Visibility.Visible;
            }
        }
    }
}
