using System.Collections.Generic;

namespace RKeeperWaiter.Models
{
    public class MenuCategory
    {
        private List<Category> _internalCategory;
        private List<Dish> _dishes;
        private string _name;

        public string Name { get { return _name; } }

        public MenuCategory(string name)
        {
            _internalCategory = new List<Category>();
            _dishes = new List<Dish>();
            _name = name;
        }

        public void AddCategory(Category category) 
        {
            _internalCategory.Add(category);
        }

        public void AddDish(Dish dish)
        {
            _dishes.Add(dish);
        }

        public IEnumerable<Category> GetCategories()
        {
            return _internalCategory;
        }

        public IEnumerable<Dish> GetDishes() 
        {
            return _dishes;
        }
    }
}
