using System;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace Tarsier.Networking
{
    public class NetworkMonitor
    {

        private static NetworkMonitor instance;
        public static NetworkMonitor Instance {
            get {
                return instance = new NetworkMonitor();
            }
        }

        private BackgroundWorker pingWorker;
        public int TimeDelay { get; set; } = 3000;

        private string[] pingServers = new string[4]{
            "https://www.skype.com/",
            "https://tarsier-marianz.blogspot.com/",
            "https://www.youtube.com/",
            "https://github.com/marianz-bonfire"
        };

        private bool isConnected = false;

        public bool IsConnected {
            get {
                return isConnected;
            }
        }

        public event Action<bool> StatusChange;

        public NetworkMonitor() {
            Ping();
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerSupportsCancellation = true;
            pingWorker = backgroundWorker;
            pingWorker.DoWork += PingWorker_DoWork;
            pingWorker.RunWorkerAsync();
        }

        private void PingWorker_DoWork(object sender, DoWorkEventArgs e) {
            while (!pingWorker.CancellationPending) {
                bool pingedFlag = false;
                string[] array = pingServers;
                foreach (string ip in array) {
                    try {
                        if (Ping(ip)) {
                            pingedFlag = true;
                            break;
                        }
                    } catch (Exception ex) {
                        string message = ip + ": " + ex.Message;
                    }
                }
                if (isConnected != pingedFlag && this.StatusChange != null) {
                    isConnected = pingedFlag;
                    this.StatusChange(isConnected);
                }
                Thread.Sleep(TimeDelay);
            }
        }

        private void Ping() {
            try {
                string[] array = pingServers;
                foreach (string ip in array) {
                    if (Ping(ip)) {
                        isConnected = true;
                        break;
                    }
                }
            } catch {
            }
        }

        public bool Ping(string ip) {
            Ping ping = new Ping();
            PingOptions pingOptions = new PingOptions();
            pingOptions.DontFragment = true;
            string requestBody = "Hello Tarsier!";
            byte[] bytes = Encoding.ASCII.GetBytes(requestBody);
            int timeout = 5000;
            if (ping.Send(ip, timeout, bytes, pingOptions).Status == IPStatus.Success) {
                return true;
            }
            return false;
        }
    }
}
