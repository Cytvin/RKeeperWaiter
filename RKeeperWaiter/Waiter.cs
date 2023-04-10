using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private User _user;

        public int StationId => _stationId;
        public NetworkService NetworkService { get; private set; }
        public User CurrentUser => _user;
        public IEnumerable<Hall> Halls => _halls;
        public IEnumerable<OrderType> OrderTypes => _orderTypes;

        public Waiter()
        {
            _categories = new List<Category>();
            _dishes = new List<Dish>();
            _guestTypes = new List<GuestType>();
            _halls = new List<Hall>();
            _orderTypes = new List<OrderType>();
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
            _categories = GetCategories();
            _dishes = GetDishes();
            _guestTypes = GetGuestTypes();
            _halls = GetHalls();
            _orderTypes = GetOrderTypes();
            SetPrice();
            GetSystemInfo();
        }

        public Menu GetMenuCategory(int id)
        {
            string categoryName = _categories.First(c => c.Id == id).Name;

            Menu menuCategory = new Menu(categoryName);

            IEnumerable<Category> categories = _categories.Where(category => category.ParentId == id && category.Id != 0);

            foreach (Category category in categories)
            {
                menuCategory.InsertCategory(category);
            }

            IEnumerable<Dish> dishes = _dishes.Where(dish => dish.ParentId == id && dish.InMenu);

            foreach (Dish dish in dishes)
            {
                menuCategory.InsertDish(dish);
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

            XElement xWaiters = usersList.Root.Element("CommandResult").Element("Waiters");

            if (xWaiters == null)
            {
                throw new ArgumentNullException(nameof(xWaiters), "В базе нет активных официантов");
            }
            
            IEnumerable<XElement> xWaitersList = xWaiters.Elements("waiter").Where(x => x.Attribute("Code").Value == userCode);

            if (xWaitersList.Count() == 0)
            {
                throw new ArgumentNullException(null, $"Пользовтель с кодом \"{userCode}\" не найден");
            }

            userIdentificator = xWaitersList.Single().Attribute("ID").Value;

            int userId = Convert.ToInt32(userIdentificator);

            GetUserData(userId);
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

                GetOrderInfo(newOrder);

                orders.Add(newOrder);
            }

            return orders;
        }

        public void CreateNewOrder(Order newOrder, int guestCount)
        {
            CreateOrder createOrder = new CreateOrder(newOrder.Table.Id, _user.Id, _stationId, newOrder.Type.Id, guestCount);
            RequestBuilder requestBuilder = new RequestBuilder();
            createOrder.CreateRequest(requestBuilder);

            XDocument createOrderResult = NetworkService.SendRequest(requestBuilder.GetXml());
        }

        private void GetSystemInfo()
        {
            GetSystemInfo getSystemInfo = new GetSystemInfo();
            RequestBuilder requestBuilder = new RequestBuilder();
            getSystemInfo.CreateRequest(requestBuilder);

            NetworkService.SendRequest(requestBuilder.GetXml());
        }

        private void GetUserData(int userId)
        {
            XElement userDataXml = RequestReference("EMPLOYEES", userId.ToString(), null, null).First();

            string userName = userDataXml.Attribute("Name").Value;
            int userCode = Convert.ToInt32(userDataXml.Attribute("Code").Value);
            Guid userGuid = Guid.Parse(userDataXml.Attribute("GUIDString").Value);

            _user = new User(userId, userName, userCode, userGuid);
        }

        private void GetOrderInfo(Order order)
        {
            RequestBuilder requestBuilder = new RequestBuilder();
            GetOrder getOrder = new GetOrder(order.Guid);
            getOrder.CreateRequest(requestBuilder);
            
            XDocument orderInfo = NetworkService.SendRequest(requestBuilder.GetXml());

            XElement xOrder = orderInfo.Root.Element("CommandResult").Element("Order");

            XElement xGuests = xOrder.Element("Guests");

            if (xGuests != null)
            {
                foreach (XElement xGuest in xGuests.Elements("Guest"))
                {
                    Guest guest = new Guest(xGuest.Attribute("guestLabel").Value);

                    order.InsertGuest(guest);
                }
            }

            XElement xSession = xOrder.Element("Session");

            if (xSession != null)
            {
                foreach (XElement dish in xSession.Elements("Dish"))
                {
                    int dishId = Convert.ToInt32(dish.Attribute("id").Value);

                    XAttribute seat = dish.Attribute("seat");

                    if (seat == null)
                    {
                        order.InsertCommonDish(_dishes.First(d => d.Id == dishId));
                        continue;
                    }

                    Guest guest = order.Guests.First(g => g.Label == seat.Value);

                    guest.InsertDish(_dishes.First(d => d.Id == dishId));
                }
            }
        }

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

        private List<Category> GetCategories()
        {
            List<Category> categories = new List<Category>();

            IEnumerable<XElement> menuCategoriesXElement = RequestReference("CategList", null, null, null);

            foreach (XElement categoryXElement in menuCategoriesXElement)
            {
                int categoryId = Convert.ToInt32(categoryXElement.Attribute("ItemIdent").Value);
                string categoryName = categoryXElement.Attribute("Name").Value;
                int parentId = Convert.ToInt32(categoryXElement.Attribute("MainParentIdent").Value);

                Category category = new Category(categoryId, categoryName, parentId);

                categories.Add(category);
            }

            return categories;
        }

        private List<Dish> GetDishes()
        {
            List<Dish> dishes = new List<Dish>();

            IEnumerable<XElement> dishesXElements = RequestReference("MenuItems", null, null, null);

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

        private void SetPrice()
        {
            Dictionary<int, decimal> prices = GetOrderMenu();

            foreach (Dish dish in _dishes)
            {
                int dishId = dish.Id;

                if (prices.ContainsKey(dishId))
                {
                    dish.SetPrice(prices[dishId]);
                }
            }
        }

        private List<GuestType> GetGuestTypes()
        {
            List<GuestType> guestTypes = new List<GuestType>();

            IEnumerable<XElement> xGuestTypes = RequestReference("GUESTTYPES", null, "Items.(Ident, Name, Code)", null);

            foreach (XElement guestType in xGuestTypes)
            {
                int id = Convert.ToInt32(guestType.Attribute("Ident").Value);
                int code = Convert.ToInt32(guestType.Attribute("Code").Value);
                string name = guestType.Attribute("Name").Value;

                GuestType type = new GuestType(id, code, name);
                guestTypes.Add(type);
            }

            return guestTypes;
        }

        private List<OrderType> GetOrderTypes()
        {
            List<OrderType> orderTypes = new List<OrderType>();

            IEnumerable<XElement> xOrderTypes = RequestReference("CHANGEABLEORDERTYPES", null, null, "1");

            foreach (XElement orderType in xOrderTypes)
            {
                int id = Convert.ToInt32(orderType.Attribute("Ident").Value);
                int code = Convert.ToInt32(orderType.Attribute("Code").Value);
                string name = orderType.Attribute("Name").Value;

                OrderType type = new OrderType(id, code, name);
                orderTypes.Add(type);
            }

            return orderTypes;
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

        private List<Hall> GetHalls()
        {
            List<Hall> halls = new List<Hall>();

            IEnumerable<XElement> hallList = RequestReference("HallPlans", null, "Items.(Ident, Code, ActiveHierarchy,Status,Name,Items.(*))", "1");

            foreach (XElement hall in hallList)
            {
                int id = Convert.ToInt32(hall.Attribute("Ident").Value);
                int code = Convert.ToInt32(hall.Attribute("Code").Value);
                string name = hall.Attribute("Name").Value;

                Hall newHall = new Hall(id, code, name);
                halls.Add(newHall);
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

                Hall hall = halls.Where(x => x.Id == hallId).Single();
                hall.InsertTable(newTabel);
            }

            return halls;
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
