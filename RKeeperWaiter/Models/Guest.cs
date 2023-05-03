using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RKeeperWaiter.Models
{
    public class Guest
    {
        private string _label;
        private List<Dish> _dishes;

        public string Label => _label;
        public IEnumerable<Dish> Dishes => _dishes;
        public string Name => $"Гость {_label}";

        public Guest(string name) 
        {
            _label = name;
            _dishes = new List<Dish>();
        }

        public void InsertDish(Dish dish)
        {
            _dishes.Add(dish);
        }
    }
}
