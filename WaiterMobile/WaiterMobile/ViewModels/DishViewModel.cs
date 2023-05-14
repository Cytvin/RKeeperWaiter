using RKeeperWaiter.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace WaiterMobile.ViewModels
{
    public class DishViewModel : INotifyPropertyChanged
    {
        private Dish _dish;
        private Func<DishViewModel, bool> _remove;
        private OrderViewModel _currentOrder;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand GoToBack { get; private set; }
        public ICommand IncreaseModifierCount { get; private set; }
        public ICommand DecreaseModifierCount { get; private set; }
        public ICommand CourseSelected { get; private set; }
        public ICommand Remove { get; private set; }
        public ICommand Transfer { get; private set; }
        public string Name => _dish.Name;
        public IEnumerable<Course> Courses => App.Waiter.Courses;
        public ModifiersShemeViewModel ModifiersSheme { get; private set; }
        public Course SelectedCourse { get; set; }
        public Dish InternalDish => _dish;
        public decimal Price => _dish.Price;

        public DishViewModel(Dish dish, Func<DishViewModel, bool> remove) 
        {
            _dish = dish;
            SelectedCourse = _dish.Course;
            GoToBack = new Command(OnGoToBack);
            CourseSelected = new Command(OnCourseSelected);
            IncreaseModifierCount = new Command<ModifierViewModel>(OnIncreaseModifierCount);
            DecreaseModifierCount = new Command<ModifierViewModel>(OnDecreaseModifierCount);
            Remove = new Command(OnRemove);
            Transfer = new Command(OnTransfer);
            ModifiersSheme = new ModifiersShemeViewModel(dish.ModifiersSheme);

            _remove = remove;
        }

        private void OnGoToBack()
        {
            Shell.Current.Navigation.PopAsync(true);
        }

        private void OnIncreaseModifierCount(ModifierViewModel modifier)
        {
            ModifiersGroupViewModel modifierGroup = ModifiersSheme.ModifiersGroups.Single(g => g.Modifiers.Contains(modifier));

            if (modifierGroup != null)
            {
                if (modifierGroup.IsInUpLimit)
                {
                    modifier.IncreaseCount.Execute(modifier);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
                }
            }
        }

        private void OnDecreaseModifierCount(ModifierViewModel modifier) 
        {
            modifier.DecreaseCount.Execute(modifier);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
        }

        private void OnCourseSelected()
        {
            _dish.Course = SelectedCourse;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCourse)));
        }

        private void OnRemove()
        {
            _remove(this);
            Shell.Current.Navigation.PopAsync(true);
        }

        private async void OnTransfer()
        {
            string transferType = await Shell.Current.DisplayActionSheet("Куда переносим блюдо?", "Отмена", null, "Другому гостю", "В другой заказ");

            if (transferType == "Другому гостю")
            {
                string[] parameters = { "Новому гостю" };
                await Shell.Current.DisplayActionSheet("Выберите гостя:", "Отмена", null, _currentOrder.Guests.Select(g => g.Name).ToArray());
            }
            else
            {
                
            }
        }

        public void SetCurrentOrder(OrderViewModel order)
        {
            _currentOrder = order;
        }
    }
}
