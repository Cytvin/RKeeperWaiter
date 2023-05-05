using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaiterMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaiterMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderTotal : ContentPage
    {
        public OrderTotal(OrderViewModel order)
        {
            InitializeComponent();
            BindingContext = order;
        }
    }
}