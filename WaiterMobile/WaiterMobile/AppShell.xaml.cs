using WaiterMobile.Views;
using Xamarin.Forms;

namespace WaiterMobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SettingsView), typeof(SettingsView));
            Routing.RegisterRoute(nameof(Orders), typeof(Orders));
            Routing.RegisterRoute(nameof(Tables), typeof(Tables));
            Routing.RegisterRoute(nameof(GuestOrders), typeof(GuestOrders));
        }
    }
}