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
    public partial class GuestOrders : ContentPage
    {
        public GuestOrders()
        {
            InitializeComponent();

            _orderLabel.Text = App.Waiter.NewOrder.Hall.Name + "|";
            _orderLabel.Text += App.Waiter.NewOrder.Table.Name;

            OrderTypesPickerInitialize();
        }

        private void OrderTypesPickerInitialize()
        {
            foreach (OrderType orderType in App.Waiter.OrderTypes)
            {
                _orderTypePicker.Items.Add(orderType.Name);
            }

            _orderTypePicker.SelectedIndex = 1;
        }
    }
}