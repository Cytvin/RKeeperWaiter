using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RKeeperWaiter.Models
{
    public class Guest
    {
        private string _label;
        private List<Dish> _dishes;
        public event Action DishInserted;

        public string Label => _label;
        public IEnumerable<Dish> Dishes => _dishes;

        public Guest(string name) 
        {
            _label = name;
            _dishes = new List<Dish>();
        }

        public void InsertDish(Dish dish)
        {
            _dishes.Add(dish);
            DishInserted?.Invoke();
        }
    }
}
