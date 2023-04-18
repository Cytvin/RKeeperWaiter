using System;

namespace RKeeperWaiter.Models
{
    public class Dish : ICloneable
    {
        private int _id;
        private Guid _guid;
        private string _name;
        private bool _inMenu;
        private decimal _price;
        private int _parentId;
        private Course _course;

        public bool InMenu => _inMenu;
        public int Id => _id;
        public int ParentId => _parentId;
        public Guid Guid => _guid;
        public string Name => _name;
        public decimal Price => _price;
        public string NamePrice => $"{_name} {_price}";
        public Course Course { get => _course; set => _course = value; }
        public string Seat { get; set; }


        public Dish(int id, Guid guid, string name, int parent) 
        {
            _id = id;
            _guid = guid;
            _name = name;
            _inMenu = false;
            _parentId = parent;
        }

        public void SetPrice(decimal price)
        {
            if (price <= 0)
            {
                return;
            }

            _inMenu = true;
            _price = Math.Round(price / 100, 2);
        }

        public object Clone() => MemberwiseClone();
    }
}
