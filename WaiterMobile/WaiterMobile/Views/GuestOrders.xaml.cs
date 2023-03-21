using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaiterMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GuestOrders : ContentPage
    {
        public GuestOrders()
        {
            InitializeComponent();

            _orderLabel.Text = App.Waiter.NewOrder.Hall.Name + "|";
            _orderLabel.Text += App.Waiter.NewOrder.Table.Name;
        }
    }
}