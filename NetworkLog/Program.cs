using System;
using System.Threading;
using System.Net.NetworkInformation;

namespace NetworkLog
{
    internal class Program
    {
        static bool isDDOS, printed = false;
        static void Main(string[] args)
        {
            Timer t = new Timer(GetStat, null, 0, 1000);
            Console.ReadKey(true);
        }

        static void GetStat(object o)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
                return;
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface iface in interfaces)
            {
                //Change Ethernet to other network interface name you want to monitor
                if (iface.Name == "Ethernet")
                {
                    var init_received = iface.GetIPv4Statistics().BytesReceived;
                    Thread.Sleep(1000);
                    var current_received_value = iface.GetIPv4Statistics().BytesReceived;

                    var receivedMbps = (current_received_value - init_received) * 8 * Math.Pow(10, -6);

                    //if bandwitdth received over 500Mbps, a log message will be printed
                    if(receivedMbps >= 500)
                    {
                        isDDOS = true;
                        if(!printed)
                        {
                            printMessage(isDDOS, printed);
                            printed = true;
                        }
                    }
                    else
                    {
                        isDDOS = false;
                        printed = false;
                    }
                }
            }
        }

        static void printMessage(bool isDDOS, bool printed)
        {
            if(isDDOS && !printed)
            {
                Console.WriteLine($"[{DateTime.Now.ToString("dd-MM-yyyy H:mm:ss")}] LOG: Server bi ddos");
            }
        }
    }
}
