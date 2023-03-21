using WaiterMobile.Views;
using Xamarin.Forms;

namespace WaiterMobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Settings), typeof(Settings));
            Routing.RegisterRoute(nameof(Orders), typeof(Orders));
            Routing.RegisterRoute(nameof(Tables), typeof(Tables));
        }
    }
}