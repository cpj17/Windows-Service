using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Net;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        Timer objTimer = new Timer();
        string LogPath = "C:/Users/GSIAD-031/CPJ/Logs/";
        string FileName = DateTime.Now.ToString("ddMMyyyy") + "_Log.txt";
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            File.AppendAllText(LogPath + FileName, "SVC started : " + DateTime.Now.ToString("ddMMyyyy HH:mm:ss") + Environment.NewLine);

            objTimer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            objTimer.Interval = 2000;
            objTimer.Enabled = true;
            objTimer.Start();
        }

        private void Timer_Elapsed(object source, ElapsedEventArgs e)
        {
            try
            {
                Process[] pname = Process.GetProcessesByName("wscript");

                if (pname.Length == 0)
                {
                    if (CheckInternetConnection())
                    {
                        File.AppendAllText(LogPath + FileName, DateTime.Now.ToString("ddMMyyyy HH:mm:ss") + "  Support Script is not running right now. It will start now" + Environment.NewLine);
                        //Process.Start("C:/Users/GSIAD-031/CPJ/Scripts/Support.vbe");

                        var p = new Process();
                        p.StartInfo = new ProcessStartInfo("mspaint.exe")
                        {
                            UseShellExecute = true
                        };
                        p.Start();

                        File.AppendAllText(LogPath + FileName, DateTime.Now.ToString("ddMMyyyy HH:mm:ss") + "  Support Script is now running" + Environment.NewLine);
                    }
                    else
                    {
                        File.AppendAllText(LogPath + FileName, DateTime.Now.ToString("ddMMyyyy HH:mm:ss") + "  Internet is not there" + Environment.NewLine);
                    }
                }
            }
            catch (Exception objException)
            {
                File.AppendAllText(LogPath + FileName, DateTime.Now.ToString("ddMMyyyy HH:mm:ss") + "  " + objException.ToString() + Environment.NewLine);
            }
        }

        protected override void OnStop()
        {
            File.AppendAllText(LogPath + FileName, "SVC stopped : " + DateTime.Now.ToString("ddMMyyyy HH:mm:ss") + Environment.NewLine);
        }

        public static bool CheckInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
