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
        private List<Hall> _halls;
        private List<OrderType> _orderTypes;

        private NewOrder _newOrder;
        private User _user;

        public int StationId { get { return _stationId; } }
        public NetworkService NetworkService { get; private set; }
        public User CurrentUser { get { return _user; } }
        public NewOrder NewOrder { get { return _newOrder; } }
        public IEnumerable<Hall> Halls { get { return _halls; } }
        public IEnumerable<OrderType> OrderTypes { get { return _orderTypes; } }

        public Waiter()
        {
            _categories = new List<Category>();
            _dishes = new List<Dish>();
            _guestTypes = new List<GuestType>();
            _halls = new List<Hall>();
            _newOrder = new NewOrder();
            NetworkService = new NetworkService();
        }

        public void SetStationId(string stationId)
        {
            if (Int32.TryParse(stationId, out _stationId) == false)
            {
                throw new Exception();
            }
        }

        public void DownloadReferences()
        {
            GetCategories();
            GetDishes();
            GetGuestTypes();
            GetHalls();
            GetOrderTypes();
            SetPrice();
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
            XElement userDataXml = RequestReference("EMPLOYEES", userId.ToString(), null, null).First();

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

        //public void MakeTestOrder()
        //{
        //    StringBuilder stringBuilder = new StringBuilder();

        //    using (XmlWriter writer = XmlWriter.Create(stringBuilder))
        //    {
        //        writer.WriteStartDocument();
        //        writer.WriteStartElement("RK7Query");

        //        writer.WriteStartElement("RK7Command");
        //        writer.WriteAttributeString("CMD", "CreateOrder");

        //        writer.WriteStartElement("Table");
        //        writer.WriteAttributeString("code", "1");
        //        writer.WriteEndElement();

        //        writer.WriteStartElement("Waiter");
        //        writer.WriteAttributeString("id", _user.Id.ToString());
        //        writer.WriteEndElement();

        //        writer.WriteStartElement("Station");
        //        writer.WriteAttributeString("id", _stationId.ToString());
        //        writer.WriteEndElement();

        //        writer.WriteStartElement("Guests");

        //        writer.WriteStartElement("Guest");
        //        writer.WriteAttributeString("GuestLabel", "1");
        //        writer.WriteEndElement();

        //        writer.WriteEndElement();

        //        writer.WriteEndElement();

        //        writer.WriteEndElement();
        //    }

        //    XDocument newOrder = NetworkService.SendRequest(stringBuilder);
        //}

        //public void DeleteOrder(int visitId)
        //{
        //    StringBuilder stringBuilder = new StringBuilder();

        //    using (XmlWriter writer = XmlWriter.Create(stringBuilder))
        //    {
        //        writer.WriteStartDocument();
        //        writer.WriteStartElement("RK7Query");

        //        writer.WriteStartElement("RK7Command");
        //        writer.WriteAttributeString("CMD", "CloseVisit");
        //        writer.WriteAttributeString("VisitID", visitId.ToString());

        //        writer.WriteEndElement();

        //        writer.WriteEndElement();
        //    }

        //    XDocument closeOrder = NetworkService.SendRequest(stringBuilder);
        //}

        private void GetCategories()
        {
            IEnumerable<XElement> menuCategoriesXElement = RequestReference("CategList", null, null, null);

            foreach (XElement categoryXElement in menuCategoriesXElement)
            {
                int categoryId = Convert.ToInt32(categoryXElement.Attribute("ItemIdent").Value);
                string categoryName = categoryXElement.Attribute("Name").Value;
                int parentId = Convert.ToInt32(categoryXElement.Attribute("MainParentIdent").Value);

                Category category = new Category(categoryId, categoryName, parentId);

                _categories.Add(category);
            }
        }

        private void GetDishes()
        {
            IEnumerable<XElement> dishesXElements = RequestReference("MenuItems", null, null, null);

            foreach (XElement dishesXElement in dishesXElements)
            {
                int dishId = Convert.ToInt32(dishesXElement.Attribute("ItemIdent").Value);
                string dishName = dishesXElement.Attribute("Name").Value;
                int parentId = Convert.ToInt32(dishesXElement.Attribute("MainParentIdent").Value);
                string guidString = dishesXElement.Attribute("GUIDString").Value;

                Guid dishGuid = guidString == "" ? Guid.NewGuid() : Guid.Parse(guidString);

                Dish dish = new Dish(dishId, dishGuid, dishName, parentId);

                _dishes.Add(dish);
            }
        }

        private void SetPrice()
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

        private void GetGuestTypes()
        {
            IEnumerable<XElement> guestTypes = RequestReference("GUESTTYPES", null, "Items.(Ident, Name, Code)", null);

            foreach (XElement guestType in guestTypes)
            {
                int id = Convert.ToInt32(guestType.Attribute("Ident").Value);
                int code = Convert.ToInt32(guestType.Attribute("Code").Value);
                string name = guestType.Attribute("Name").Value;

                GuestType type = new GuestType(id, code, name);
                _guestTypes.Add(type);
            }
        }

        private void GetOrderTypes()
        {
            IEnumerable<XElement> orderTypes = RequestReference("CHANGEABLEORDERTYPES", null, null, "1");

            foreach (XElement orderType in orderTypes)
            {
                int id = Convert.ToInt32(orderType.Attribute("Ident").Value);
                int code = Convert.ToInt32(orderType.Attribute("Code").Value);
                string name = orderType.Attribute("Name").Value;

                OrderType type = new OrderType(id, code, name);
                _orderTypes.Add(type);
            }
        }

        private Dictionary<int, decimal> GetOrderMenu()
        {
            RequestBuilder requestBuilder = new RequestBuilder();
            GetOrderMenu getOrderMenu = new GetOrderMenu(_stationId, _user.Id);
            getOrderMenu.CreateRequest(requestBuilder);

            XDocument orderMenu = NetworkService.SendRequest(requestBuilder.GetXml());

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

        private void GetHalls()
        {
            IEnumerable<XElement> hallList = RequestReference("HallPlans", null, "Items.(Ident, Code, ActiveHierarchy,Status,Name,Items.(*))", "1");

            foreach (XElement hall in hallList)
            {
                int id = Convert.ToInt32(hall.Attribute("Ident").Value);
                int code = Convert.ToInt32(hall.Attribute("Code").Value);
                string name = hall.Attribute("Name").Value;

                Hall newHall = new Hall(id, code, name);
                _halls.Add(newHall);
            }

            IEnumerable<XElement> tables = RequestReference("Tables", null, "Items.(Ident, Code, Name, Hall, MaxGuests)", "1");

            foreach (XElement table in tables)
            {
                int id = Convert.ToInt32(table.Attribute("Ident").Value);
                int code = Convert.ToInt32(table.Attribute("Code").Value);
                string name = table.Attribute("Name").Value;
                int hallId = Convert.ToInt32(table.Attribute("Hall").Value);
                int maxGuests = Convert.ToInt32(table.Attribute("MaxGuests").Value);


                Table newTabel = new Table(id, code, name, maxGuests);

                Hall hall = _halls.Where(x => x.Id == hallId).Single();
                hall.AddTable(newTabel);
            }
        }

        private IEnumerable<XElement> RequestReference(string refName, string refItemIdent, string propMask, string onlyActive)
        {
            RequestBuilder requestBuilder = new RequestBuilder();
            GetRefData getRefData = new GetRefData(refName, refItemIdent, propMask, onlyActive);
            getRefData.CreateRequest(requestBuilder);

            XDocument reference = NetworkService.SendRequest(requestBuilder.GetXml());

            return reference.Root.Element("CommandResult").Element("RK7Reference").Element("Items").Elements("Item");
        }
    }
}
