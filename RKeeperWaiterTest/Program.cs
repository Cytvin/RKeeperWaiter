using System;
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
using RKeeperWaiter.XmlRequests;

namespace RKeeperWaiterTest
{
    internal class Program
    {
        private static IWaiter _waiter;
        public static void Menu()
        {
            while (true)
            {
                Console.WriteLine("Введите код категории: ");
                int categoryId = int.Parse(Console.ReadLine());

                Console.WriteLine($"***** Объекты в категории {categoryId} *****");

                MenuCategory menuCategory = _waiter.GetMenuCategory(categoryId);

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

        public static void SaveTestOrder(int stationId)
        {
            Table table = _waiter.Halls.First().Tables.Last();
            OrderType orderType = _waiter.OrderTypes.First();

            Order newOrder = new Order();
            newOrder.Table = table;
            newOrder.Type = orderType;

            _waiter.CreateNewOrder(newOrder, 3);

            newOrder = _waiter.GetOrderList().Last();

            Console.WriteLine($"Order: {newOrder.Guid} | Guest Count: {newOrder.Guests.Count()}");

            Dish dish1 = _waiter.Dishes.Single(d => d.Id == 1000030);
            dish1.Course = Course.Empty;

            Dish dish2 = _waiter.Dishes.Single(d => d.Id == 1000029);
            dish2.Course = Course.Empty;

            Dish dish3 = _waiter.Dishes.Single(d => d.Id == 1000035);
            dish3.Course = _waiter.Courses.First();

            Guest guest = newOrder.Guests.First();

            newOrder.InsertCommonDish(dish1);
            newOrder.InsertCommonDish(dish3);
            guest.InsertDish(dish2);

            _waiter.SaveOrder(newOrder);
        }

        public static void DisplayModifiersShemes(IEnumerable<ModifiersSheme> modifiersShemes)
        {
            foreach (ModifiersSheme modifiersSheme in modifiersShemes)
            {
                Console.WriteLine($"Sheme - {modifiersSheme.Id}: {modifiersSheme.Name}:");
                
                foreach (ModifiersGroup modifiersGroup in modifiersSheme.ModifiersGroups)
                {
                    Console.WriteLine($"Group - {modifiersGroup.Id}: {modifiersGroup.Name}");

                    foreach (ModifiersGroup internalGroup in modifiersGroup.ModifiersGroups)
                    {
                        Console.WriteLine($"InternalGroup - {internalGroup.Id}: {internalGroup.Name}");
                    }

                    foreach (Modifier modifier in modifiersGroup.Modifiers)
                    {
                        Console.WriteLine($"Modifier - {modifier.Id}: {modifier.Name} Price: {modifier.Price}");
                    }
                }

                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            //_waiter = new Waiter();
            //_waiter = new WaiterTest();
            Waiter waiter = new Waiter();


            string userCode = "15";
            string stationId = "";
            string authString = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(""));

            waiter.NetworkService.SetParameters("", "", authString);

            waiter.UserAuthorization(userCode);
            waiter.SetStationId(stationId);
            waiter.GetModifiers();

            //_waiter.DownloadReferences();
            //_waiter.CreateLicense(Guid.NewGuid());

            Console.ReadLine();
        }
    }
}
