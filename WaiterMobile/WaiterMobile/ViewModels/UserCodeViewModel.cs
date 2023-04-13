using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WaiterMobile.Models;
using WaiterMobile.Views;
using Xamarin.Forms;

namespace WaiterMobile.ViewModels
{
    public class UserCodeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private UserCode _currentUser;

        public ICommand AddNumber { private set; get; }
        public ICommand RemoveNumber { private set; get; }
        public ICommand OpenSetting { private set; get; }
        public ICommand Authorization { private set; get; }

        public UserCodeViewModel()
        {
            _currentUser = new UserCode();
            AddNumber = new Command<string>(AddNumberToCode);
            RemoveNumber = new Command(RemoveLastNumberFromeCode);
            OpenSetting = new Command(GoToSetting);
            Authorization = new Command(UserAuthorization);
        }

        public string Code
        {
            get { return _currentUser.Value; }
            set 
            {
                _currentUser.Value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Code)));
            }
        }

        private void AddNumberToCode(string number)
        {
            if (Code.Length >= 8)
            {
                return;
            }

            Code += number;
        }

        private void RemoveLastNumberFromeCode() 
        {
            int codeLength = Code.Length;

            if (codeLength == 0) 
            {
                return;
            }

            Code = Code.Remove(codeLength - 1);
        }

        private void GoToSetting()
        {
            Shell.Current.Navigation.PushAsync(new SettingsView());
        }

        private void UserAuthorization()
        {
            try
            {
                App.Waiter.UserAuthorization(Code);
                App.Waiter.DownloadReferences();
            }
            catch (ArgumentNullException ex)
            {
                Shell.Current.DisplayAlert("Вход не выполнен", ex.Message, "OK");
                return;
            }
            catch (TaskCanceledException ex)
            {
                Shell.Current.DisplayAlert("Ошибка сервера", ex.Message, "OK");
                return;
            }

            Shell.Current.Navigation.PushAsync(new Orders());
        }
    }
}
