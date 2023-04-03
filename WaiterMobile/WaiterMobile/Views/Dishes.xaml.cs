using RKeeperWaiter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaiterMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dishes : ContentPage
    {
        private Stack<MenuCategory> _previousCategories;
        private Action<Dish> _addDishToList;

        private int _gridRow = 0;
        private int _gridColumn = 0;

        public Dishes(Action<Dish> _addDish)
        {
            InitializeComponent();

            _previousCategories = new Stack<MenuCategory>();
            _addDishToList = _addDish;

            SetCategory(0);
        }

        private void SetCategory(int categoryId)
        {
            MenuCategory menuCategory = App.Waiter.GetMenuCategory(categoryId);

            DisplayMenuCategory(menuCategory);
        }

        private void DisplayMenuCategory(MenuCategory category)
        {
            _categoryName.Text = category.Name;

            _dishesGrid.Children.Clear();
            _dishesGrid.RowDefinitions.Clear();

            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = 100;
            _dishesGrid.RowDefinitions.Add(rowDefinition);

            _gridRow = 0;
            _gridColumn = 0;

            if (_previousCategories.Count > 0)
            {
                Button backButton = AddButtonToGrid("<-");
                backButton.Clicked += (s, e) =>
                {
                    DisplayMenuCategory(_previousCategories.Pop());
                };
            }

            foreach (Category internalCategory in category.GetCategories())
            {
                Button button = AddButtonToGrid(internalCategory.Name);
                button.Clicked += (s, e) =>
                {
                    _previousCategories.Push(category);
                    SetCategory(internalCategory.Id);
                };
            }

            foreach (Dish dish in category.GetDishes())
            {
                string price = String.Format("{0:0.00}", dish.Price);
                string namePrice = $"{dish.Name}  {price}";

                Button button = AddButtonToGrid(namePrice);
                button.Clicked += (s, e) =>
                {
                    _addDishToList(dish);
                };
            }
        }

        private void OnBackButtonCLicked(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PopAsync(true);
        }

        private Button AddButtonToGrid(string buttonText)
        {
            Button button = new Button();
            button.Text = buttonText;

            Grid.SetColumn(button, _gridColumn++);
            Grid.SetRow(button, _gridRow);

            _dishesGrid.Children.Add(button);

            if (_gridColumn == 2)
            {
                _gridColumn = 0;
                _gridRow++;


                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = 100;
                _dishesGrid.RowDefinitions.Add(rowDefinition);
            }

            return button;
        }
    }
}