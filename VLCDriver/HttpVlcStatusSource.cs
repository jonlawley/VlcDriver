using System;
using System.Net;
using System.Text;

namespace VLCDriver
{
    public interface IVlcStatusSource
    {
        string GetXml();
        string Url { get; set; }
    }

    public class HttpVlcStatusSource : IVlcStatusSource
    {
        public string Url { get; set; }

        public string GetXml()
        {
            using (var webClient = new WebClient())
            {
                var username = string.Empty;
                var password = Properties.Settings.Default.VlcHttpPassword;
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
                webClient.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
                return webClient.DownloadString(Url);
            }
        }
    }
}