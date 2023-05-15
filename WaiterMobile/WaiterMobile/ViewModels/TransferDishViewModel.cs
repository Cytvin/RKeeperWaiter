using RKeeperWaiter.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace WaiterMobile.ViewModels
{
    public class TransferDishViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Order _sourceOrder;
        private DishViewModel _transferDish;

        public ICommand GoToBack { get; private set; }
        public IEnumerable<Hall> Halls => App.Waiter.Halls;
        public ObservableCollection<Table> Tables { get; private set; }
        public ObservableCollection<Order> Orders { get; private set; }
        public Hall SelectedHall { get; set; }
        public Table SelectedTable { get; set; }
        public ICommand SelectHall { get; private set; }
        public ICommand SelectTable { get; private set; }
        public ICommand SelectOrder { get; private set; } 

        public TransferDishViewModel(Order sourceOrder, DishViewModel transferDish)
        {
            _sourceOrder = sourceOrder;
            _transferDish = transferDish;

            SelectHall = new Command(OnSelectHall);
            SelectTable = new Command(OnSelectTable);
            SelectOrder = new Command<Order>(OnSelectOrder);
            GoToBack = new Command(OnGoToBack);
            Tables = new ObservableCollection<Table>();
            Orders = new ObservableCollection<Order>();
        }

        private void OnSelectHall()
        {
            Tables.Clear();
            Tables = new ObservableCollection<Table>(SelectedHall.Tables);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tables)));
        }

        private void OnSelectTable()
        {
            Orders.Clear();

            if (SelectedTable == null)
            {
                return;
            }

            List<Order> orders = App.Waiter.GetOrderList();
            Orders = new ObservableCollection<Order>(orders.Where(o => o.Table.Id == SelectedTable.Id));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Orders)));
        }

        private void OnSelectOrder(Order destination)
        {
            App.Waiter.TransferDish(_sourceOrder, destination, _transferDish.InternalDish);

            PageRemover.Remove(1);

            _transferDish.Remove.Execute(_transferDish);

            Shell.Current.DisplayToastAsync($"Блюдо перенесено в заказ {destination.Name}", 3000);
        }

        private void OnGoToBack()
        {
            Shell.Current.Navigation.PopAsync(true);
        }
    }
}
