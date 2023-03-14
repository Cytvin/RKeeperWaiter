using System;

namespace RKeeperWaiter.Models
{
    public class Dish
    {
        private int _id;
        private Guid _guid;
        private string _name;
        private bool _inMenu;
        private decimal _price;
        private int _parentId;
        
        public bool InMenu { get { return _inMenu; } set { _inMenu = value; } }
        public int Id { get { return _id; } }
        public int ParentId { get { return _parentId; } }
        public Guid Guid { get { return _guid; } }
        public string Name { get { return _name; } }
        public decimal Price { get { return _price; } }

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
            if (price < 0)
            {
                _inMenu = false;
                return;
            }

            _price = Math.Round(price / 100, 2);
        }
    }
}
