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

        private void CreateTables()
        {
            _tables.Children.Clear();

            int selectedHall = _hallPicker.SelectedIndex;

            IEnumerable<Table> tables = App.Waiter.Halls.ToArray()[selectedHall].Tables;

            foreach (Table table in tables)
            {
                Button tableButton = new Button();

                tableButton.Text = table.Name;

                _tables.Children.Add(tableButton);
            }
        }
    }
}