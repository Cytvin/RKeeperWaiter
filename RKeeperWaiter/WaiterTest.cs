using RKeeperWaiter.Models;
using RKeeperWaiter.XmlRequests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RKeeperWaiter
{
    public class WaiterTest : IWaiter
    {
        private int _stationId;
        private int _restaurantCode;
        private User _user;

        private List<Category> _categories;
        private List<Dish> _dishes;
        private List<GuestType> _guestTypes;
        private List<Hall> _halls;
        private List<OrderType> _orderTypes;
        private List<Course> _courses;
        private List<ModifiersSheme> _modifiersShemes;

        public int StationId => _stationId;
        public User CurrentUser => _user;
        public IEnumerable<Hall> Halls => _halls;
        public IEnumerable<OrderType> OrderTypes => _orderTypes;
        public IEnumerable<Dish> Dishes => _dishes.Where(d => d.InMenu == true);
        public IEnumerable<Course> Courses => _courses;
        public IEnumerable<ModifiersSheme> Modifiers => _modifiersShemes;
        public NetworkService NetworkService { get; private set; }

        public WaiterTest()
        {
            _categories = new List<Category>();
            _dishes = new List<Dish>();
            _guestTypes = new List<GuestType>();
            _halls = new List<Hall>();
            _orderTypes = new List<OrderType>();
            _modifiersShemes = new List<ModifiersSheme>();
            NetworkService = new NetworkService();
        }

        public void DownloadReferences()
        {
            _categories = GetCategories();
            _guestTypes = GetGuestTypes();
            _halls = GetHalls();
            _orderTypes = GetOrderTypes();
            _courses = GetCourses();
            _modifiersShemes = GetModifiers();
            _dishes = GetDishes();
            GetRestCode();
            SetPrice();
        }

        public void CreateLicense(Guid applicationGuid){ }

        public void CreateNewOrder(Order newOrder, int guestCount)
        {
            throw new NotImplementedException();
        }

        public void DeleteDish()
        {
            throw new NotImplementedException();
        }

        public MenuCategory GetMenuCategory(int id)
        {
            string categoryName = _categories.Single(c => c.Id == id).Name;

            MenuCategory menuCategory = new MenuCategory(categoryName);

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

        public List<Order> GetOrderList()
        {
            return new List<Order>();
        }

        public void SaveOrder(Order order)
        {
            
        }

        public void SetStationId(string stationId)
        {
            if (Int32.TryParse(stationId, out _stationId) == false)
            {
                throw new Exception();
            }
        }

        public void TransferDish(Order source, Order destionation, Dish dish)
        {
            throw new NotImplementedException();
        }

        public void UserAuthorization(string userCode)
        {
            XDocument usersList = XDocument.Load(TestData.GetUserAuthorizationTR());

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

        private List<Category> GetCategories()
        {
            List<Category> categories = new List<Category>()
            {
                new Category(0, "Все", 0),
                new Category(1000007, "Нераспределяемые Наценки", 0),
                new Category(1000015, "Первые блюда", 0),
                new Category(1000024, "Горячие блюда", 0),
                new Category(1000025, "Котлеты", 1000024),
                new Category(1000028, "Блюда из курицы", 1000024),
                new Category(1000033, "Блюда из свинины", 1000024),
                new Category(1000037, "Мангал/гриль", 0),
                new Category(1000038, "Шашлык", 1000037),
                new Category(1000041, "Люля-кебаб", 1000037),
                new Category(1000044, "Гарниры", 0),
                new Category(1000056, "Салаты", 0),
                new Category(1000080, "Стейки", 1000037),
                new Category(1000094, "Комбо-блюда", 0),
                new Category(1000095, "Бургеры", 0),
                new Category(1000102, "Картошка", 0),
                new Category(1000108, "Напиток", 0),
                new Category(1000128, "Комбо", 0),
                new Category(1000130, "Комплексные обеды", 0),
                new Category(1000131, "Супы", 1000130),
                new Category(1000132, "Вторые блюда", 1000130),
                new Category(1000133, "Напитки", 1000130),
                new Category(1000161, "Комплексный обед", 0),
                new Category(1000233, "Чек-листы", 0),
                new Category(1000234, "Элементы", 1000233),
                new Category(1000410, "Десерты", 0),
                new Category(1000947, "Бизнес ланч", 0),
                new Category(1000980, "Первые блюда", 0),
                new Category(1000981, "Вторые блюда", 0),
                new Category(1001002, "Тест", 0),
                new Category(1001004, "орпопроро", 1000024),
                new Category(1001051, "Блюда", 0),
                new Category(1001072, "Тест!", 0),
                new Category(1001261, "Услуги", 0),
                new Category(1001298, "Сыры", 0),
                new Category(1001360, "Тест", 0),
                new Category(1001369, "шашлык", 0),
                new Category(1001374, "люля", 1001369),
                new Category(1001376, "прокат велосип", 0)
            };

            return categories;
        }

        private List<GuestType> GetGuestTypes()
        {
            List<GuestType> guestTypes = new List<GuestType>()
            {
                new GuestType(1, 1, "Гость"),
                new GuestType(1000893, 2, "ПН"),
                new GuestType(1000946, 3, "Семья")
            };

            return guestTypes;
        }

        private List<Hall> GetHalls()
        {
            List<Hall> halls = new List<Hall>()
            {
                new Hall(1000071, 1, "Основной зал"),
                new Hall(1000072, 2, "VIP зал"),
                new Hall(1000415, 3, "Быстрый"),
                new Hall(1001287, 4, "Достака")
            };

            List<Table> tables = new List<Table>()
            {
                new Table(1000075, 1, "1", 1000071),
                new Table(1000076, 2, "2", 1000071),
                new Table(1000077, 3, "VIP 3", 1000072),
                new Table(1000078, 4, "VIP 4", 1000072),
                new Table(1000079, 5, "VIP 5", 1000072),
                new Table(1000398, 6, "Ч/Л", 1000071),
                new Table(1000416, 11, "11", 1000415),
                new Table(1000502, 8, "8", 1000415),
                new Table(1001288, 7, "Дост", 1001287),
                new Table(1001350, 9, "3", 1000071),
                new Table(1001783, 10, "4", 1000071)
            };

            foreach (Table table in tables)
            {
                Hall hall = halls.Where(x => x.Id == table.HallId).Single();
                hall.InsertTable(table);
            }

            return halls;
        }

        private List<OrderType> GetOrderTypes()
        {
            List<OrderType> orderTypes = new List<OrderType>()
            {
                new OrderType(1, 1, "Общие"),
                new OrderType(10069, 9001, "не выбрано"),
                new OrderType(1000989, 2, "С собой"),
                new OrderType(1000990, 3, "С доставкой")
            };

            return orderTypes;
        }

        private List<Course> GetCourses()
        {
            List<Course> courses = new List<Course>()
            {
                new Course(1, "Готовить позже"),
                new Course(2, "Впервую очередь"),
            };

            return courses;
        }

        private List<ModifiersSheme> GetModifiers()
        {
            List<ModifiersGroup> modifiersGroups = new List<ModifiersGroup>();

            XDocument xModifiersGroupsDocument = XDocument.Load(TestData.GetModifiersGroupsTR());

            IEnumerable<XElement> xModifiersGroups = xModifiersGroupsDocument.Root.Element("CommandResult").Element("RK7Reference").Element("Items").Elements("Item");

            foreach (XElement xModifiersGroup in xModifiersGroups)
            {
                int id = Convert.ToInt32(xModifiersGroup.Attribute("Ident").Value);
                int code = Convert.ToInt32(xModifiersGroup.Attribute("Code").Value);
                string name = xModifiersGroup.Attribute("Name").Value;
                int parentGroupId = Convert.ToInt32(xModifiersGroup.Attribute("MainParentIdent").Value);

                ModifiersGroup modifiersGroup = new ModifiersGroup(id, code, name);
                modifiersGroups.Add(modifiersGroup);

                if (parentGroupId != 0)
                {
                    ModifiersGroup parrent = modifiersGroups.Single(g => g.Id == parentGroupId);
                    parrent?.InsertGroup(modifiersGroup);
                }
            }

            XDocument xModifiersDocument = XDocument.Load(TestData.GetModifiersTR());

            IEnumerable<XElement> xModifiers = xModifiersDocument.Root.Element("CommandResult").Element("RK7Reference").Element("Items").Elements("Item");

            foreach (XElement xModifier in xModifiers)
            {
                int id = Convert.ToInt32(xModifier.Attribute("Ident").Value);
                int code = Convert.ToInt32(xModifier.Attribute("Code").Value);
                string name = xModifier.Attribute("Name").Value;
                int groupId = Convert.ToInt32(xModifier.Attribute("MainParentIdent").Value);

                Modifier modifier = new Modifier(id, code, name);

                ModifiersGroup modifiersGroup = modifiersGroups.Single(g => g.Id == groupId);
                modifiersGroup.InsertModifier(modifier);
            }

            XDocument xModifiersShemesDocument = XDocument.Load(TestData.GetModifiersShemesTR());

            IEnumerable<XElement> xModifiersShemes = xModifiersShemesDocument.Root.Element("CommandResult").Element("RK7Reference").Element("Items").Elements("Item");

            List<ModifiersSheme> modifiersShemes = new List<ModifiersSheme>();

            foreach (XElement xModifiersSheme in xModifiersShemes)
            {
                int id = Convert.ToInt32(xModifiersSheme.Attribute("Ident").Value);
                int code = Convert.ToInt32(xModifiersSheme.Attribute("Code").Value);
                string name = xModifiersSheme.Attribute("Name").Value;

                ModifiersSheme modifiersSheme = new ModifiersSheme(id, code, name);

                XElement RIChildItems = xModifiersSheme.Element("RIChildItems");

                IEnumerable<XElement> xChilds = null;

                if (RIChildItems != null)
                {
                    xChilds = RIChildItems.Elements("TModiSchemeDetail");

                    foreach (XElement xChild in xChilds)
                    {
                        int modifiersGroupId = Convert.ToInt32(xChild.Attribute("ModiGroup").Value);
                        int upLimit = Convert.ToInt32(xChild.Attribute("UpLimit").Value);
                        int downLimit = Convert.ToInt32(xChild.Attribute("DownLimit").Value);

                        ModifiersGroup modifiersGroup = modifiersGroups.SingleOrDefault(g => g.Id == modifiersGroupId);
                        modifiersGroup.UpLimit = upLimit;
                        modifiersGroup.DownLimit = downLimit;

                        if (modifiersGroup != null)
                        {
                            modifiersSheme.InsertModifiersGroup(modifiersGroup);
                        }
                    }
                }

                modifiersShemes.Add(modifiersSheme);
            }

            return modifiersShemes;
        }

        private List<Dish> GetDishes()
        {
            List<Dish> dishes = new List<Dish>();

            XDocument xDishesDocument = XDocument.Load(TestData.GetDishesTR());

            IEnumerable<XElement> xDishes = xDishesDocument.Root.Element("CommandResult").Element("RK7Reference").Element("Items").Elements("Item");

            foreach (XElement xDish in xDishes)
            {
                int dishId = Convert.ToInt32(xDish.Attribute("ItemIdent").Value);
                string dishName = xDish.Attribute("Name").Value;
                int parentId = Convert.ToInt32(xDish.Attribute("MainParentIdent").Value);
                string guidString = xDish.Attribute("GUIDString").Value;
                int modifiersShemeId = Convert.ToInt32(xDish.Attribute("ModiScheme").Value);

                Guid dishGuid = guidString == "" ? Guid.NewGuid() : Guid.Parse(guidString);

                Dish dish = new Dish(dishId, dishGuid, dishName, parentId);

                if (modifiersShemeId != 0)
                {
                    dish.ModifiersSheme = (ModifiersSheme)_modifiersShemes.Single(m => m.Id == modifiersShemeId).Clone();
                }

                dishes.Add(dish);
            }

            return dishes;
        }

        private void GetRestCode()
        {
            _restaurantCode = 400580001;
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

            foreach (ModifiersSheme modifiersSheme in _modifiersShemes)
            {
                foreach (ModifiersGroup modifiersGroup in modifiersSheme.ModifiersGroups)
                {
                    foreach (Modifier modifier in modifiersGroup.Modifiers)
                    {
                        int modifierId = modifier.Id;

                        if (prices.ContainsKey(modifierId))
                        {
                            modifier.SetPrice(prices[modifierId]);
                        }
                    }
                }
            }
        }

        private Dictionary<int, decimal> GetOrderMenu()
        {
            XDocument xOrderMenu = XDocument.Load(TestData.GetMenuItemTR());

            Dictionary<int, decimal> prices = new Dictionary<int, decimal>();

            IEnumerable<XElement> xDishesPrices = xOrderMenu.Root.Element("Dishes").Elements("Item");

            foreach (XElement xDishPrice in xDishesPrices)
            {
                int dishId = Convert.ToInt32(xDishPrice.Attribute("Ident").Value);
                decimal price = Convert.ToDecimal(xDishPrice.Attribute("Price").Value);

                prices.Add(dishId, price);
            }

            IEnumerable<XElement> xModifiersPrices = xOrderMenu.Root.Element("Modifiers").Elements("Item");

            foreach (XElement xModifierPrice in xModifiersPrices)
            {
                int modifierId = Convert.ToInt32(xModifierPrice.Attribute("Ident").Value);
                decimal price = Convert.ToDecimal(xModifierPrice.Attribute("Price").Value);

                prices.Add(modifierId, price);
            }

            return prices;
        }

        private void GetUserData(int userId)
        {
            XDocument xUserDataDocument = XDocument.Load(TestData.GetUserDataTR());

            XElement xUserData = xUserDataDocument.Root.Element("CommandResult").Element("RK7Reference").Element("Items").Elements("Item").First();

            string userName = xUserData.Attribute("Name").Value;
            int userCode = Convert.ToInt32(xUserData.Attribute("Code").Value);
            Guid userGuid = Guid.Parse(xUserData.Attribute("GUIDString").Value);

            _user = new User(userId, userName, userCode, userGuid);
        }
    }
}
