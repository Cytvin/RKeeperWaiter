using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using RKeeperWaiter.Models;
using WaiterMobile.Views;
using Xamarin.Forms;

namespace WaiterMobile.ViewModels
{
    public class OrderViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Order _order;

        public ICommand GoToBack { get; private set; }
        public ICommand AddDish { get; private set; }
        public ICommand EditDish { get; private set; }
        public ICommand SaveOrder { get; private set; }
        public ObservableCollection<Dish> CommonDishes { get; set; }
        public ObservableCollection<Guest> Guests { get; set; }
        public Action<Dish> AddDishToCommonDishes => CommonDishes.Add;
        public string OrderName => _order.Name;

        public OrderViewModel(Order order)
        {
            _order = order;
            GoToBack = new Command(OnGoToBack);
            AddDish = new Command<Action<Dish>>(OnAddDish);
            EditDish = new Command<Dish>(OnEditDish);
            SaveOrder = new Command(OnSaveOrder);
            CommonDishes = new ObservableCollection<Dish>(order.CommonDishes);
            Guests = new ObservableCollection<Guest>(order.Guests);
        }

        private void OnGoToBack()
        {
            Shell.Current.Navigation.PopAsync(true);
        }

        private void OnAddDish(Action<Dish> action)
        {
            Shell.Current.Navigation.PushAsync(new Dishes(action), true);
        }

        private void OnEditDish(Dish dish)
        {
            Shell.Current.Navigation.PushAsync(new DishView(dish), true);
        }

        private void OnSaveOrder()
        {
            try
            {
                App.Waiter.SaveOrder(_order);
            }
            catch (Exception ex) 
            {
                Shell.Current.DisplayAlert("Ошибка", ex.Message, "ОК");
                return;
            }

            Shell.Current.DisplayAlert("Готово", "Заказ успешно отправлен", "ОК");
        }
    }
}
