using RKeeperWaiter.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using WaiterMobile.Views;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace WaiterMobile.ViewModels
{
    public class OrdersViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<Order> _orders;
        private Grid _ordersGrid;

        public ICommand Create { get; private set; }
        public ICommand Select { get; private set; }
        public ICommand OpenOptions { get; private set; }
        public IEnumerable<Order> Orders { get {  return _orders; } }
        public string UserName 
        {
            get { return App.Waiter.CurrentUser.Name; }
        }

        public OrdersViewModel(Grid ordersGrid)
        {
            _orders = App.Waiter.GetOrderList();

            Create = new Command(CreateOrder);
            Select = new Command<Order>(SelectOrder);
            OpenOptions = new Command(OnOpenOptions);
            _ordersGrid = ordersGrid;

            CreateOrderButtons();
        }

        private void CreateOrder()
        {
            Shell.Current.Navigation.PushAsync(new Tables());
        }

        private void SelectOrder(Order order)
        {
            Shell.Current.Navigation.PushAsync(new OrderView(order));
        }

        private async void OnOpenOptions()
        {
            string options = await Shell.Current.DisplayActionSheet("Опции", "Отмена", null, "Закрыть общую смену");

            if (options == "Закрыть общую смену") 
            {
                bool closeOut = await Shell.Current.DisplayAlert("", "Закрыть смену?", "Да", "Нет");

                if (closeOut)
                {
                    Shell.Current.DisplayToastAsync("Смена успешно закрыта", 3000);
                    Shell.Current.Navigation.PopAsync();
                }
            }
        }

        private void CreateOrderButtons()
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
                orderButton.Command = Select;
                orderButton.CommandParameter = order;

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
    }
}
