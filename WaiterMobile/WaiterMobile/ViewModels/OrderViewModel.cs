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
using System.Security.Cryptography.X509Certificates;

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
        public ICommand GoToCloseOrder { get; private set; }
        public ICommand CloseOrder { get; private set;}
        public bool ShowSendButton => !_order.IsSend;
        public bool ShowTotalButton => _order.IsSend;
        public decimal Total => _order.Sum;
        public string ReceivedAmount { get; set; }
        public Action<Dish> AddDishToCommonDishes => InsertCommonDish;
        public Func<DishViewModel, bool> RemoveDishFromCommonDishes => RemoveCommonDish;
        public ObservableCollection<DishViewModel> CommonDishes { get; set; }
        public ObservableCollection<GuestViewModel> Guests { get; set; }
        public string OrderName => _order.Name;

        public OrderViewModel(Order order)
        {
            _order = order;

            CommonDishes = MakeDishViewModels();
            Guests = MakeGuestViewOrder();

            GoToBack = new Command(OnGoToBack);
            AddDish = new Command<Action<Dish>>(OnAddDish);
            EditDish = new Command<DishViewModel>(OnEditDish);
            SaveOrder = new Command(OnSaveOrder);
            AddCommentary = new Command(OnAddCommentary);
            AddGuest = new Command(OnGuestAdd);
            GoToCloseOrder = new Command<OrderViewModel>(OnGoToCloseOrder);
            CloseOrder = new Command(OnCloseOrder);

            CommonDishes.CollectionChanged += OnCommonDishesAdded;
            Guests.CollectionChanged += OnGuestAdded;
        }

        private void OnGoToBack()
        {
            Shell.Current.Navigation.PopAsync(true);
        }

        private void OnAddDish(Action<Dish> action)
        {
            Shell.Current.Navigation.PushAsync(new Dishes(action), true);
        }

        private void OnEditDish(DishViewModel dish)
        {
            Shell.Current.Navigation.PushAsync(new DishView(dish, this), true);
        }

        private void OnCommonDishesAdded(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (DishViewModel dish in e.NewItems)
                {
                    dish.InternalDish.Seat = "0";
                    _order.InsertCommonDish(dish.InternalDish);
                }

                return;
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (DishViewModel dish in e.OldItems)
                {
                    _order.RemoveCommonDish(dish.InternalDish);
                }
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowSendButton)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowTotalButton)));
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

        private void OnGoToCloseOrder(OrderViewModel order)
        {
            Shell.Current.Navigation.PushAsync(new OrderTotal(order));
        }

        private void OnCloseOrder()
        {
            decimal receivedAmount = 0;

            if (decimal.TryParse(ReceivedAmount, out receivedAmount) == false)
            {
                Shell.Current.DisplayAlert("Ошибка", "Неверные данные в поле с суммой", "ОК");
                return;
            }

            if (receivedAmount < _order.Sum)
            {
                Shell.Current.DisplayAlert("Ошибка", "Введенная сумма меньше суммы заказа", "ОК");
                return;
            }

            App.Waiter.CloseOrder(_order);
            Shell.Current.DisplayAlert("", $"Заказ успешно закрыт. Сдача: {receivedAmount - _order.Sum}", "ОК");
            Shell.Current.Navigation.PushAsync(new Orders());
        }

        private void InsertCommonDish(Dish dish)
        {
            DishViewModel dishViewModel = new DishViewModel(dish, RemoveDishFromCommonDishes);
            CommonDishes.Add(dishViewModel);
        }

        private bool RemoveCommonDish(DishViewModel dish)
        {
            return CommonDishes.Remove(dish);
        }

        private ObservableCollection<DishViewModel> MakeDishViewModels()
        {
            ObservableCollection<DishViewModel> dishes = new ObservableCollection<DishViewModel>();

            foreach (Dish dish in _order.CommonDishes)
            {
                dishes.Add(new DishViewModel(dish, RemoveDishFromCommonDishes));
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
