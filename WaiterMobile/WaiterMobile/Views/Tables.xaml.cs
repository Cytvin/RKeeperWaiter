using RKeeperWaiter.Models;
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
    public partial class Tables : ContentPage
    {
        public Tables()
        {
            InitializeComponent();

            HallPickerInitialize();
        }

        private void HallPickerInitialize()
        {
            foreach(Hall hall in App.Waiter.Halls)
            {
                _hallPicker.Items.Add(hall.Name);
            }
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            CreateTables();
        }

        private void OnTableSelected(Table table, Hall hall) 
        {
            Order newOrder = new Order();
            newOrder.Table = table;

            string hallTable = $"{hall.Name} | {table.Name}";

            Shell.Current.Navigation.PushAsync(new GuestOrders(newOrder, hallTable));
        }

        private void CreateTables()
        {
            _tables.Children.Clear();

            int selectedHall = _hallPicker.SelectedIndex;
            Hall hall = App.Waiter.Halls.ToArray()[selectedHall];
            IEnumerable<Table> tables = hall.Tables;

            foreach (Table table in tables)
            {
                Button tableButton = new Button();

                tableButton.Clicked += (s, e) => 
                { 
                    OnTableSelected(table, hall); 
                };

                tableButton.Text = table.Name;

                _tables.Children.Add(tableButton);
            }
        }

        private void OnBackButtonCLicked(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PopAsync(true);
        }
    }
}