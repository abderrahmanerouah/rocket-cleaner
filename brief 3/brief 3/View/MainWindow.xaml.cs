using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;



namespace brief_3
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Make a reference to a directory.
            DirectoryInfo di = new DirectoryInfo(Path.GetTempPath());
            // Get a reference to each file in that directory.
            FileInfo[] fiArr = di.GetFiles();
            var size = di.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
            txt_espace_a_nett.Text = $"Espace à nettoyer :  {size} Octets";


            //Display last analyse date in top of app using split read last line
            string wktA = File.ReadLines(@"..\History\Analyse.txt").Last();
            string[] dateA = wktA.Split(';');
            string ladateA = dateA[1];
            txt_last_analyse.Text = "Dernière analyse   : " + ladateA;


            //Display last analyse date in top of app using split read last line
            string wktD = File.ReadLines(@"..\History\Delete.txt").Last();
            string[] dateB = wktD.Split(';');
            string ladateD = dateB[1];
            txt_last_maj.Text = "Dernier nettoyage : " + ladateD;


        }



        //Bouton analyser 1 /////////////////////////////////////////////////////
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txt_output.Items.Clear();
            System.Windows.Forms.MessageBox.Show("Commencer l'analyse ?");

            //Verifier qu'il y a des fichiers à suprimer sinon message
            if (Directory.GetFileSystemEntries(Path.GetTempPath(), "*.*", SearchOption.AllDirectories).Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Il n'y a auccun fichier!");

            }
            else
            {
                txt_analyse.Text = "Analyse en cours";
                btn_vue.IsEnabled = false;
                btn_analyse1.IsEnabled = false;
                btn_histo1.IsEnabled = false;
                btn_option.IsEnabled = false;
                btn_analyse.Content = "Analyse en cours";
                btn_analyse.IsEnabled = false;

                btn_netoyerGrid.Visibility = Visibility.Hidden;
                btn_histoGrid.Visibility = Visibility.Hidden;
                btn_majGrid.Visibility = Visibility.Hidden;
                img_nett_grid.Visibility = Visibility.Hidden;
                img_histo_grid.Visibility = Visibility.Hidden;
                img_maj_grid.Visibility = Visibility.Hidden;
                btn_clear__histo.Visibility = Visibility.Hidden;
                txt_espace_a_nett.Visibility = Visibility.Hidden;
                txt_last_analyse.Visibility = Visibility.Hidden;
                txt_last_maj.Visibility = Visibility.Hidden;


                bar_progress.Visibility = Visibility.Visible;


                // Make a reference to a directory.
                DirectoryInfo di = new DirectoryInfo(Path.GetTempPath());
                double fileCount = di.GetFiles().Count();
                bar_progress.Value = 0;
                bar_progress.Maximum = fileCount;



                Task.Run(() =>
                {

                    for (int i = 0; i <= fileCount + 1; i++)
                    {
                        Thread.Sleep(500);
                        this.Dispatcher.Invoke(() => //Use Dispather to Update UI Immediately  
                        {

                            //Convert progress bar progress to %
                            double percent = (i / bar_progress.Maximum) * 100;
                            txt_pb_percent.Visibility = Visibility.Visible;
                            double percentRound = Math.Round(percent, 1);
                            txt_pb_percent.Text = percentRound.ToString() + "%";
                            bar_progress.Value = i;

                            if (i == (fileCount + 1))
                            {
                                txt_output.Visibility = Visibility.Visible;

                                // Get a reference to each file in that directory.
                                FileInfo[] fiArr = di.GetFiles("*.*", SearchOption.AllDirectories);

                                if (Directory.GetFileSystemEntries(Path.GetTempPath()).Length == 0)
                                {
                                    txt_output.Items.Add("There is no files ! ");
                                }
                                else
                                {
                                    // Display the names and sizes of the files.
                                    txt_output.Items.Add($"The directory {di.Name} contains the following files:\n");

                                    foreach (FileInfo f in fiArr)
                                    {
                                        txt_output.Items.Add($"The size of {f.Name}  is {f.Length} Octets. \n");
                                    }

                                }

                                var size = di.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
                                txt_espace_a_nett.Text = $"Espace à nettoyer :  {size} Octets";

                                txt_last_analyse.Text = "Dernière analyse   ; " + DateTime.Now.ToString();


                                //Add record to history
                                File.AppendAllText(@"..\History\Analyse.txt", "Analyse réalisée le ; " + DateTime.Now.ToString() + Environment.NewLine);

                                //Close the file
                                //sw.Close();

                                txt_analyse.Text = "Analyse terminée !";
                                btn_analyse.Visibility = Visibility.Hidden;
                                btn_back__db.Visibility = Visibility.Visible;
                                btn_vue.IsEnabled = true;
                                btn_analyse1.IsEnabled = true;
                                btn_histo1.IsEnabled = true;
                                btn_option.IsEnabled = true;
                                bar_progress.Visibility = Visibility.Hidden;

                                txt_pb_percent.Visibility = Visibility.Hidden;

                                System.Windows.Forms.MessageBox.Show("Analyse finie !");
                            }


                        });
                    }
                });
            }

        }



        //Bouton netoyer ( suprimer ) ////////////////////////////////////////////////
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            txt_output.Items.Clear();

            System.Windows.Forms.MessageBox.Show("Commencer le nettoyage ?");

            if (Directory.GetFileSystemEntries(Path.GetTempPath(), "*.*", SearchOption.AllDirectories).Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Il n'y a pas de fichier à supprimer !");
            }
            else
            {

                txt_analyse.Text = "Nettoyage en cours";
                btn_vue.IsEnabled = false;
                btn_analyse1.IsEnabled = false;
                btn_histo1.IsEnabled = false;
                btn_option.IsEnabled = false;
                btn_analyse.Visibility = Visibility.Hidden;

                btn_netoyerGrid.Visibility = Visibility.Hidden;
                btn_histoGrid.Visibility = Visibility.Hidden;
                btn_majGrid.Visibility = Visibility.Hidden;
                img_nett_grid.Visibility = Visibility.Hidden;
                img_histo_grid.Visibility = Visibility.Hidden;
                img_maj_grid.Visibility = Visibility.Hidden;
                btn_clear__histo.Visibility = Visibility.Hidden;
                txt_espace_a_nett.Visibility = Visibility.Hidden;
                txt_last_analyse.Visibility = Visibility.Hidden;
                txt_last_maj.Visibility = Visibility.Hidden;


                bar_progress.Visibility = Visibility.Visible;


                // Make a reference to a directory.
                DirectoryInfo di = new DirectoryInfo(Path.GetTempPath());
                double fileCount = di.GetFiles("*.*", SearchOption.AllDirectories).Count();
                bar_progress.Value = 0;
                bar_progress.Maximum = fileCount;

                Task.Run(() =>
                {

                    for (int i = 0; i <= fileCount + 1; i++)
                    {
                        Thread.Sleep(500);
                        this.Dispatcher.Invoke(() => //Use Dispather to Update UI Immediately  
                        {

                            //Conver progression to %
                            double percent = (i / bar_progress.Maximum) * 100;
                            txt_pb_percent.Visibility = Visibility.Visible;
                            double percentRound = Math.Round(percent, 1);
                            txt_pb_percent.Text = percentRound.ToString() + "%";
                            bar_progress.Value = i;

                            if (i == (fileCount + 1))
                            {
                                txt_output.Visibility = Visibility.Visible;

                                string[] files = Directory.GetFiles(Path.GetTempPath(), "*.*", SearchOption.AllDirectories);

                                try
                                {
                                    foreach (string file in files)
                                    {
                                        try
                                        {
                                            File.Delete(file);

                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);

                                        }
                                    }
                                    foreach (DirectoryInfo dir in di.GetDirectories("*.*", SearchOption.AllDirectories))
                                    {
                                        try
                                        {
                                            dir.Delete(true);
                                            Directory.Delete(dir.FullName);
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);

                                        }

                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);

                                }

                                txt_output.Items.Add($" The folder is empty ! \n");


                                // Get a reference to each file in that directory.
                                FileInfo[] fiArr = di.GetFiles("*.*", SearchOption.AllDirectories);
                                var size = di.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);

                                txt_espace_a_nett.Text = $"Espace à nettoyer :  {size} Octets";

                                txt_last_maj.Text = "Dernier nettoyage ; " + DateTime.Now.ToString();

                                //Keep record in histo
                                File.AppendAllText(@"..\History\Delete.txt", "Nettoyage réalisé le ; " + DateTime.Now.ToString() + Environment.NewLine);


                                txt_analyse.Text = "Nettoyage terminé !";
                                btn_analyse.Visibility = Visibility.Hidden;
                                btn_back__db.Visibility = Visibility.Visible;
                                btn_vue.IsEnabled = true;
                                btn_analyse1.IsEnabled = true;
                                btn_histo1.IsEnabled = true;
                                btn_option.IsEnabled = true;
                                bar_progress.Visibility = Visibility.Hidden;

                                txt_pb_percent.Visibility = Visibility.Hidden;

                                System.Windows.Forms.MessageBox.Show("Nettoyage términé !");
                            }
                        });
                    }
                });
            }
        }




        //Bouton analyser 2 //////////////////////////////////////////////////////////
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            txt_output.Items.Clear();
            System.Windows.Forms.MessageBox.Show("Commencer l'analyse ?");

            //Verifier qu'il y a des fichiers à suprimer sinon message
            if (Directory.GetFileSystemEntries(Path.GetTempPath(), "*.*", SearchOption.AllDirectories).Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Il n'y a auccun fichier!");

            }
            else
            {
                txt_analyse.Text = "Analyse en cours";
                btn_vue.IsEnabled = false;
                btn_analyse1.IsEnabled = false;
                btn_histo1.IsEnabled = false;
                btn_option.IsEnabled = false;
                btn_analyse.Content = "Analyse en cours";
                btn_analyse.IsEnabled = false;

                btn_netoyerGrid.Visibility = Visibility.Hidden;
                btn_histoGrid.Visibility = Visibility.Hidden;
                btn_majGrid.Visibility = Visibility.Hidden;
                img_nett_grid.Visibility = Visibility.Hidden;
                img_histo_grid.Visibility = Visibility.Hidden;
                img_maj_grid.Visibility = Visibility.Hidden;
                btn_clear__histo.Visibility = Visibility.Hidden;
                txt_espace_a_nett.Visibility = Visibility.Hidden;
                txt_last_analyse.Visibility = Visibility.Hidden;
                txt_last_maj.Visibility = Visibility.Hidden;


                bar_progress.Visibility = Visibility.Visible;


                // Make a reference to a directory.
                DirectoryInfo di = new DirectoryInfo(Path.GetTempPath());
                double fileCount = di.GetFiles().Count();
                bar_progress.Value = 0;
                bar_progress.Maximum = fileCount;



                Task.Run(() =>
                {

                    for (int i = 0; i <= fileCount + 1; i++)
                    {
                        Thread.Sleep(500);
                        this.Dispatcher.Invoke(() => //Use Dispather to Update UI Immediately  
                        {

                            //Convert progress bar progress to %
                            double percent = (i / bar_progress.Maximum) * 100;
                            txt_pb_percent.Visibility = Visibility.Visible;
                            double percentRound = Math.Round(percent, 1);
                            txt_pb_percent.Text = percentRound.ToString() + "%";
                            bar_progress.Value = i;

                            if (i == (fileCount + 1))
                            {
                                txt_output.Visibility = Visibility.Visible;

                                // Get a reference to each file in that directory.
                                FileInfo[] fiArr = di.GetFiles("*.*", SearchOption.AllDirectories);

                                if (Directory.GetFileSystemEntries(Path.GetTempPath()).Length == 0)
                                {
                                    txt_output.Items.Add("There is no files ! ");
                                }
                                else
                                {
                                    // Display the names and sizes of the files.
                                    txt_output.Items.Add($"The directory {di.Name} contains the following files:\n");

                                    foreach (FileInfo f in fiArr)
                                    {
                                        txt_output.Items.Add($"The size of {f.Name}  is {f.Length} Octets. \n");
                                    }

                                }

                                var size = di.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
                                txt_espace_a_nett.Text = $"Espace à nettoyer :  {size} Octets";

                                txt_last_analyse.Text = "Dernière analyse   ; " + DateTime.Now.ToString();


                                //Add record to history
                                File.AppendAllText(@"..\History\Analyse.txt", "Analyse réalisée le ; " + DateTime.Now.ToString() + Environment.NewLine);

                                //Close the file
                                //sw.Close();

                                txt_analyse.Text = "Analyse terminée !";
                                btn_analyse.Visibility = Visibility.Hidden;
                                btn_back__db.Visibility = Visibility.Visible;
                                btn_vue.IsEnabled = true;
                                btn_analyse1.IsEnabled = true;
                                btn_histo1.IsEnabled = true;
                                btn_option.IsEnabled = true;
                                bar_progress.Visibility = Visibility.Hidden;

                                txt_pb_percent.Visibility = Visibility.Hidden;

                                System.Windows.Forms.MessageBox.Show("Analyse finie !");
                            }


                        });
                    }
                });
            }

        }



        //Bouton historique 1 //////////////////////////////////////////
        private void btn_histo1_Click(object sender, RoutedEventArgs e)
        {
            txt_output.Items.Clear();
            btn_netoyerGrid.Visibility = Visibility.Hidden;
            btn_histoGrid.Visibility = Visibility.Hidden;
            btn_majGrid.Visibility = Visibility.Hidden;
            img_nett_grid.Visibility = Visibility.Hidden;
            img_histo_grid.Visibility = Visibility.Hidden;
            img_maj_grid.Visibility = Visibility.Hidden;

            txt_output.Visibility = Visibility.Visible;
            btn_clear__histo.Visibility = Visibility.Visible;


            //Creer Histo
            string[] files = { @"..\History\Analyse.txt", @"..\History\Delete.txt" };

            FileStream outputFile = new FileStream(@"..\History\histo.txt", FileMode.Create);

            using (BinaryWriter ws = new BinaryWriter(outputFile))
            {
                foreach (string file in files)
                {

                    txt_output.Items.Add(File.ReadAllText(file)); ;
                }
            }

            String line;

            //Pass the file path and file name to the StreamReader constructor
            StreamReader sr = new StreamReader(@"..\History\histo.txt");

            //Read the first line of text
            line = sr.ReadLine();

            //Continue to read until you reach end of file
            while (line != null)
            {
                //write the lie to console window
                txt_output.Items.Add(line);
                //Read the next line
                line = sr.ReadLine();
            }
            //close the file
            sr.Close();

        }


        //Bouton Historique 2 grid //////////////////////////////////////////
        private void btn_histoGrid_Click(object sender, RoutedEventArgs e)
        {
            txt_output.Items.Clear();
            btn_netoyerGrid.Visibility = Visibility.Hidden;
            btn_histoGrid.Visibility = Visibility.Hidden;
            btn_majGrid.Visibility = Visibility.Hidden;
            img_nett_grid.Visibility = Visibility.Hidden;
            img_histo_grid.Visibility = Visibility.Hidden;
            img_maj_grid.Visibility = Visibility.Hidden;

            txt_output.Visibility = Visibility.Visible;
            btn_clear__histo.Visibility = Visibility.Visible;


            //Creer Histo
            string[] files = { @"..\History\Analyse.txt", @"..\History\Delete.txt" };

            FileStream outputFile = new FileStream(@"..\History\histo.txt", FileMode.Create);

            using (BinaryWriter ws = new BinaryWriter(outputFile))
            {
                foreach (string file in files)
                {

                    txt_output.Items.Add(File.ReadAllText(file)); ;
                }
            }

            String line;

            //Pass the file path and file name to the StreamReader constructor
            StreamReader sr = new StreamReader(@"..\History\histo.txt");

            //Read the first line of text
            line = sr.ReadLine();

            //Continue to read until you reach end of file
            while (line != null)
            {
                //write the lie to console window
                txt_output.Items.Add(line);
                //Read the next line
                line = sr.ReadLine();
            }
            //close the file
            sr.Close();

        }


        //Bouton retourner à l'accueil //////////////////////////////////////////
        private void btn_back__db_Click(object sender, RoutedEventArgs e)
        {
            txt_output.Visibility = Visibility.Hidden;

            img_nett_grid.Visibility = Visibility.Visible;
            img_histo_grid.Visibility = Visibility.Visible;
            img_maj_grid.Visibility = Visibility.Visible;
            btn_netoyerGrid.Visibility = Visibility.Visible;
            btn_histoGrid.Visibility = Visibility.Visible;
            btn_majGrid.Visibility = Visibility.Visible;
            btn_back__db.Visibility = Visibility.Hidden;
            btn_analyse.Visibility = Visibility.Visible;
            btn_clear__histo.Visibility = Visibility.Hidden;
            btn_analyse.Content = "ANALYSER";
            btn_analyse.IsEnabled = true;

            txt_espace_a_nett.Visibility = Visibility.Visible;
            txt_last_analyse.Visibility = Visibility.Visible;
            txt_last_maj.Visibility = Visibility.Visible;

            txt_output.Items.Clear();

        }

        //Bouton Vue d'ensemble //////////////////////////////////////////
        private void btn_vue_Click(object sender, RoutedEventArgs e)
        {
            txt_output.Visibility = Visibility.Hidden;

            img_nett_grid.Visibility = Visibility.Visible;
            img_histo_grid.Visibility = Visibility.Visible;
            img_maj_grid.Visibility = Visibility.Visible;
            btn_netoyerGrid.Visibility = Visibility.Visible;
            btn_histoGrid.Visibility = Visibility.Visible;
            btn_majGrid.Visibility = Visibility.Visible;
            btn_back__db.Visibility = Visibility.Hidden;
            btn_analyse.Visibility = Visibility.Visible;
            btn_clear__histo.Visibility = Visibility.Hidden;
            btn_analyse.Content = "ANALYSER";
            btn_analyse.IsEnabled = true;

            txt_espace_a_nett.Visibility = Visibility.Visible;
            txt_last_analyse.Visibility = Visibility.Visible;
            txt_last_maj.Visibility = Visibility.Visible;

            txt_output.Items.Clear();

        }

        //Clear history button 
        private void btn_clear__histo_Click(object sender, RoutedEventArgs e)
        {
            txt_output.Items.Clear();

            //File.WriteAllText(@"C:\Users\Youcode\source\repos\brief 3\brief 3\bin\History\Analyse.txt", string.Empty);
            //File.WriteAllText(@"C:\Users\Youcode\source\repos\brief 3\brief 3\bin\History\Delete.txt", string.Empty);
            File.WriteAllText(@"..\History\histo.txt", string.Empty);
        }


        // Option button
        private void btn_option_Click(object sender, RoutedEventArgs e)
        {
            txt_output.Visibility = Visibility.Hidden;

            img_nett_grid.Visibility = Visibility.Visible;
            img_histo_grid.Visibility = Visibility.Visible;
            img_maj_grid.Visibility = Visibility.Visible;
            btn_netoyerGrid.Visibility = Visibility.Visible;
            btn_histoGrid.Visibility = Visibility.Visible;
            btn_majGrid.Visibility = Visibility.Visible;
            btn_back__db.Visibility = Visibility.Hidden;
            btn_analyse.Visibility = Visibility.Visible;
            btn_clear__histo.Visibility = Visibility.Hidden;
            btn_analyse.Content = "ANALYSER";
            btn_analyse.IsEnabled = true;

            txt_output.Items.Clear();
        }


        // Oppen the updater app
        private void btn_majGrid_Click(object sender, RoutedEventArgs e)
        {


            WebClient webClient = new WebClient();

           
                if (!webClient.DownloadString("https://pastebin.com/sWt7jMj3").Contains("v0.0.0"))
                {
                    if (System.Windows.Forms.MessageBox.Show("Looks like there is an update! Do you want to download it?", "Demo", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes) using (var client = new WebClient())

                        {
                            Process.Start(@"..\..\..\SmartScanUpdater\bin\Release\SmartScanUpdater.exe");
                            
                            this.Close();
                        }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Vous êtes déjà à jour !");
                }
            
            
        }
    }
}
