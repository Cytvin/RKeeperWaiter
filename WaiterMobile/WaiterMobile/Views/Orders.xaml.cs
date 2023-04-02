using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RKeeperWaiter.Models;
using System.Collections.Generic;

namespace WaiterMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Orders : ContentPage
    {
        public Orders()
        {
            InitializeComponent();
            LoadOrders();
            _userName.Text = App.Waiter.CurrentUser.Name;
        }

        private void LoadOrders()
        {
            _ordersGrid.Children.Clear();
            _ordersGrid.RowDefinitions.Clear();

            List<Order> orders = App.Waiter.GetOrderList();

            int ordersCount = 0, columnNumber = 0, rowNumber = 0;

            foreach (Order order in orders)
            {
                if (ordersCount % 2 == 0)
                {
                    RowDefinition rowDefinition = new RowDefinition();

                    rowDefinition.Height = 150;
                    _ordersGrid.RowDefinitions.Add(rowDefinition);
                }

                Button orderButton = new Button();
                orderButton.Text = order.Name;
                orderButton.WidthRequest = 150;
                orderButton.HorizontalOptions = LayoutOptions.Center;
                orderButton.Clicked += (s, e) =>
                {
                    OnOrderButtonClick(order);
                };

                Grid.SetColumn(orderButton, columnNumber++);
                Grid.SetRow(orderButton, rowNumber);

                _ordersGrid.Children.Add(orderButton);

                ordersCount++;

                if (columnNumber == 2)
                {
                    columnNumber = 0;
                    rowNumber++;
                }
            }
        }

        private void OnOrderButtonClick(Order order)
        {
            Shell.Current.Navigation.PushAsync(new OrderView(order));
        }

        private void OnCreateOrderClick(object sender, System.EventArgs e)
        {
            Shell.Current.Navigation.PushAsync(new Tables());
        }
    }
}