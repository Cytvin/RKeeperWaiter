using RKeeperWaiter.Models;
using WaiterMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaiterMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderView : ContentPage
    {
        public OrderView(Order order)
        {
            InitializeComponent();

            BindingContext = new OrderViewModel(order);
        }
    }
}