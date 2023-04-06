using System;
using System.Net.Http;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace RKeeperWaiter
{
    public class License
    {
        private string _userName = "";
        private string _password = "";
        private string _integrationToken = "";
        private string _licenseId = "";
        private string _restaurantCode;
        private string _token = null;

        public string Token => _token == null ? GetInstance() : _token;

        public License(string restaurantId)
        {
            _restaurantCode = restaurantId;
        }

        public string GetInstance()
        {
            string userTokenCode = MakeUserTokenCode();
            string anchor = MakeAnchor();

            string serverResponse = SendInstanceRequest(userTokenCode, anchor);

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

        private string MakeAnchor()
        {
            return $"6%3A{_licenseId}%23{_restaurantCode}/17";
        }

        private string SendInstanceRequest(string userTokenCode, string anchor)
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
