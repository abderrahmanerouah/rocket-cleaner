using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using System.IO;
using System.IO.Compression;
using System;


namespace SmartScanUpdater
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            btn_start.Visibility = Visibility.Hidden;

            bar_progress.Visibility = Visibility.Visible;

            txt_status.Visibility = Visibility.Visible;

            Task.Run(() =>
            {
                for (int i = 0; i <= 100; i++)
                {
                    Thread.Sleep(50);
                    this.Dispatcher.Invoke(() => //Use Dispather to Update UI Immediately  
                    {
                        bar_progress.Value = i;
                        if (i == 100)
                        {

                            WebClient webClient = new WebClient();
                            var client = new WebClient();

                            //Thread.Sleep(5000);
                            string[] files = Directory.GetFiles(@"..\..\..\brief 3\bin\Release");

                            foreach (string file in files)
                            {
                                File.Delete(file);
                            }
                            //if crash , everything lost , best not delate and replace directly 

                            client.DownloadFile("https://docs.google.com/uc?export=download&id=122v33IOkdXSitA0ZIEa2FmC7qNmE-09i", @"..\..\..\brief 3\bin\Release\Release.zip");
                            string zipPath = @"..\..\..\brief 3\bin\Release\Release.zip";
                            //. pour racourcir 
                            string extractPath = @"..\..\..\brief 3\bin\Release";
                            ZipFile.ExtractToDirectory(zipPath, extractPath);

                            // not delate the zip file and leave it as backup by rename
                            File.Delete(@"..\..\..\brief 3\bin\Release\Release.zip");
                            //Process.Start(@"..\..\..\brief 3\bin\Release\brief 3.exe");
                            //this.Close();



                            btn_quiter.Visibility = Visibility.Visible;
                            txt_status.Text = "Mise à jour terminée !";
                        }

                    });
                }
            });
        }

        private void btn_quiter_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Process.Start(@"..\..\..\brief 3\bin\Release\brief 3.exe");
        }


    }
}
