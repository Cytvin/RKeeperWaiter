using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using RKeeperWaiter.Models;
using RKeeperWaiter.XmlRequests;

namespace RKeeperWaiter
{
    public class Waiter
    {
        private int _stationId;

        private List<Category> _categories;
        private List<Dish> _dishes;
        private List<GuestType> _guestTypes;

        private User _user;

        public int StationId { get { return _stationId; } }
        public NetworkService NetworkService { get; private set; }
        public User CurrentUser { get { return _user; } }

        public Waiter()
        {
            NetworkService = new NetworkService();
        }

        public void SetStationId(string stationId)
        {
            if (Int32.TryParse(stationId, out _stationId) == false)
            {
                throw new Exception();
            }
        }

        public void CreateReferences()
        {
            _categories = GetCategories();
            _dishes = GetDishes();
            _guestTypes = GetGuestTypes();
            SetMenuPrice();
        }

        public MenuCategory GetMenuCategory(int id)
        {
            MenuCategory menuCategory = new MenuCategory();

            IEnumerable<Category> categories = _categories.Where(category => category.ParentId == id);

            foreach (Category category in categories)
            {
                menuCategory.AddCategory(category);
            }

            IEnumerable<Dish> dishes = _dishes.Where(dish => dish.ParentId == id && dish.InMenu);

            foreach (Dish dish in dishes)
            {
                menuCategory.AddDish(dish);
            }

            return menuCategory;
        }

        public void UserAuthorization(string userCode)
        {
            RequestBuilder requestBuilder = new RequestBuilder();

            GetWaiterList getWaiterList = new GetWaiterList();
            getWaiterList.CreateRequest(requestBuilder);

            XDocument usersList = NetworkService.SendRequest(requestBuilder.GetXml());

            string userIdentificator;

            try
            {
                userIdentificator = usersList.Root.Element("CommandResult").Element("Waiters").Elements("waiter").Where(x => x.Attribute("Code").Value == userCode).Single().Attribute("ID").Value;
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }

            int userId = Convert.ToInt32(userIdentificator);

            GetUserData(userId);
        }

        private void GetUserData(int userId)
        {
            RequestBuilder requestBuilder = new RequestBuilder();
            
            GetRefData getRefData = new GetRefData("EMPLOYEES", userId.ToString(), null, null);
            getRefData.CreateRequest(requestBuilder);

            XDocument xDocument = NetworkService.SendRequest(requestBuilder.GetXml());

            XElement userDataXml = xDocument.Root.Element("CommandResult").Element("RK7Reference").Element("Items").Element("Item");

            string userName = userDataXml.Attribute("Name").Value;
            int userCode = Convert.ToInt32(userDataXml.Attribute("Code").Value);
            Guid userGuid = Guid.Parse(userDataXml.Attribute("GUIDString").Value);

            _user = new User(userId, userName, userCode, userGuid);
        }

        public List<Order> GetOrderList()
        {
            RequestBuilder requestBuilder = new RequestBuilder();
            GetOrderList getOrderList = new GetOrderList(_user.Id);
            getOrderList.CreateRequest(requestBuilder);

            List<Order> orders = new List<Order>();

            XDocument ordersXml = NetworkService.SendRequest(requestBuilder.GetXml());

            IEnumerable<XElement> visitList = ordersXml.Root.Element("CommandResult").Elements("Visit");

            foreach (XElement visit in visitList)
            {
                int visitId = Convert.ToInt32(visit.Attribute("VisitID").Value);

                XElement order = visit.Element("Orders").Element("Order");

                int orderId = Convert.ToInt32(order.Attribute("OrderID").Value);
                Guid orderGuid = Guid.Parse(order.Attribute("guid").Value);
                string orderName = order.Attribute("OrderName").Value;

                Order newOrder = new Order(orderId, visitId, orderGuid, orderName);

                orders.Add(newOrder);
            }

            return orders;
        }

        public void MakeTestOrder()
        {
            StringBuilder stringBuilder = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(stringBuilder))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("RK7Query");

                writer.WriteStartElement("RK7Command");
                writer.WriteAttributeString("CMD", "CreateOrder");

                writer.WriteStartElement("Table");
                writer.WriteAttributeString("code", "1");
                writer.WriteEndElement();

                writer.WriteStartElement("Waiter");
                writer.WriteAttributeString("id", _user.Id.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("Station");
                writer.WriteAttributeString("id", _stationId.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("GuestType");
                writer.WriteAttributeString("id", "1");
                writer.WriteEndElement();

                writer.WriteStartElement("Guests");

                writer.WriteStartElement("Guest");
                writer.WriteAttributeString("GuestLabel", "1");
                writer.WriteEndElement();

                writer.WriteEndElement();

                writer.WriteEndElement();

                writer.WriteEndElement();
            }

            XDocument newOrder = NetworkService.SendRequest(stringBuilder);
        }

        public void DeleteOrder(int visitId)
        {
            StringBuilder stringBuilder = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(stringBuilder))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("RK7Query");

                writer.WriteStartElement("RK7Command");
                writer.WriteAttributeString("CMD", "CloseVisit");
                writer.WriteAttributeString("VisitID", visitId.ToString());

