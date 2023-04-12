using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using System.Xml;
using WaiterMobile.Models;
using Xamarin.Forms;

namespace WaiterMobile.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Settings _settings;

        public ICommand GoToBack { get; private set; }
        public ICommand Save { get; private set; }

        public SettingsViewModel()
        {
            _settings = new Settings();
            GoToBack = new Command(Return);
            Save = new Command(SaveSettings);
            DisplaySettings();
        }

        public string StationId
        {
            get { return _settings.StationId; }
            set
            {
                _settings.StationId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StationId)));
            }
        }

        public string IP
        {
            get { return _settings.IP; }
            set
            {
                _settings.IP = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IP)));
            }
        }

        public string Port
        {
            get { return _settings.Port; }
            set
            {
                _settings.Port = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Port)));
            }
        }

        public string Login
        {
            get { return _settings.Login; }
            set
            {
                _settings.Login = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Login)));
            }
        }

        public string Password
        {
            get { return _settings.Password; }
            set
            {
                _settings.Password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
            }
        }

        private void DisplaySettings()
        {
            StationId = App.Waiter.StationId.ToString();
            IP = App.Waiter.NetworkService.Ip;
            Port = App.Waiter.NetworkService.Port;
            Login = App.Waiter.NetworkService.Login;
            Password = App.Waiter.NetworkService.Password;
        }

        private void Return()
        {
            Shell.Current.Navigation.PopAsync(true);
        }

        private void SaveSettings()
        {
            App.Waiter.SetStationId(StationId);
            App.Waiter.NetworkService.SetParameters(IP, Port, Login, Password);

            StringBuilder stringBuilder = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(stringBuilder))
            {
                writer.WriteStartDocument();

                writer.WriteStartElement("Setting");

                writer.WriteStartElement("StationId");
                writer.WriteValue(StationId);
                writer.WriteEndElement();

                writer.WriteStartElement("ServerIp");
                writer.WriteValue(IP);
                writer.WriteEndElement();

                writer.WriteStartElement("ServerPort");
                writer.WriteValue(Port);
                writer.WriteEndElement();

                writer.WriteStartElement("UserLogin");
                writer.WriteValue(Login);
                writer.WriteEndElement();

                writer.WriteStartElement("UserPassword");
                writer.WriteValue(Password);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }

            XmlDocument settings = new XmlDocument();
            settings.LoadXml(stringBuilder.ToString());
            settings.Save(App.SettingsFile);

            Shell.Current.Navigation.PopAsync(true);
        }
    }
}
