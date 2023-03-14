﻿using System;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Security;

namespace RKeeperWaiter
{
    public class NetworkService
    {
        private string _ip;
        private string _port;
        private string _login;
        private string _password;

        private Uri _uri;
        private string _authorizationString;

        public string Ip { get { return _ip; } }
        public string Port { get { return _port; } }
        public string Login { get { return _login; } }
        public string Password { get { return _password; } }

        private void SetUrl()
        {
            _uri = new Uri($"https://{_ip}:{_port}/rk7api/v0/xmlinterface.xml");
        }

        private void SetAuthorization()
        {
            _authorizationString = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{_login}:{_password}"));
        }

        public void SetParameters(string ip, string port, string login, string password)
        {
            _ip = ip;
            _port = port;
            _login = login;
            _password = password;

            SetUrl();
            SetAuthorization();
        }

        public XDocument SendRequest(StringBuilder xmlContent)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                return true;
            };

            //XmlDocument requestContent = new XmlDocument();
            //requestContent.LoadXml(xmlContent.ToString());

            //DateTime requestSaveTime = DateTime.Now;
            //requestContent.Save($"D:\\sqllog\\Request_{requestSaveTime.Year}{requestSaveTime.Month}{requestSaveTime.Day}_" +
            //    $"{requestSaveTime.Hour}{requestSaveTime.Minute}{requestSaveTime.Second}{requestSaveTime.Millisecond}.xml");

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + _authorizationString);
                httpClient.Timeout = TimeSpan.FromSeconds(10);

                StreamContent requestContent = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(xmlContent.ToString())));

                HttpResponseMessage response = httpClient.PostAsync(_uri, requestContent).Result;
                HttpStatusCode httpStatusCode = response.StatusCode;

                if (httpStatusCode == HttpStatusCode.OK)
                {
                    XDocument responseContent = XDocument.Parse(response.Content.ReadAsStringAsync().Result);

                    //DateTime responseSaveTime = DateTime.Now;

                    //responseContent.Save($"D:\\sqllog\\Response_{responseSaveTime.Year}{responseSaveTime.Month}{responseSaveTime.Day}_" +
                    //    $"{responseSaveTime.Hour}{responseSaveTime.Minute}{responseSaveTime.Second}{responseSaveTime.Millisecond}.xml");

                    return responseContent;
                }
                else if (httpStatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new Exception();
                }

                throw new Exception();
            }
        }
    }
}
