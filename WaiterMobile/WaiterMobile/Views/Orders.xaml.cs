using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RKeeperWaiter.Models;
using System.Collections.Generic;
using WaiterMobile.ViewModel;

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