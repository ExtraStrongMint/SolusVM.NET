using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SolusVM
{
    public class SolusClient
    {
        #region Properties
        private string API_KEY { get; set; }
        private string API_HASH { get; set; }
        private string URI { get; set; }
        private int Port { get; set; }
        private bool SSL { get; set; }
        private Requests Request;

        public bool ReturnStatus { get; set; }
        #endregion

        public enum eAction
        {
            REBOOT,
            BOOT,
            SHUTDOWN,
            STATUS,
            INFO
        };

        public enum eInfoAction
        {
            INFO_IPADDR,
            INFO_HDD,
            INFO_MEM,
            INFO_BW,
            INFO_ALL
        }

        #region Constructor
        public SolusClient(string key, string hash, string uri, int port, bool ssl = false)
        {
            API_KEY = key;
            API_HASH = hash;
            URI = uri;
            Port = port;
            SSL = ssl;

            GoReady();
        }
        #endregion

        #region Public Functions
        public async Task<string> Action(eAction action)
        {
            string result = await Request.Do(action, ReturnStatus);
            if (result.IsNotNull())
                result = String.Format("<SolusVM.Net>{0}</SolusVM.Net>", result);
            return result;
        }

        public async Task<string> Action(eAction action, List<eInfoAction> infoAction)
        {
            string result = await Request.Do(action, infoAction, ReturnStatus);
            if (result.IsNotNull())
                result = String.Format("<SolusVM.Net>{0}</SolusVM.Net>", result);
            return result;
        }

        public async Task<string> Action(eAction action, eInfoAction infoAction)
        {
            string result = await Request.Do(action, infoAction, ReturnStatus);
            if (result.IsNotNull())
                result = String.Format("<SolusVM.Net>{0}</SolusVM.Net>", result);
            return result;
        }

        public dynamic ParseResult(string result)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(result);
            MemoryStream stream = new MemoryStream(byteArray);
            XmlSerializer serializer = new XmlSerializer(typeof(SolusResponse), new XmlRootAttribute("SolusVM.Net"));
            SolusResponse response = (SolusResponse) serializer.Deserialize(stream);
            return response;
        }
        #endregion

        #region Private Functions
        private void GoReady()
        {
            Uri uri;

            uri = new Uri(String.Format("{0}://{1}:{2}/api/client/command.php", (SSL.IsTrue()) ? "https" : "http", URI, Port));

            Request = new Requests(uri, API_KEY, API_HASH);
        }
        #endregion
    }

    public class SolusResponse
    {
        // default responses
        public string status { get; set; }
        public string statusmsg { get; set; }

        // on success
        public string hostname { get; set; }
        public string ipaddress { get; set; }

        // info only (comma separated items)
        public string ipaddr { get; set; }
        public string hdd { get; set; }
        public string mem { get; set; }
        public string bw { get; set; }

        // vmstat
        public string vmstat { get; set; }

    }
}
