using System;
using System.Net;
using System.Runtime.InteropServices;

namespace Tarsier.Networking
{
    /// <summary>
    /// Class that will check if internet is avaialable
    /// </summary>
    public class InternetAvailability
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);

        /// <summary>
        /// Method that returns TRUE if there is internet
        /// </summary>
        /// <returns>boolean</returns>
        public static bool IsInternetAvailable() {
            try {
                int description;
                return InternetGetConnectedState(out description, 0);
            } catch {
                return false;
            }
        }

        public static string GetPublicIpddress() {
            string pulicIpAddress = string.Empty;
            //Task.Factory.StartNew(() => {
            pulicIpAddress = InternetProtocolAddresses.GetExternalIPAddress();
            //});
            return pulicIpAddress;
        }

        public static string GetLocalIpddress() {
            IPHostEntry Host = default(IPHostEntry);
            string ipAddress = null;
            Host = Dns.GetHostEntry(Environment.MachineName);
            foreach (IPAddress IP in Host.AddressList) {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {
                    ipAddress = Convert.ToString(IP);
                }
            }
            return ipAddress;
        }

    }
}