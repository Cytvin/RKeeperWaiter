using System;
using WaiterMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaiterMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dishes : ContentPage
    {
        public Dishes(Action<DishViewModel> addDish)
        {
            InitializeComponent();

            BindingContext = new CategoryViewModel(_dishesGrid, addDish);
        }
    }
}