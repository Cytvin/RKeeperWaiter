﻿using RKeeperWaiter.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using WaiterMobile.Views;
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

        public void SetCurrentOrder(OrderViewModel order)
        {
            _currentOrder = order;
        }
        
        private void OnGoToBack()
        {
            Shell.Current.Navigation.PopAsync(true);
        }

        private void OnIncreaseModifierCount(ModifierViewModel modifier)
        {
            ModifiersGroupViewModel modifierGroup = ModifiersSheme.RequiredModifiersGroups.SingleOrDefault(g => g.Modifiers.Contains(modifier));

            if (modifierGroup == null)
            {
                modifierGroup = ModifiersSheme.OptionalModifiersGroups.SingleOrDefault(g => g.Modifiers.Contains(modifier));
            }

            if (modifierGroup != null)
            {
                if (modifierGroup.IsInUpLimit)
                {
                    modifier.IncreaseCount.Execute(modifier);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
                }
            }
            _currentOrder.OnPropertyChanged(nameof(_currentOrder.Total));
        }

        private void OnDecreaseModifierCount(ModifierViewModel modifier) 
        {
            modifier.DecreaseCount.Execute(modifier);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
            _currentOrder.OnPropertyChanged(nameof(_currentOrder.Total));
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
                string[] parameters = { "Общие", "Новый гость"};
                string[] guests = _currentOrder.Guests.Select(g => g.Name).ToArray();
                parameters = parameters.Concat(guests).ToArray();

                string guest = await Shell.Current.DisplayActionSheet("Выберите гостя:", "Отмена", null,  parameters);

                GuestViewModel source = null;
                GuestViewModel destination = null;

                if (_dish.Seat != "0")
                {
                    source = _currentOrder.Guests.Single(g => g.Label == _dish.Seat);
                }

                if (guest == "Общие")
                {
                    TransferToCommons(source);
                    return;
                }
                else if (guest == "Новый гость")
                {
                    _currentOrder.AddGuest.Execute(_currentOrder);
                    destination = _currentOrder.Guests.Last();

                    TransferToAnotherGuest(source, destination);
                    return;
                }
                else if (guest == "Отмена")
                {
                    return;
                }
                else
                {
                    destination = _currentOrder.Guests.Single(g => g.Name == guest);

                    TransferToAnotherGuest(source, destination);
                    return;
                }
            }
            else if (transferType == "В другой заказ")
            {
                Shell.Current.Navigation.PushAsync( new TransferDish(_currentOrder.InternalOrder, this) , true);
            }
        }

        private void TransferToCommons(GuestViewModel source)
        {
            if (source == null)
            {
                Shell.Current.DisplayToastAsync("Блюдо уже в группе \"общие\"", 2000);
                return;
            }

            source.RemoveDish(this);
            _currentOrder.AddDishToCommonDishes(_dish);
            Shell.Current.DisplayToastAsync($"Блюдо пермещено в Общие", 2000);
            Shell.Current.Navigation.PopAsync(true);
        }

        private void TransferToAnotherGuest(GuestViewModel source, GuestViewModel destination)
        {
            if (destination.Label == _dish.Seat)
            {
                Shell.Current.DisplayToastAsync($"Блюдо уже у гостя {destination.Name}", 2000);
                return;
            }

            if (source == null)
            {
                _currentOrder.CommonDishes.Remove(this);
            }
            else
            {
                source.RemoveDish(this);
            }

            destination.InsertDish(_dish);
            Shell.Current.DisplayToastAsync($"Блюдо пермещено к {destination.Name}", 2000);
            Shell.Current.Navigation.PopAsync(true);
        }
    }
}
