using System;
using System.Collections.Generic;
using System.Linq;
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

                Menu menuCategory = _waiter.GetMenuCategory(categoryId);

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

        static void Main(string[] args)
        {
            _waiter = new Waiter();

            string userCode = "15";
            string stationId = "";

            _waiter.NetworkService.SetParameters("", "", "IT", "10");

            _waiter.UserAuthorization(userCode);
            _waiter.SetStationId(stationId);
            _waiter.DownloadReferences();

            Console.WriteLine($"{_waiter.CurrentUser.Id}:{_waiter.CurrentUser.Name}");

            Order order = new Order();
            Table table = _waiter.Halls.First().Tables.First();
            OrderType orderType = _waiter.OrderTypes.First();

            order.SetTable(table);
            order.SetType(orderType);

            _waiter.CreateNewOrder(order, 1);

            List<Order> orders = _waiter.GetOrderList();
            DisplayOrders(orders);

            Menu();
            Console.ReadLine();
        }
    }
}
