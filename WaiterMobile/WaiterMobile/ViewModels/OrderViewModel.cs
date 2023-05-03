using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Collections.Specialized;
using RKeeperWaiter.Models;
using WaiterMobile.Views;
using Xamarin.Forms;
using System.Threading.Tasks;

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
        public ICommand AddCommentary { get; private set; }
        public ICommand AddGuest { get; private set; }
        public Action<DishViewModel> AddDishToCommonDishes => CommonDishes.Add;
        public ObservableCollection<DishViewModel> CommonDishes { get; set; }
        public ObservableCollection<GuestViewModel> Guests { get; set; }
        public string OrderName => _order.Name;

        public OrderViewModel(Order order)
        {
            _order = order;

            MakeDishViewModels();
            MakeGuestViewOrder();

            GoToBack = new Command(OnGoToBack);
            AddDish = new Command<Action<DishViewModel>>(OnAddDish);
            EditDish = new Command<DishViewModel>(OnEditDish);
            SaveOrder = new Command(OnSaveOrder);
            AddCommentary = new Command(OnAddCommentary);
            AddGuest = new Command(OnGuestAdd);

            CommonDishes = MakeDishViewModels();
            Guests = MakeGuestViewOrder();

            CommonDishes.CollectionChanged += OnCommonDishesAdded;
            Guests.CollectionChanged += OnGuestAdded;
        }

        private void OnGoToBack()
        {
            Shell.Current.Navigation.PopAsync(true);
        }

        private void OnAddDish(Action<DishViewModel> action)
        {
            Shell.Current.Navigation.PushAsync(new Dishes(action), true);
        }

        private void OnEditDish(DishViewModel dish)
        {
            Shell.Current.Navigation.PushAsync(new DishView(dish), true);
        }

        private void OnCommonDishesAdded(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach(DishViewModel dish in e.NewItems)
            {
                _order.InsertCommonDish(dish.InternalDish);
            }
        }

        private void OnGuestAdded(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (GuestViewModel guest in e.NewItems)
            {
                _order.InsertGuest(guest.InternalGuest);
            }
        }

        private void OnGuestAdd()
        {
            string guestName = (Guests.Count + 1).ToString();
            Guest newGuest = new Guest(guestName);
            Guests.Add(new GuestViewModel(newGuest));
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

        private async void OnAddCommentary()
        {
            string comment = await Shell.Current.DisplayPromptAsync("Введите комментарий к заказу", message: "", initialValue: _order.Comment);
            
            if (comment == null)
            {
                return;
            }

            _order.Comment = comment;
        }

        private ObservableCollection<DishViewModel> MakeDishViewModels()
        {
            ObservableCollection<DishViewModel> dishes = new ObservableCollection<DishViewModel>();

            foreach (Dish dish in _order.CommonDishes)
            {
                dishes.Add(new DishViewModel(dish));
            }

            return dishes;
        }

        private ObservableCollection<GuestViewModel> MakeGuestViewOrder()
        {
            ObservableCollection<GuestViewModel> guests = new ObservableCollection<GuestViewModel>();

            foreach (Guest guest in _order.Guests)
            {
                guests.Add(new GuestViewModel(guest));
            }

            return guests;
        }
    }
}
