using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaiterMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Settings : ContentPage
	{
        private string _settingsPath;

		public Settings ()
		{
			InitializeComponent ();
            _settingsPath = Path.Combine(App.FolderPath, "settings.xml");
            LoadSettings();
        }

        private void LoadSettings()
        {
            if (File.Exists(_settingsPath) == false)
            {
                return;
            }

            XDocument settings = XDocument.Load(_settingsPath);

            XElement root = settings.Root;

            _stationId.Text = root.Element("StationId").Value;
            _ip.Text = root.Element("ServerIp").Value;
            _port.Text = root.Element("ServerPort").Value;
            _login.Text = root.Element("UserLogin").Value;
            _password.Text = root.Element("UserPassword").Value;
        }

        private void OnBackButtonClick(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("..", true);
        }

        private void OnSaveButtonClick(object sender, EventArgs e)
        {
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
            settings.Save(_settingsPath);

            Shell.Current.GoToAsync("..", true);
        }
    }
}