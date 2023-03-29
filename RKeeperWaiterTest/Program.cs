using System;
using System.Collections.Generic;
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

            _waiter.NetworkService.SetParameters("", "", "IT", "10");

            _waiter.UserAuthorization(userCode);
            _waiter.SetStationId(stationId);
            _waiter.DownloadReferences();

            Console.WriteLine($"{_waiter.CurrentUser.Id}:{_waiter.CurrentUser.Name}");

            List<Order> orders = _waiter.GetOrderList();

            foreach (Order order in orders) 
            {
                Console.WriteLine();
                Console.WriteLine($"{order.Id} | {order.Name}: {order.Guid}");
                
                Console.WriteLine("Гости: ");
                foreach (Guest guest in order.Guests)
                {
                    Console.WriteLine($"Guest: {guest.Label}");
                }

                Console.WriteLine("Блюда: ");
                foreach (Dish dish in order.Dishes)
                {
                    Console.WriteLine($"{dish.Id} | {dish.Name} : {dish.Price}");
                }
            }

            //Menu();
            Console.ReadLine();
        }
    }
}
