using RKeeperWaiter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using WaiterMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaiterMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderView : ContentPage
    {
        private Order _order;

        public OrderView(Order order)
        {
            InitializeComponent();

            BindingContext = new OrderViewModel(order);

            _order = order;

            //_order.OrderChanged += DisplayOrder;

            //DisplayOrder();
        }

        //private void DisplayOrder()
        //{
        //    _orderDishes.Children.Clear();

        //    _orderDishes.Children.Add(AddGridToStack("", _order.InsertCommonDish));

        //    foreach (Dish dish in _order.CommonDishes)
        //    {
        //        _orderDishes.Children.Add(CreateDishLabel(dish));
        //    }

        //    foreach (Guest guest in _order.Guests)
        //    {
        //        guest.DishInserted += DisplayOrder;
        //        _orderDishes.Children.Add(AddGridToStack(guest.Label, guest.InsertDish));

        //        foreach (Dish dish in guest.Dishes)
        //        {
        //            _orderDishes.Children.Add(CreateDishLabel(dish));
        //        }
        //    }
        //}

        private void OnAddDishButtonClicked(Action<Dish> action)
        {
            Shell.Current.Navigation.PushAsync(new Dishes(action));
        }

        private Grid AddGridToStack(string label, Action<Dish> action)
        {
            Grid guestGrid = new Grid();

            guestGrid.Margin = new Thickness(30, 10, 30, 10);

            ColumnDefinition labelColumn = new ColumnDefinition();

            ColumnDefinition buttonColumn = new ColumnDefinition();
            buttonColumn.Width = 50;

            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = 50;

            guestGrid.ColumnDefinitions.Add(labelColumn);
            guestGrid.ColumnDefinitions.Add(buttonColumn);

            guestGrid.RowDefinitions.Add(rowDefinition);

            Label guestLabel = new Label();
            guestLabel.Text = label == "" ? "Общие" : "Гость " + label;
            guestLabel.HorizontalOptions = LayoutOptions.Start;
            guestLabel.VerticalOptions = LayoutOptions.Center;

            Button addDishButton = new Button();
            addDishButton.Text = "+";
            addDishButton.HorizontalOptions = LayoutOptions.End;
            addDishButton.VerticalOptions = LayoutOptions.Center;
            addDishButton.Clicked += (s, e) =>
            {
                OnAddDishButtonClicked(action);
            };

            Grid.SetColumn(guestLabel, 0);
            Grid.SetColumn(addDishButton, 1);

            guestGrid.Children.Add(guestLabel);
            guestGrid.Children.Add(addDishButton);

            return guestGrid;
        }

        private Label CreateDishLabel(Dish dish)
        {
            Label dishLabel = new Label();
            dishLabel.Text = dish.Name + " " + dish.Price;
            dishLabel.Margin = new Thickness(30, 5, 30, 5);

            return dishLabel;
        }
    }
}