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

        private List<Guest> _guests;
        private List<Dish> _commonDishes;

        public int Id => _id;
        public int VisitId => _visitId;
        public Guid Guid => _guid;
        public string Name => _name;
        public string Comment { get => _comment; set => _comment = value; }
        public Table Table { get => _table; set => _table = value; }
        public OrderType Type { get => _type; set => _type = value; }
        public decimal Sum { get => _sum; set => _sum = value / 100; }

        public IEnumerable<Guest> Guests => _guests;
        public IEnumerable<Dish> CommonDishes => _commonDishes;

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
            _commonDishes.Remove(dish);
        }
    }
}
