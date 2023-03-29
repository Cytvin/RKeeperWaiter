using RKeeperWaiter;
using System;
using System.Text;
using System.Xml;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaiterMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Settings : ContentPage
	{
		public Settings ()
		{
			InitializeComponent ();
            LoadSettings();
        }

        private void LoadSettings()
        {
            NetworkService networkService = App.Waiter.NetworkService;

            _stationId.Text = App.Waiter.StationId.ToString();
            _ip.Text = networkService.Ip;
            _port.Text = networkService.Port;
            _login.Text = networkService.Login;
            _password.Text = networkService.Password;
        }

        private void OnBackButtonClick(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PopAsync(true);
        }

        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            App.Waiter.SetStationId(_stationId.Text);
            App.Waiter.NetworkService.SetParameters(_ip.Text, _port.Text, _login.Text, _password.Text);

            StringBuilder stringBuilder = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(stringBuilder)) 
            {
                writer.WriteStartDocument();

                writer.WriteStartElement("Setting");

                writer.WriteStartElement("StationId");
                writer.WriteValue(_stationId.Text);
                writer.WriteEndElement();

                writer.WriteStartElement("ServerIp");
                writer.WriteValue(_ip.Text);
                writer.WriteEndElement();

                writer.WriteStartElement("ServerPort");
                writer.WriteValue(_port.Text);
                writer.WriteEndElement();

                writer.WriteStartElement("UserLogin");
                writer.WriteValue(_login.Text);
                writer.WriteEndElement();

                writer.WriteStartElement("UserPassword");
                writer.WriteValue(_password.Text);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }

            XmlDocument settings = new XmlDocument ();
            settings.LoadXml(stringBuilder.ToString());
            settings.Save(App.SettingsFile);

            Shell.Current.Navigation.PopAsync(true);
        }
    }
}