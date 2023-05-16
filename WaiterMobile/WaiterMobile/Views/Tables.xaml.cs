using RKeeperWaiter.Models;
using WaiterMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaiterMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Tables : ContentPage
    {
        public Tables()
        {
            InitializeComponent();
            BindingContext = new TablesViewModel();
        }

        public Tables(Order transferOrder)
        {
            InitializeComponent();

            BindingContext = new TablesViewModel(transferOrder);
        }
    }
}