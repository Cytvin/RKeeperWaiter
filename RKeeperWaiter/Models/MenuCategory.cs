using System.Collections.Generic;

namespace RKeeperWaiter.Models
{
    public class MenuCategory
    {
        private List<Category> _internalCategory;
        private List<Dish> _dishes;
        private string _name;

        public string Name => _name;
        public IEnumerable<Category> Categories => _internalCategory;
        public IEnumerable<Dish> Dishes => _dishes;

        public MenuCategory(string name)
        {
            _internalCategory = new List<Category>();
            _dishes = new List<Dish>();
            _name = name;
        }

        public void InsertCategory(Category category) 
        {
            _internalCategory.Add(category);
        }

        public void InsertDish(Dish dish)
        {
            _dishes.Add(dish);
        }
    }
}
