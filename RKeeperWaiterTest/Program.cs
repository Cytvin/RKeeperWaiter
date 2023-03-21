using System;
using System.Text;
using System.Xml;
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

                MenuCategory menuCategory = _waiter.GetMenuCategory(categoryId);

                foreach (Category category in menuCategory.GetCategories())
                {
                    Console.WriteLine($"({category.ParentId}) {category.Id} - {category.Name}");
                }

                foreach (Dish dish in menuCategory.GetDishes())
                {
                    Console.WriteLine($"({dish.ParentId}) {dish.Id} ({dish.Guid}) - {dish.Name} : {dish.Price}");
                }
            }
        }

        static void Main(string[] args)
        {
            _waiter = new Waiter();

            string userCode = "15";
            string stationId = "";

            //_waiter.NetworkService.SetParameters("", "", "IT", "10");

            //_waiter.UserAuthorization(userCode);
            //_waiter.GetOrderList();
            //_waiter.SetStationId(stationId);
            //_waiter.CreateReferences();

            //_waiter.MakeTestOrder();
            //_waiter.MakeTestOrder();

            //Menu();

            //XmlRequestBuilder builder = new XmlRequestBuilder();

            //GetWaiterList getWaiterList = new GetWaiterList();

            //getWaiterList.CreateRequest(builder);

            //XmlDocument xmlDocument = new XmlDocument();

            //xmlDocument.LoadXml(builder.GetRequestText());

            //xmlDocument.Save("D:\\sqllog\\test.xml");

            RequestBuilder refRequestBuilder = new RequestBuilder();
            GetRefData gwl = new GetRefData("EMPLOYEES", "1033231", null, null);
            gwl.CreateRequest(refRequestBuilder);

            XmlDocument doc = refRequestBuilder.GetXml();

            doc.Save("D:\\sqllog\\test.xml");

            Console.ReadLine();
        }
    }
}
