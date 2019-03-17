using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace AutoConnectWifi
{
    class Program
    {
        [DllImport("wininet.dll", EntryPoint = "InternetGetConnectedState")]

        //if network is okay, return true; otherwise return false;
        public extern static bool InternetGetConnectedState(out int conState, int reder);

        private const string argInterval = "--interval";
        private const string argWifi = "--wifi";

        static void Main(string[] args)
        {
            #region parse arguments
            int interval = 10;
            string wifiName = string.Empty;

            if (args != null && args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == argInterval)
                    {
                        if (i + 1 < args.Length)
                        {
                            int.TryParse(args[i + 1], out interval);
                        }
                    }
                    else if (args[i] == argWifi)
                    {
                        if (i + 1 < args.Length)
                        {
                            wifiName = args[i + 1];
                        }
                    }
                }
            }

            #endregion

            #region connect wifi in loop
            while (!string.IsNullOrWhiteSpace(wifiName))
            {
                Console.Write(DateTime.Now.ToString("HH:mm:ss"));

                int connectState = -1;
                if (InternetGetConnectedState(out connectState, 0))
                {
                    Console.WriteLine(" network is okay");
                }
                else
                {
                    Console.WriteLine($" connect wifi : {wifiName}");
                    ConnectWifi(wifiName);
                }

                Thread.Sleep(1000 * interval);
            }
            #endregion
        }

        /// <summary>
        /// connect wifi
        /// </summary>
        /// <param name="wifiName"></param>
        private static void ConnectWifi(string wifiName)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.Arguments = $"/C netsh wlan connect {wifiName}";
            cmd.Start();
        }
    }
}
