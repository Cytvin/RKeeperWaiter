using RKeeperWaiter.Models;
using RKeeperWaiter.XmlRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace RKeeperWaiter
{
    public interface IWaiter
    {
        IEnumerable<Course> Courses { get; }
        User CurrentUser { get; }
        IEnumerable<Dish> Dishes { get; }
        IEnumerable<Hall> Halls { get; }
        IEnumerable<ModifiersSheme> Modifiers { get; }
        NetworkService NetworkService { get; }
        IEnumerable<OrderType> OrderTypes { get; }
        int StationId { get; }

        void CreateLicense(Guid applicationGuid);
        void CreateNewOrder(Order newOrder, int guestCount);
        void DeleteDish();
        void DownloadReferences();
        MenuCategory GetMenuCategory(int id);
        List<Order> GetOrderList();
        void SaveOrder(Order order);
        void SetStationId(string stationId);
        void TransferDish(Order source, Order destionation, Dish dish);
        void UserAuthorization(string userCode);
    }
}