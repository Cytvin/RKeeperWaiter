using RKeeperWaiter.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace WaiterMobile.ViewModels
{
    public class GuestViewModel
    {
        private Guest _guest;

        public string Name => _guest.Name;
        public ObservableCollection<DishViewModel> Dishes { get; private set; }
        public Action<DishViewModel> InsertDish => Dishes.Add;
        public Guest InternalGuest => _guest;
        public GuestViewModel(Guest guest)
        {
            _guest = guest;
            Dishes = MakeDishViewModels();
        }

        private ObservableCollection<DishViewModel> MakeDishViewModels()
        {
            ObservableCollection<DishViewModel> dishes = new ObservableCollection<DishViewModel>();

            foreach(Dish dish in _guest.Dishes)
            {
                dishes.Add(new DishViewModel(dish));
            }

            return dishes;
        }
    }
}
