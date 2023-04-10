﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using RKeeperWaiter;
using RKeeperWaiter.Models;

namespace RKeeperWaiterTest
{
    internal class Program
    {
        private static Waiter _waiter;
        public static void Menu()
        {
            while (true)
            {
                Console.WriteLine("Введите код категории: ");
                int categoryId = int.Parse(Console.ReadLine());

                Console.WriteLine($"***** Объекты в категории {categoryId} *****");

                Menu menuCategory = _waiter.GetMenuCategory(categoryId);

                foreach (Category category in menuCategory.Categories)
                {
                    Console.WriteLine($"({category.ParentId}) {category.Id} - {category.Name}");
                }

                foreach (Dish dish in menuCategory.Dishes)
                {
                    Console.WriteLine($"({dish.ParentId}) {dish.Id} ({dish.Guid}) - {dish.Name} : {dish.Price}");
                }
            }
        }

        public static void DisplayOrders(List<Order> orders)
        {
            foreach (Order order in orders)
            {
                Console.WriteLine();
                Console.WriteLine($"{order.Id} | {order.Name}: {order.Guid}");

                if (order.CommonDishes.Count() > 0)
                {
                    Console.WriteLine("Общие блюда: ");
                    foreach (Dish dish in order.CommonDishes)
                    {
                        Console.WriteLine($"{dish.Id} | {dish.Name} : {dish.Price}");
                    }
                }

                Console.WriteLine("Гости: ");
                foreach (Guest guest in order.Guests)
                {
                    Console.WriteLine($"Guest: {guest.Label}");

                    Console.WriteLine("Блюда гостя: ");
                    foreach (Dish dish in guest.Dishes)
                    {
                        Console.WriteLine($"{dish.Id} | {dish.Name} : {dish.Price}");
                    }
                }
            }
        }

        public static XmlDocument CreateXml()
        {
            StringBuilder stringBuilder = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(stringBuilder))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("RK7Query");

                writer.WriteStartElement("RK7Command");
                writer.WriteAttributeString("CMD", "CreateOrder");

                writer.WriteStartElement("Table");
                writer.WriteAttributeString("code", "12");

                writer.WriteStartElement("Seat");
                writer.WriteAttributeString("code", "20");

                writer.WriteStartElement("Person");
                writer.WriteAttributeString("code", "84");
                writer.WriteEndElement();

                writer.WriteEndElement();

                writer.WriteEndElement();

                writer.WriteEndElement();

                writer.WriteEndElement();
            }

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(stringBuilder.ToString());
            return xml;
        }

        public static XmlNode GetLastestChildNode(XmlElement xmlElement)
        {
            if (xmlElement.LastChild == null)
            {
                return xmlElement;
            }

            return GetLastestChildNode(xmlElement.LastChild as XmlElement);
        }

        public static void TestMD5()
        {
            MD5 mD5 = MD5.Create();

            string userName = "";
            string password = "";
            string token = "";

            byte[] md5Token = mD5.ComputeHash(Encoding.ASCII.GetBytes(token));

            string lowerCaseMD5Token = BitConverter.ToString(md5Token).Replace("-", "").ToLower();

            byte[] md5UsernamePassword = mD5.ComputeHash(Encoding.ASCII.GetBytes(userName + password));

            string lowerCaseMD5User = BitConverter.ToString(md5UsernamePassword).Replace("-", "").ToLower();

            string codeString = $"{userName};{lowerCaseMD5User};{lowerCaseMD5Token}";

            string code = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(codeString));

            string anchor = "6%3Ab68e917f-6c64-4765-bf4f-64f3317aa443%23400580001/17";

            Uri licenseUri = new Uri($"https://l.ucs.ru/ls5api/api/License/GetLicenseIdByAnchor?anchor={anchor}");

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromSeconds(30);
                httpClient.DefaultRequestHeaders.Add("usr", code);

                HttpResponseMessage response = httpClient.GetAsync(licenseUri).Result;

                HttpStatusCode httpStatusCode = response.StatusCode;

                if (httpStatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                }
                else if (httpStatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new Exception();
                }
            }
        }

        static void Main(string[] args)
        {
            //_waiter = new Waiter();

            //string userCode = "15";
            //string stationId = "";

            //_waiter.NetworkService.SetParameters("", "", "IT", "10");

            //_waiter.UserAuthorization(userCode);
            //_waiter.SetStationId(stationId);
            //_waiter.DownloadReferences();

            //Console.WriteLine($"{_waiter.CurrentUser.Id}:{_waiter.CurrentUser.Name}");

            //License license = new License("400580001");
            //Console.WriteLine(license.Token);

            //List<Order> orders = _waiter.GetOrderList();
            //DisplayOrders(orders);

            //Menu();

            int code = 3241412;
            int delete = 10;

            int codeLength = code.ToString().Length;

            for (int i = 0; i < codeLength; i++)
            {
                code = code / delete;
                Console.WriteLine($"Code: {code}");
            }

            Console.ReadLine();
        }
    }
}
