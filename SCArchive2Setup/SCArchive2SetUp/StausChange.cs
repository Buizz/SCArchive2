using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SCArchive2SetUp
{
    public partial class MainWindow : MetroWindow
    {
        private enum status
        {
            installerror,
            vaccineerror,
            start,
            download,
            unzip,
            movefile,
            complete
        }

        private void statusChange(status s)
        {
            switch (s)
            {
                case status.vaccineerror:
                    StatusMessage.Text = "설치에 실패했습니다. \n백신 예외 설정을 하고 재설치 해주세요.";
                    excetionSettingWindow.Visibility = Visibility.Visible;
                    progressBar.Visibility = Visibility.Collapsed;
                    setupBtn.Visibility = Visibility.Visible;
                    setupBtn.Content = "다시 설치하기";
                    break;
                case status.installerror:
                    StatusMessage.Text = "설치에 실패했습니다.";
                    progressBar.Visibility = Visibility.Collapsed;
                    setupBtn.Visibility = Visibility.Visible;
                    setupBtn.Content = "다시 설치하기";
                    break;
                case status.start:
                    setupBtn.Content = "설치하기";

                    break;
                case status.unzip:
                    progressBar.IsIndeterminate = true;
                    StatusMessage.Text = "압축을 해제중입니다.";

                    unZipFile();
                    break;
                case status.movefile:
                    StatusMessage.Text = "파일을 검사중입니다.";

                    moveFile();
                    break;
                case status.complete:
                    IsForceExit = true;
                    progressBar.Visibility = Visibility.Collapsed;
                    StatusMessage.Text = "설치가 완료되었습니다.";
                    setupBtn.Visibility = Visibility.Collapsed;
                    cancelBtn.Visibility = Visibility.Collapsed;
                    excetionSettingWindow.Visibility = Visibility.Collapsed;
                    startSCABtn.Visibility = Visibility.Visible;

                    break;
                case status.download:
                    if (IsUpdate)
                    {
                        exepath = AppDomain.CurrentDomain.BaseDirectory;
                        System.IO.DirectoryInfo topDir = new System.IO.DirectoryInfo(exepath);
                        exeparetpath = topDir.Parent.FullName;
                        scaexepath = topDir.Parent.FullName + @"\SCArchive 2.exe";
                        updatetemppath = AppDomain.CurrentDomain.BaseDirectory + @"temp";
                        zipfilepath = updatetemppath + @"\updatetemp";
                    }
                   

                    ErrorMesage.Text = "";
                    setupBtn.Visibility = Visibility.Collapsed;
                    progressBar.Visibility = Visibility.Visible;
                    StatusMessage.Text = "프로그램을 다운로드 중입니다.";
                    
                    downloadZipFile(onlinepath);
                    break;
            }
        }

        

    }
}
