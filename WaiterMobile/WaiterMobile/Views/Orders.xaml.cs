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
            App.Waiter.CreateReferences();
            LoadOrders();
            _userName.Text = App.Waiter.CurrentUser.Name;
        }

        private void LoadOrders()
        {
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

                Button button = new Button();
                button.Text = order.Name;
                button.WidthRequest = 150;
                button.HorizontalOptions = LayoutOptions.Center;

                Grid.SetColumn(button, columnNumber++);
                Grid.SetRow(button, rowNumber);

                _ordersGrid.Children.Add(button);

                ordersCount++;

                if (columnNumber == 2)
                {
                    columnNumber = 0;
                    rowNumber++;
                }
            }
        }
    }
}