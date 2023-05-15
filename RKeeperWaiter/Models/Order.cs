using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace RKeeperWaiter.Models
{
    public class Order
    {
        private int _id;
        private int _visitId;
        private Guid _guid;
        private string _name;
        private decimal _sum;
        private OrderType _type;
        private Table _table;
        private string _comment;
        private bool _isSend;
        private bool _isClosed = false;

        private List<Guest> _guests;
        private List<Dish> _commonDishes;

        public int Id => _id;
        public int VisitId => _visitId;
        public Guid Guid => _guid;
        public string Name => _name;
        public string Comment { get => _comment; set => _comment = value; }
        public Table Table { get => _table; set => _table = value; }
        public OrderType Type { get => _type; set => _type = value; }
        public bool IsSend { get => _isSend; set => _isSend = value; }
        public bool IsClosed { get => _isClosed; set => _isClosed = value; }
        public IEnumerable<Guest> Guests => _guests;
        public IEnumerable<Dish> CommonDishes => _commonDishes;
        public int GuestCount 
        {
            get 
            {
                if (_guests.Count == 0)
                {
                    return 1;
                }

                return _guests.Count;
            }
        }
        public decimal Sum
        {
            get
            {
                if (_sum == 0)
                {
                    return CountSum();
                }

                return _sum;
            }
            set => _sum = value / 100;
        }

        public Order() { }

        public Order (int id, int visitId, Guid guid, string name)
        {
            _id = id;
            _visitId = visitId;
            _guid = guid;
            _name = name;

            _guests = new List<Guest>();
            _commonDishes = new List<Dish>();
        }

        public void InsertGuest(Guest guest)
        {
            _guests.Add(guest);
        }

        public void InsertCommonDish(Dish dish)
        {
            _commonDishes.Add(dish);
        }

        public void RemoveCommonDish(Dish dish)
        {
            if (_commonDishes.Contains(dish) == false)
            {
                return;
            }

            _commonDishes.Remove(dish);
        }

        private decimal CountSum()
        {
            decimal sum = 0;

            sum += CountDishesSum(_commonDishes);

            foreach (Guest guest in _guests)
            {
                sum += CountDishesSum(guest.Dishes);
            }

            return sum;
        }

        private decimal CountDishesSum(IEnumerable<Dish> dishes)
        {
            decimal sum = 0;

            foreach (Dish dish in dishes)
            {
                sum += dish.Price;
            }

            return sum;
        }
    }
}
