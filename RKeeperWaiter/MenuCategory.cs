using System.Collections.Generic;

namespace RKeeperWaiter
{
    public class MenuCategory
    {
        private List<Category> _internalCategory;
        private List<Dish> _dishes;

        public MenuCategory()
        {
            _internalCategory = new List<Category>();
            _dishes = new List<Dish>();
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
