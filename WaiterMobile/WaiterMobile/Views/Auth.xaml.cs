using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaiterMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Auth : ContentPage
    {
        public Auth()
        {
            InitializeComponent();
        }

        private void OnNumberButtonClick(object sender, EventArgs e)
        {
            Button numberButton = sender as Button;

            if (_userCode.Text.Length >= 8)
            {
                return;
            }

            _userCode.Text += numberButton.Text;
        }

        private void OnButtonDeleteClick(object sender, EventArgs e)
        {
            int userCodeLength = _userCode.Text.Length;

            if (userCodeLength == 0)
            {
                return;
            }

            _userCode.Text = _userCode.Text.Remove(userCodeLength - 1);
        }

        private void OnButtonSettingsClick(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PushAsync(new Settings());
        }

        private void OnButtonEnterClick(object sender, EventArgs e)
        {
            try
            {
                App.Waiter.UserAuthorization(_userCode.Text);
            }
            catch (InvalidOperationException ex) 
            {
                DisplayAlert("Вход не выполнен", "Не найден пользователь с таким кодом", "OK");
                return;
            }

            App.Waiter.DownloadReferences();

            Shell.Current.Navigation.PushAsync(new Orders());
        }
    }
}