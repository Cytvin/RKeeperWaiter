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

            _orderLabel.Text = App.Waiter.NewOrder.Hall.Name + " | ";
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

        private void OnCreateOrderClick(object sender, EventArgs e) 
        {
            App.Waiter.SendNewOrder();
            Shell.Current.Navigation.PushAsync(new Orders());
        }

        private void OnOrderTypeChanged(object sender, EventArgs e)
        {
            int orderType = App.Waiter.OrderTypes.ToArray()[_orderTypePicker.SelectedIndex].Id;
            App.Waiter.NewOrder.SetOrderType(orderType);
        }

        private void OnBackButtonClicked(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PopAsync(true);
        }

        private void OnGuestSwitchToggled(object sender, ToggledEventArgs e)
        {
            if (_guestSwitch.IsToggled)
            {
                App.Waiter.NewOrder.SetGuestCount(0);

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

            int maxGuests = App.Waiter.NewOrder.Table.MaxGuests;

            if (guestCount > maxGuests)
            {
                _guestCountEntry.Text = maxGuests.ToString();
                guestCount = maxGuests;
            }

            App.Waiter.NewOrder.SetGuestCount(guestCount);
        }
    }
}