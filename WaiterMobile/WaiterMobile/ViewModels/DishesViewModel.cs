using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WaiterMobile.ViewModels
{
    public class DishesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string CategoryName { get; private set; } 
    }
}
