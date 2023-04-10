using RKeeperWaiter.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using WaiterMobile.Views;
using Xamarin.Forms;

namespace WaiterMobile.ViewModel
{
    public class TablesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand GoToBack { get; private set; }
        public ICommand SelectHall { get; private set; }
        public ICommand TableSelected { get; private set; }
        public Hall SelectedHall { get; set; }
        public IEnumerable<Hall> Halls { get { return App.Waiter.Halls; } }
        public ObservableCollection<Table> Tables { get; set; }

        public TablesViewModel()
        {
            GoToBack = new Command(Return);
            SelectHall = new Command(OnHallSelected);
            TableSelected = new Command<Table>(OnTableSelected);
            Tables = new ObservableCollection<Table>();
        }

        private void Return()
        {
            Shell.Current.Navigation.PopAsync(true);
        }

        private void OnHallSelected()
        {
            Tables.Clear();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedHall)));
            foreach (Table table in SelectedHall.Tables) 
            {
                Tables.Add(table);
            }
        }

        private void OnTableSelected(Table table)
        {
            Order newOrder = new Order();
            newOrder.Table = table;

            string hallTable = $"{SelectedHall.Name} | {table.Name}";

            Shell.Current.Navigation.PushAsync(new GuestOrders(newOrder, hallTable));
        }
    }
}
