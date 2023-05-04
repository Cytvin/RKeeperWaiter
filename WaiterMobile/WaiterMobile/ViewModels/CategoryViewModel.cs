using RKeeperWaiter.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace WaiterMobile.ViewModels
{
    public class CategoryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Stack<MenuCategory> _previousCategories;
        private Action<Dish> _addDishToList;
        private Grid _categoryItems;
        private int _gridRow = 0;
        private int _gridColumn = 0;

        public ICommand GoToBack { get; private set; }
        public ICommand DisplayCategory { get; private set; }
        public string CategoryName { get; private set; }

        public CategoryViewModel(Grid categoryItems, Action<Dish> addDish)
        {
            _addDishToList = addDish;
            _categoryItems = categoryItems;

            _previousCategories = new Stack<MenuCategory>();
            GoToBack = new Command(OnGoToBack);
            DisplayCategory = new Command<MenuCategory>(DisplayMenuCategory);

            SetCategory(0);
        }

        private void SetCategory(int categoryId)
        {
            MenuCategory menuCategory = App.Waiter.GetMenuCategory(categoryId);

            DisplayMenuCategory(menuCategory);
        }

        private void DisplayMenuCategory(MenuCategory category)
        {
            CategoryName = category.Name;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryName)));

            _categoryItems.Children.Clear();
            _categoryItems.RowDefinitions.Clear();

            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = 100;
            _categoryItems.RowDefinitions.Add(rowDefinition);

            _gridRow = 0;
            _gridColumn = 0;

            if (_previousCategories.Count > 0)
            {
                Button backButton = AddButtonToGrid("<-");
                backButton.Command = DisplayCategory;
                backButton.CommandParameter = _previousCategories.Pop();
            }

            foreach (Category internalCategory in category.Categories)
            {
                Button button = AddButtonToGrid(internalCategory.Name);
                button.Command = new Command(() =>
                {
                    _previousCategories.Push(category);
                    SetCategory(internalCategory.Id);
                });
            }

            foreach (Dish dish in category.Dishes)
            {
                string price = String.Format("{0:0.00}", dish.Price);
                string namePrice = $"{dish.Name}  {price}";

                Button button = AddButtonToGrid(namePrice);

                button.Command = new Command(() =>
                {
                    Dish currentDish = dish.Clone() as Dish;
                    _addDishToList(currentDish);
                });
            }
        }

        private Button AddButtonToGrid(string buttonText)
        {
            Button button = new Button();
            button.Text = buttonText;

            Grid.SetColumn(button, _gridColumn++);
            Grid.SetRow(button, _gridRow);

            _categoryItems.Children.Add(button);

            if (_gridColumn == 2)
            {
                _gridColumn = 0;
                _gridRow++;

                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = 100;
                _categoryItems.RowDefinitions.Add(rowDefinition);
            }

            return button;
        }

        private void OnGoToBack()
        {
            Shell.Current.Navigation.PopAsync(true);
        }
    }
}
