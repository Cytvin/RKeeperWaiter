using RKeeperWaiter.Models;
using WaiterMobile.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaiterMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DishView : ContentPage
    {
        public DishView(DishViewModel dish)
        {
            InitializeComponent();

            BindingContext = dish;
        }
    }
}