                writer.WriteEndElement();

                writer.WriteEndElement();
            }

            XDocument closeOrder = NetworkService.SendRequest(stringBuilder);
        }

        private List<Category> GetCategories()
        {
            RequestBuilder requestBuilder = new RequestBuilder();

            GetRefData getRefData = new GetRefData("CategList", null, null, null);
            getRefData.CreateRequest(requestBuilder);

            List<Category> menuCategories = new List<Category>();

            XDocument menuCategoriesXml = NetworkService.SendRequest(requestBuilder.GetXml());

            IEnumerable<XElement> menuCategoriesXElement = menuCategoriesXml.Root.Element("CommandResult").Element("RK7Reference").Element("Items").Elements("Item");

            foreach (XElement categoryXElement in menuCategoriesXElement)
            {
                int categoryId = Convert.ToInt32(categoryXElement.Attribute("ItemIdent").Value);
                string categoryName = categoryXElement.Attribute("Name").Value;
                int parentId = Convert.ToInt32(categoryXElement.Attribute("MainParentIdent").Value);

                Category category = new Category(categoryId, categoryName, parentId);

                menuCategories.Add(category);
            }

            return menuCategories;
        }

        private List<Dish> GetDishes()
        {
            RequestBuilder requestBuilder = new RequestBuilder();

            GetRefData getRefData = new GetRefData("MenuItems", null, null, null);
            getRefData.CreateRequest(requestBuilder);

            List<Dish> dishes = new List<Dish>();

            XDocument dishesXml = NetworkService.SendRequest(requestBuilder.GetXml());

            IEnumerable<XElement> dishesXElements = dishesXml.Root.Element("CommandResult").Element("RK7Reference").Element("Items").Elements("Item");

            foreach (XElement dishesXElement in dishesXElements)
            {
                int dishId = Convert.ToInt32(dishesXElement.Attribute("ItemIdent").Value);
                string dishName = dishesXElement.Attribute("Name").Value;
                int parentId = Convert.ToInt32(dishesXElement.Attribute("MainParentIdent").Value);
                string guidString = dishesXElement.Attribute("GUIDString").Value;

                Guid dishGuid = guidString == "" ? Guid.NewGuid() : Guid.Parse(guidString);

                Dish dish = new Dish(dishId, dishGuid, dishName, parentId);

                dishes.Add(dish);
            }

            return dishes;
        }

        private void SetMenuPrice()
        {
            Dictionary<int, decimal> prices = GetOrderMenu();

            foreach (Dish dish in _dishes)
            {
                int dishId = dish.Id;

                if (prices.ContainsKey(dishId))
                {
                    dish.InMenu = true;
                    dish.SetPrice(prices[dishId]);
                }
            }
        }

        private List<GuestType> GetGuestTypes()
        {
            RequestBuilder requestBuilder = new RequestBuilder();

            GetRefData getRefData = new GetRefData("GUESTTYPES", null, "Items.(Ident, Name, Code)", null);
            getRefData.CreateRequest(requestBuilder);

            List<GuestType> guestTypesList = new List<GuestType>();

            XDocument guestTypesXml = NetworkService.SendRequest(requestBuilder.GetXml());

            IEnumerable<XElement> guestTypes = guestTypesXml.Root.Element("CommandResult").Element("RK7Reference").Element("Items").Elements("Item");

            foreach (XElement guestType in guestTypes)
            {
                int id = Convert.ToInt32(guestType.Attribute("Ident").Value);
                int code = Convert.ToInt32(guestType.Attribute("Code").Value);
                string name = guestType.Attribute("Name").Value;

                GuestType type = new GuestType(id, code, name);
                guestTypesList.Add(type);
            }

            return guestTypesList;
        }

        private Dictionary<int, decimal> GetOrderMenu()
        {
            StringBuilder stringBuilder = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(stringBuilder))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("RK7Query");

                writer.WriteStartElement("RK7Command");
                writer.WriteAttributeString("CMD", "GetOrderMenu");
                writer.WriteAttributeString("PropMask", "Dishes.(Ident,Price),Modifiers.(Ident,Price),OrderTypes.(Ident)");
                writer.WriteAttributeString("checkrests", "false");

                writer.WriteStartElement("Station");
                writer.WriteAttributeString("id", _stationId.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("Waiter");
                writer.WriteAttributeString("id", _user.Id.ToString());
                writer.WriteEndElement();

                writer.WriteEndElement();

                writer.WriteEndElement();
            }

            XDocument orderMenu = NetworkService.SendRequest(stringBuilder);
            Dictionary<int, decimal> prices = new Dictionary<int, decimal>();

            IEnumerable<XElement> pricesXml = orderMenu.Root.Element("CommandResult").Element("Dishes").Elements("Item");

            foreach (XElement priceXelement in pricesXml)
            {
                int dishId = Convert.ToInt32(priceXelement.Attribute("Ident").Value);
                decimal price = Convert.ToDecimal(priceXelement.Attribute("Price").Value);

                prices.Add(dishId, price);
            }

            return prices;
        }
    }
}
