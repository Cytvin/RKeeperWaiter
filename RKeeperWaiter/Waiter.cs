using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace RKeeperWaiter
{
    public class Waiter
    {
        private Uri _uri;
        private string _authorizationString;
        private int _stationId;
        private int _userId;

        private List<Category> _categories;
        private List<Dish> _dishes;
        private List<GuestType> _guestTypes;

        public void SetUrl(string ip, string port)
        {
            _uri = new Uri($"https://{ip}:{port}/rk7api/v0/xmlinterface.xml");
        }

        public void SetAuthorization(string username, string password)
        {
            _authorizationString = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{username}:{password}"));
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
            StringBuilder stringBuilder = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(stringBuilder))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("RK7Query");

                writer.WriteStartElement("RK7Command");
                writer.WriteAttributeString("CMD", "GetWaiterList");
                writer.WriteAttributeString("checkrests", "false");
                writer.WriteEndElement();

                writer.WriteEndElement();
            }

            XDocument usersList = SendRequest(stringBuilder);

            string userIdentificator = usersList.Root.Element("CommandResult").Element("Waiters").Elements("waiter").Where(x => x.Attribute("Code").Value == userCode).Single().Attribute("ID").Value;

            _userId = Convert.ToInt32(userIdentificator);
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
                writer.WriteAttributeString("id", _userId.ToString());
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

            XDocument newOrder = SendRequest(stringBuilder);
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

            XDocument closeOrder = SendRequest(stringBuilder);
        }

        private List<Category> GetCategories()
        {
            StringBuilder stringBuilder = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(stringBuilder))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("RK7Query");

                writer.WriteStartElement("RK7Command");
                writer.WriteAttributeString("CMD", "GetRefData");
                writer.WriteAttributeString("RefName", "CategList");

                writer.WriteEndElement();

                writer.WriteEndElement();
            }

            List<Category> menuCategories = new List<Category>();

            XDocument menuCategoriesXml = SendRequest(stringBuilder);

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
            StringBuilder stringBuilder = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(stringBuilder))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("RK7Query");

                writer.WriteStartElement("RK7Command");
                writer.WriteAttributeString("CMD", "GetRefData");
                writer.WriteAttributeString("RefName", "MenuItems");

                writer.WriteEndElement();

                writer.WriteEndElement();
            }

            List<Dish> dishes = new List<Dish>();

            XDocument dishesXml = SendRequest(stringBuilder);

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
            List<GuestType> guestTypesList = new List<GuestType>();

            StringBuilder stringBuilder = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(stringBuilder))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("RK7Query");

                writer.WriteStartElement("RK7Command");
                writer.WriteAttributeString("CMD", "GetRefData");
                writer.WriteAttributeString("RefName", "GUESTTYPES");
                writer.WriteAttributeString("PropMask", "Items.(Ident,Name,Code)");

                writer.WriteEndElement();

                writer.WriteEndElement();
            }

            XDocument guestTypesXml = SendRequest(stringBuilder);

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
                writer.WriteAttributeString("id", _userId.ToString());
                writer.WriteEndElement();

                writer.WriteEndElement();
                
                writer.WriteEndElement();
            }

            XDocument orderMenu = SendRequest(stringBuilder);
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

        private XDocument SendRequest(StringBuilder xmlContent)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                return true;
            };

            XmlDocument requestContent = new XmlDocument();
            requestContent.LoadXml(xmlContent.ToString());

            DateTime requestSaveTime = DateTime.Now;
            requestContent.Save($"D:\\sqllog\\Request_{requestSaveTime.Year}{requestSaveTime.Month}{requestSaveTime.Day}_" +
                $"{requestSaveTime.Hour}{requestSaveTime.Minute}{requestSaveTime.Second}{requestSaveTime.Millisecond}.xml");

            using (HttpClient httpClient = new HttpClient(httpClientHandler))
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, _uri);

                httpRequestMessage.Headers.Add("Authorization", "Basic " + _authorizationString);

                httpRequestMessage.Content = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(xmlContent.ToString())));

                HttpResponseMessage response = httpClient.SendAsync(httpRequestMessage).Result;

                HttpStatusCode httpStatusCode = response.StatusCode;

                if (httpStatusCode == HttpStatusCode.OK)
                {
                    XDocument responseContent = XDocument.Parse(response.Content.ReadAsStringAsync().Result);

                    DateTime responseSaveTime = DateTime.Now;

                    responseContent.Save($"D:\\sqllog\\Response_{responseSaveTime.Year}{responseSaveTime.Month}{responseSaveTime.Day}_" +
                        $"{responseSaveTime.Hour}{responseSaveTime.Minute}{responseSaveTime.Second}{responseSaveTime.Millisecond}.xml");

                    return responseContent;
                }
                else if (httpStatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new Exception();
                }

                throw new Exception();
            }
        }
    }
}
