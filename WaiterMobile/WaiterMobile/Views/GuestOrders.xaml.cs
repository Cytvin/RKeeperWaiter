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
        private Order _newOrder;

        public GuestOrders(Order newOrder, string hallTable)
        {
            InitializeComponent();

            _orderLabel.Text = hallTable;
            _newOrder = newOrder;

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

        private void OnCreateOrderClick(object sender, EventArgs e) 
        {
            int guestCount = Convert.ToInt32(_guestCountEntry.Text);

            App.Waiter.CreateNewOrder(_newOrder, guestCount);

            Shell.Current.Navigation.PushAsync(new Orders());
        }

        private void OnOrderTypeChanged(object sender, EventArgs e)
        {
            OrderType orderType = App.Waiter.OrderTypes.ToArray()[_orderTypePicker.SelectedIndex];
            _newOrder.SetType(orderType);
        }

        private void OnBackButtonClicked(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PopAsync(true);
        }

        private void OnGuestSwitchToggled(object sender, ToggledEventArgs e)
        {
            if (_guestSwitch.IsToggled)
            {
                _guestCountEntry.IsEnabled = false;
                _guestCountLable.IsEnabled = false;

                _guestCountEntry.IsVisible = false;
                _guestCountLable.IsVisible = false;

                return;
            }

            _guestCountEntry.IsEnabled = true;
            _guestCountLable.IsEnabled = true;

            _guestCountEntry.IsVisible = true;
            _guestCountLable.IsVisible = true;
        }

        private void OnGuestEntryTextChanged(object sender, EventArgs e)
        {
            int guestCount;

            if (int.TryParse(_guestCountEntry.Text, out guestCount) == false)
            {
                _guestCountEntry.Text = "";
            }

            if (guestCount >= 100)
            {
                _guestCountEntry.Text = "99";
                guestCount = 99;
            }
        }
    }
}