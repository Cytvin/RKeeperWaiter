using System;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Threading.Tasks;

namespace RKeeperWaiter
{
    public enum DocumentType
    {
        Request,
        Response
    }

    public class NetworkService
    {
        private string _ip;
        private string _port;
        private string _login;
        private string _password;

        private Uri _uri;
        private string _authorizationString;

        public string Ip => _ip;
        public string Port => _port;
        public string Login => _login;
        public string Password => _password;

        private void MakeUrl()
        {
            _uri = new Uri($"https://{_ip}:{_port}/rk7api/v0/xmlinterface.xml");
        }

        private void MakeAuthorizationString()
        {
            _authorizationString = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{_login}:{_password}"));
        }

        public void SetParameters(string ip, string port, string login, string password)
        {
            _ip = ip;
            _port = port;
            _login = login;
            _password = password;

            MakeUrl();
            MakeAuthorizationString();
        }

        public XDocument SendRequest(XmlDocument xmlContent)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                return true;
            };

            XmlNode xmlElement = xmlContent.GetElementsByTagName("RK7Command").Item(0);
            string requestType = xmlElement.Attributes["CMD"].Value;

            //Log<XmlDocument>(xmlContent, requestType, DocumentType.Request);

            using (HttpClient httpClient = new HttpClient(httpClientHandler))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + _authorizationString);
                httpClient.Timeout = TimeSpan.FromSeconds(30);

                StreamContent requestContent = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(xmlContent.OuterXml)));

                Task<HttpResponseMessage> serverResponse = httpClient.PostAsync(_uri, requestContent);

                HttpResponseMessage response;

                try
                {
                    response = serverResponse.Result;
                }
                catch (AggregateException ex)
                {
                    throw new TaskCanceledException("Не получен ответ от сервера");
                }

                HttpStatusCode httpStatusCode = response.StatusCode;

                if (httpStatusCode == HttpStatusCode.OK)
                {
                    XDocument responseContent = XDocument.Parse(response.Content.ReadAsStringAsync().Result);

                    //Log<XDocument>(responseContent, requestType, DocumentType.Response);

                    return responseContent;
                }
                else if (httpStatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new Exception();
                }

                throw new Exception();
            }
        }

        private void Log<T>(T xmlContent, string cmdName, DocumentType type)
        {
            XmlDocument saveDocument = new XmlDocument();

            if (xmlContent is XDocument)
            {
                saveDocument = XDocumentToXml(xmlContent as XDocument);
            }
            else
            {
                saveDocument = xmlContent as XmlDocument;
            }

            DateTime requestSaveTime = DateTime.Now;
            saveDocument.Save($"D:\\sqllog\\{type}_{cmdName}_{requestSaveTime.Year}{requestSaveTime.Month}{requestSaveTime.Day}_" +
                $"{requestSaveTime.Hour}{requestSaveTime.Minute}{requestSaveTime.Second}{requestSaveTime.Millisecond}.xml");
        }

        private XmlDocument XDocumentToXml(XDocument xDocument)
        {
            XmlDocument resultXml = new XmlDocument();
            
            using (var xmlReader = xDocument.CreateReader())
            {
                resultXml.Load(xDocument.CreateReader());
            }

            return resultXml;
        }
    }
}
