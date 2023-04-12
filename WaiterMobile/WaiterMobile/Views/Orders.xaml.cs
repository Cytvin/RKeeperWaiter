using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WaiterMobile.ViewModels;

namespace WaiterMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Orders : ContentPage
    {
        public Orders()
        {
            InitializeComponent();

            BindingContext = new OrdersViewModel(_ordersGrid);
        }
    }
}