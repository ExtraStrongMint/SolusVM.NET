using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SolusVM
{
    // TODO: re-evaluate why on earth I would do this...
    internal class Requests
    {
        private string Hash { get; set; }
        private string Key { get; set; }
        private Uri URI { get; set; }
        private Dictionary<string, string> PostFields = new Dictionary<string, string>();

        public Requests(Uri uri, string key, string hash)
        {
            URI = uri;
            Key = key;
            Hash = hash;
            ResetPostFields();
        }

        internal async Task<string> Do(SolusClient.eAction action, bool vmstat)
        {
            if (vmstat.IsTrue() && PostFields.ContainsKey("status").IsFalse())
                PostFields.Add("status", "true");
            PostFields.Add("action", action.ToString().ToLower());

            using(HttpClient client = new HttpClient())
            {
                HttpContent content = new FormUrlEncodedContent(PostFields);
                ResetPostFields();

                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "SolusVM.NET 0.0.1.0 - github.com/ExtraStrongMint");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                content.Headers.ContentType.CharSet = "UTF-8";
                client.DefaultRequestHeaders.ExpectContinue = false;

                try
                {
                    HttpResponseMessage response = client.PostAsync(URI, content).Result;
                    client.Dispose();
                    return await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex) { return null; }
            }
        }

        internal async Task<string> Do(SolusClient.eAction action, SolusClient.eInfoAction infoAction, bool vmstat)
        {
            List<SolusClient.eInfoAction> infoActions = new List<SolusClient.eInfoAction>()
            {
                infoAction
            };
            
            return await Do(action, infoActions, vmstat);
        }

        // TODO: clean this up
        internal async Task<string> Do(SolusClient.eAction action, List<SolusClient.eInfoAction> infoActions, bool vmstat)
        {

            if (vmstat.IsTrue() && PostFields.ContainsKey("status").IsFalse())
                PostFields.Add("status", "true");

            foreach (SolusClient.eInfoAction infoAction in infoActions)
            {
                switch (infoAction)
                {
                    case SolusClient.eInfoAction.INFO_BW:
                        PostFields.Add("bw", "true");
                        break;
                    case SolusClient.eInfoAction.INFO_HDD:
                        PostFields.Add("hdd", "true");
                        break;
                    case SolusClient.eInfoAction.INFO_IPADDR:
                        PostFields.Add("ipaddr", "true");
                        break;
                    case SolusClient.eInfoAction.INFO_MEM:
                        PostFields.Add("mem", "true");
                        break;
                    case SolusClient.eInfoAction.INFO_ALL:
                        PostFields.Add("bw", "true");
                        PostFields.Add("hdd", "true");
                        PostFields.Add("ipaddr", "true");
                        PostFields.Add("mem", "true");
                        break;
                    default:
                        PostFields.Add("bw", "true");
                        PostFields.Add("hdd", "true");
                        PostFields.Add("ipaddr", "true");
                        PostFields.Add("mem", "true");
                        break;
                }
            }

            PostFields.Add("action", action.ToString().ToLower());


            using (HttpClient client = new HttpClient())
            {
                HttpContent content = new FormUrlEncodedContent(PostFields);
                ResetPostFields();

                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "SolusVM.NET 0.0.1.0 - github.com/ExtraStrongMint");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                content.Headers.ContentType.CharSet = "UTF-8";
                client.DefaultRequestHeaders.ExpectContinue = false;

                try
                {
                    HttpResponseMessage response = client.PostAsync(URI, content).Result;
                    client.Dispose();
                    return await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex) { return null; }
            }
        }

        private void ResetPostFields()
        {
            PostFields.Clear();
            PostFields.Add("key", Key);
            PostFields.Add("hash", Hash);
        }
    }
}
