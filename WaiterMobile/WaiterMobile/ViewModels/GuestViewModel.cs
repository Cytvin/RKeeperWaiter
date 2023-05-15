using RKeeperWaiter.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Windows.Input;
using WaiterMobile.Views;
using Xamarin.Forms;

namespace WaiterMobile.ViewModels
{
    public class GuestViewModel
    {
        private Guest _guest;

        public string Name => _guest.Name;
        public ObservableCollection<DishViewModel> Dishes { get; private set; }
        public Action<Dish> InsertDish => OnInsertDish;
        public Func<DishViewModel, bool> RemoveDish => OnRemoveDish;
        public Guest InternalGuest => _guest;
        public string Label => _guest.Label;
        public GuestViewModel(Guest guest)
        {
            _guest = guest;
            Dishes = MakeDishViewModels();

            Dishes.CollectionChanged += OnDishesAdded;
        }

        private void OnDishesAdded(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (DishViewModel dish in e.NewItems)
                {
                    dish.InternalDish.Seat = _guest.Label;
                    _guest.InsertDish(dish.InternalDish);
                }
                return;
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (DishViewModel dish in e.OldItems)
                {
                    _guest.RemoveDish(dish.InternalDish);
                }
            }
        }

        private ObservableCollection<DishViewModel> MakeDishViewModels()
        {
            ObservableCollection<DishViewModel> dishes = new ObservableCollection<DishViewModel>();

            foreach(Dish dish in _guest.Dishes)
            {
                dishes.Add(new DishViewModel(dish, RemoveDish));
            }

            return dishes;
        }

        private void OnInsertDish(Dish dish)
        {
            DishViewModel dishViewModel = new DishViewModel(dish, RemoveDish);
            Dishes.Add(dishViewModel);
        }

        private bool OnRemoveDish(DishViewModel dish)
        {
            return Dishes.Remove(dish);
        }
    }
}
