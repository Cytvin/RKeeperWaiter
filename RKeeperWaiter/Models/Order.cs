using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private List<Guest> _guests;
        private List<Dish> _commonDishes;

        public int Id { get { return _id; } }
        public int VisitId { get { return _visitId; } }
        public Guid Guid { get { return _guid; } }
        public string Name { get { return _name; } }
        public Table Table { get { return _table; } }

        public IEnumerable<Guest> Guests { get { return _guests; } }
        public IEnumerable<Dish> CommonDishes { get { return _commonDishes; } }

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

        public void SetSum(int sum)
        {
            _sum = sum / 100;
        }

        public void SetType (OrderType type) 
        {
            _type = type;
        }

        public void SetTable (Table table)
        {
            _table = table;
        }

        public void InsertGuest(Guest guest)
        {
            _guests.Add(guest);
        }

        public void InsertCommonDish(Dish dish)
        {
            _commonDishes.Add(dish);
        }
    }
}
