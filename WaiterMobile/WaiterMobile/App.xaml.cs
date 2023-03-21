using System;
using System.IO;
using System.Xml.Linq;
using WaiterMobile;
using Xamarin.Forms;
using RKeeperWaiter;
using Xamarin.Essentials;

namespace WaiterMobile
{
    public partial class App : Application
    {
        public static string FolderPath { get; private set; }
        public static string SettingsFile { get; private set; }
        public static Waiter Waiter { get; private set; }
        public static DisplayInfo CurrentDisplay { get; private set; }

        public App()
        {
            InitializeComponent();

            FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            SettingsFile = Path.Combine(FolderPath, "settings.xml");

            Waiter = new Waiter();
            MainPage = new AppShell();
            CurrentDisplay = DeviceDisplay.MainDisplayInfo;
        }

        protected override void OnStart()
        {
            LoadSettings();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private void LoadSettings()
        {
            if (File.Exists(SettingsFile) == false)
            {
                return;
            }

            XDocument settings = XDocument.Load(SettingsFile);

            XElement root = settings.Root;

            string stationId = root.Element("StationId").Value;
            string ip = root.Element("ServerIp").Value;
            string port = root.Element("ServerPort").Value;
            string login = root.Element("UserLogin").Value;
            string password = root.Element("UserPassword").Value;

            Waiter.NetworkService.SetParameters(ip, port, login, password);
            Waiter.SetStationId(stationId);
        }
    }
}