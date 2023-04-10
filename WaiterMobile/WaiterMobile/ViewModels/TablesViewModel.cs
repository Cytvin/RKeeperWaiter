using RKeeperWaiter.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace WaiterMobile.ViewModel
{
    public class TablesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand GoToBack { get; private set; }
        public ICommand SelectHall { get; private set; }
        public Hall SelectedHall { get; set; }
        public IEnumerable<Hall> Halls { get { return App.Waiter.Halls; } }


        public TablesViewModel()
        {

        }


    }
}
