using RKeeperWaiter.Models;
using System.Collections.Generic;
using System.ComponentModel;
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
        public ModifiersSheme Modifiers => _dish.ModifiersSheme;
        public Course SelectedCourse { get; set; }

        public DishViewModel(Dish dish) 
        {
            _dish = dish;
            GoToBack = new Command(OnGoToBack);
            SelectUnselectModifier = new Command<Modifier>(OnSelectUnselectModifier);
        }

        private void OnGoToBack()
        {
            Shell.Current.Navigation.PopAsync(true);
        }

        private void OnSelectUnselectModifier(Modifier modifier)
        {
            if (modifier.Selected == false)
            {
                modifier.Select();
            }
        }
    }
}
