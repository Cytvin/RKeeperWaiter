using System;
using System.Net.Http;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace RKeeperWaiter
{
    public enum Anchor
    {
        ForLicenseRequest,
        ForOrderRequest
    }
    public class License
    {
        private string _userName = "";
        private string _password = "";
        private string _integrationToken = "";
        private string _licenseId = "";
        private string _restaurantCode;
        private string _token = null;
        private string _seqNumber = "0";
        private Guid _applicationGuid;

        public License(int restaurantId, Guid applicationGuid)
        {
            _restaurantCode = restaurantId.ToString();
            _applicationGuid = applicationGuid;

            GetLicenseToken();
        }

        public XmlElement GetXMLLicense()
        {
            XmlDocument license = new XmlDocument();

            XmlElement licenseInfo = license.CreateElement("LicenseInfo");

            XmlAttribute anchor = license.CreateAttribute("anchor");
            anchor.Value = MakeAnchor(Anchor.ForOrderRequest);
            licenseInfo.Attributes.Append(anchor);

            XmlAttribute licenseToken = license.CreateAttribute("licenseToken");
            licenseToken.Value = _token;
            licenseInfo.Attributes.Append(licenseToken);

            XmlElement licenseInstance = license.CreateElement("LicenseInstance");

            XmlAttribute guid = license.CreateAttribute("guid");
            guid.Value = _applicationGuid.ToString();
            licenseInstance.Attributes.Append(guid);

            XmlAttribute seqNumber = license.CreateAttribute("seqNumber");
            seqNumber.Value = _seqNumber.ToString();
            licenseInstance.Attributes.Append(seqNumber);

            licenseInfo.AppendChild(licenseInstance);
            license.AppendChild(licenseInfo);

            return licenseInfo;
        }

        private string GetLicenseToken()
        {
            string userTokenCode = MakeUserTokenCode();
            string anchor = MakeAnchor(Anchor.ForLicenseRequest);

            string serverResponse = SendLicenseTokenRequest(userTokenCode, anchor);

            _token = ParsingResponse(serverResponse);

            return _token;
        }

        private string MakeUserTokenCode()
        {
            string lowerCaseMD5Token = MakeMD5LowercaseString(_integrationToken);
            string lowerCaseMD5User = MakeMD5LowercaseString(_userName + _password);

            string codeString = $"{_userName};{lowerCaseMD5User};{lowerCaseMD5Token}";
            return Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(codeString));
        }

        private string MakeMD5LowercaseString(string data)
        {
            MD5 md5 = MD5.Create();

            byte[] md5String = md5.ComputeHash(Encoding.ASCII.GetBytes(data));

            return BitConverter.ToString(md5String).Replace("-", "").ToLower();
        }

        private string MakeAnchor(Anchor type)
        {
            if (type == Anchor.ForLicenseRequest)
            {
                return $"6%3A{_licenseId}%23{_restaurantCode}/17";
            }

            return $"6:{_licenseId}#{_restaurantCode}/17";
        }

        private string SendLicenseTokenRequest(string userTokenCode, string anchor)
        {
            Uri licenseUri = new Uri($"https://l.ucs.ru/ls5api/api/License/GetLicenseIdByAnchor?anchor={anchor}");

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromSeconds(30);
                httpClient.DefaultRequestHeaders.Add("usr", userTokenCode);

                HttpResponseMessage response = httpClient.GetAsync(licenseUri).Result;

                HttpStatusCode httpStatusCode = response.StatusCode;

                if (httpStatusCode == HttpStatusCode.OK)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                else if (httpStatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new Exception();
                }

                throw new Exception();
            }
        }

        private string ParsingResponse(string response)
        {
            response = response.Trim(new char[] { '{', '}' });
            response = response.Split(',')[0];
            response = response.Split(':')[1];
            response = response.Trim('"');

            return response;
        }
    }
}
