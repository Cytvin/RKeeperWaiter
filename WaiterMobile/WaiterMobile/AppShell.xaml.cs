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
        }
    }
}