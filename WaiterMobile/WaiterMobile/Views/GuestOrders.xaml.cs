using RKeeperWaiter.Models;
using WaiterMobile.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaiterMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GuestOrders : ContentPage
    {
        public GuestOrders(Order newOrder, string hallTable)
        {
            InitializeComponent();

            BindingContext = new GuestOrdersViewModel(newOrder)
            {
                PageLabel = hallTable
            };
        }

        private void OnGuestSwitchToggled(object sender, ToggledEventArgs e)
        {
            if (e.Value)
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
    }
}