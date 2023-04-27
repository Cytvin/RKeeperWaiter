using RKeeperWaiter;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
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

        public string ApplicationGuid => App.ApplicationGuid.ToString();

        public SettingsViewModel()
        {
            _settings = new Settings();
            GoToBack = new Command(Return);
            Save = new Command(SaveSettings);
            DisplaySettings();
        }

        private void DisplaySettings()
        {
            if (File.Exists(App.SettingsFile) == false)
            {
                return;
            }

            XDocument settings = XDocument.Load(App.SettingsFile);

            XElement root = settings.Root;

            StationId = root.Element("StationId").Value;
            IP = root.Element("ServerIp").Value;
            Port = root.Element("ServerPort").Value;
            Login = root.Element("UserLogin").Value;
            Password = root.Element("UserPassword").Value;
        }

        private void Return()
        {
            Shell.Current.Navigation.PopAsync(true);
        }

        private void SaveSettings()
        {
            string authString = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{Login}:{Password}"));

            App.Waiter.SetStationId(StationId);
            App.Waiter.NetworkService.SetParameters(IP, Port, authString);

            StringBuilder stringBuilder = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(stringBuilder))
            {
                writer.WriteStartDocument();

                writer.WriteStartElement("Settings");

                writer.WriteStartElement("ApplicationGuid");
                writer.WriteValue(App.ApplicationGuid.ToString());
                writer.WriteEndElement();

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

                writer.WriteStartElement("AuthString");
                writer.WriteValue(authString);
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
