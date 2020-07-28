using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Cleaner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> events = new List<string>();
        bool files = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void process_kill(object sender, RoutedEventArgs e)
        {
            files = false;
            eventList.Items.Clear();
            Dispatcher.BeginInvoke((Action)(() =>
            {
                events.Add($"__________{DateTime.Now.ToLongTimeString()}__________");
                foreach (var item in Process.GetProcesses().Where(name=>name.ProcessName!="devenv"&&name.ProcessName!="explorer"&&name.ProcessName!=Process.GetCurrentProcess().ProcessName))
                {
                    try
                    {
                        item.Kill();
                        events.Add($"{item.ProcessName} succesfully killed");
                        eventList.Items.Add($"{item.ProcessName} succesfully killed");
                    }
                    catch(Exception)
                    {
                        events.Add($"{item.ProcessName} kill failed");
                        eventList.Items.Add($"{item.ProcessName} succesfully killed");
                    }
                }
                events.Add("________________________________________");
            }
            ));
        }

        private void down_del(object sender, RoutedEventArgs e)
        {
            files = false;
            eventList.Items.Clear();
            Dispatcher.BeginInvoke((Action)(() =>
            {
                events.Add($"__________{DateTime.Now.ToLongTimeString()}__________");
                foreach (var item in Directory.GetFiles(@"C:\Users\Pavlo\Downloads"))
                {
                    try
                    {
                        File.Delete(item);
                        events.Add($"{item} deleted");
                        eventList.Items.Add($"{item} deleted");
                    }
                    catch(Exception)
                    {
                        events.Add($"{item} didn`t deleted");
                        eventList.Items.Add($"{item} didn`t deleted");
                    }
                }
                foreach (var item in Directory.GetDirectories(@"C:\Users\Pavlo\Downloads"))
                {
                    try
                    {
                        File.Delete(item);
                        events.Add($"{item} deleted");
                        eventList.Items.Add($"{item} deleted");
                    }
                    catch (Exception)
                    {
                        events.Add($"{item} didn`t deleted");
                        eventList.Items.Add($"{item} didn`t deleted");
                    }
                }
                events.Add("________________________________________");
            }));
        }

        private void tmp_del(object sender, RoutedEventArgs e)
        {
            files = false;
            eventList.Items.Clear();
            Dispatcher.BeginInvoke((Action)(() =>
            {
                events.Add($"__________{DateTime.Now.ToLongTimeString()}__________");
                foreach (var item in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)+@"\Temp\"))
                {
                    try
                    {
                        File.Delete(item);
                        events.Add($"{item} deleted");
                        eventList.Items.Add($"{item} deleted");
                    }
                    catch (Exception)
                    {
                        events.Add($"{item} didn`t deleted");
                        eventList.Items.Add($"{item} didn`t deleted");
                    }
                }
                foreach (var item in Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\"))
                {
                    try
                    {
                        Directory.Delete(item);
                        events.Add($"{item} deleted");
                        eventList.Items.Add($"{item} deleted");
                    }
                    catch (Exception)
                    {
                        events.Add($"{item} didn`t deleted");
                        eventList.Items.Add($"{item} didn`t deleted");
                    }
                }
                events.Add("________________________________________");
            }));
        }

        private void closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string fname = $"log {DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString().Replace(':', '-')}.txt";
           
            using (StreamWriter sr = new StreamWriter(Environment.CurrentDirectory + "\\" + fname))
            {
                foreach (var item in events)
                {
                    sr.WriteLine(item);
                }
            }
        }

        private void showlogs(object sender, RoutedEventArgs e)
        {
            eventList.Items.Clear();
            files = true;
            foreach (var item in Directory.GetFiles(Environment.CurrentDirectory))
            {
                if (Path.GetFileName(item).Contains("log")) 
                {
                    eventList.Items.Add(Path.GetFileName(item));
                }
            }
        }

        private void chng(object sender, SelectionChangedEventArgs e)
        {
            if(files)
            {
                files = false;
                string fname = eventList.SelectedValue.ToString();
                //eventList.Items.Clear();
                
                string tmp = File.ReadAllText(Environment.CurrentDirectory+"\\"+fname);
                var arr = tmp.Split('\n');
                foreach (var item in arr)
                {
                    eventList.Items.Add(item);
                }
            }
        }
    }
}