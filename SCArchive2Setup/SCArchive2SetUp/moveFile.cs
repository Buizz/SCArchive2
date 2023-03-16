using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SCArchive2SetUp
{
    public partial class MainWindow : MetroWindow
    {
        private void moveFile()
        {
            List<string> removedFile = new List<string>();

            bool IsInstallError = false;

            foreach (var file in ZipFileList)
            {
                if (!IsUpdate)
                {
                    //새로 설치일 경우 업데이터만 옮기고 여기서 종료하고 다시 실행한다.
                    if (!file.Contains("updater")) continue;
                }
                else
                {
                    if (file.Contains("updater")) continue;
                }


                string filepath = updatetemppath + "\\" + file.Replace("/", @"\");

                string movefilepath;
                if (IsUpdate)
                {
                    movefilepath = exeparetpath + "\\" + file.Replace("/", @"\");
                }
                else
                {
                    movefilepath = exepath + file.Replace("/", @"\");
                }

                if (filepath.Last() == '\\')
                {
                    //폴더
                    if (System.IO.Directory.Exists(filepath))
                    {
                        System.IO.Directory.CreateDirectory(movefilepath);
                    }
                    else
                    {
                        //파일없음
                        IsInstallError = true;
                        removedFile.Add(file);
                    }
                }
                else
                {
                    //파일
                    if (System.IO.File.Exists(filepath))
                    {
                        try
                        {
                            if (System.IO.File.Exists(movefilepath))
                            {
                                //옮길 위치에 파일이 있으면 삭제하고 덮어쒸운다.
                                System.IO.File.Delete(movefilepath);
                            }

                            System.IO.File.Move(filepath, movefilepath);
                        }
                        catch (Exception)
                        {
                            //백신오류
                            IsInstallError = true;
                            removedFile.Add(file);
                        }
                    }
                    else
                    {
                        //파일없음
                        IsInstallError = true;
                        removedFile.Add(file);
                    }
                }
            }


            if (IsInstallError)
            {
                string rt = "다음 파일들을 옮기는데 실패했습니다. ";
                int count = 0;
                foreach (var item in removedFile)
                {
                    if(count > 10)
                    {
                        rt += "\n" + ". . .";
                        break;
                    }
                    rt += "\n" +item ;
                    count++;
                }

                ErrorMesage.Text = rt;
                statusChange(status.vaccineerror);
            }
            else
            {
                if (!IsUpdate)
                {
                    IsForceExit = true;

                    Process.Start(exepath + @"updater\SCArchive2SetUp.exe", "SETUP");

                    Close();
                    return;
                }

                statusChange(status.complete);
            }
        }
    }
}
