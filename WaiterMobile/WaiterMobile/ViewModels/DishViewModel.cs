using RKeeperWaiter.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace WaiterMobile.ViewModels
{
    public class DishViewModel : INotifyPropertyChanged
    {
        private Dish _dish;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand GoToBack { get; private set; }
        public ICommand SelectUnselectModifier { get; private set; }
        public string Name => _dish.Name;
        public IEnumerable<Course> Courses => App.Waiter.Courses;
        public ModifiersSheme Modifiers { get; private set; }
        public Course SelectedCourse { get; set; }

        public DishViewModel(Dish dish) 
        {
            _dish = dish;
            Modifiers = _dish.ModifiersSheme;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Modifiers)));
            GoToBack = new Command(OnGoToBack);
            SelectUnselectModifier = new Command<Modifier>(OnSelectUnselectModifier);
        }

        private void OnGoToBack()
        {
            Shell.Current.Navigation.PopAsync(true);
        }

        private void OnSelectUnselectModifier(Modifier modifier)
        {
            if (_dish.Modifiers.Contains(modifier))
            {
                _dish.RemoveModifier(modifier);
                return;
            }

            _dish.InsertModifier(modifier);
        }
    }
}
