using System;
using System.IO;
using System.Xml.Linq;
using WaiterMobile;
using Xamarin.Forms;
using RKeeperWaiter;
using Xamarin.Essentials;
using Xamarin.Forms.PlatformConfiguration;

namespace WaiterMobile
{
    public partial class App : Application
    {
        public static string FolderPath { get; private set; }
        public static string SettingsFile { get; private set; }
        public static IWaiter Waiter { get; private set; }
        public static DisplayInfo CurrentDisplay { get; private set; }
        public static Guid ApplicationGuid { get; private set; }

        public App()
        {
            InitializeComponent();

            FolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            SettingsFile = Path.Combine(FolderPath, "settings.xml");

            Waiter = new WaiterTest();
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
                ApplicationGuid = Guid.NewGuid();
                return;
            }

            XDocument settings = XDocument.Load(SettingsFile);

            XElement root = settings.Root;

            ApplicationGuid = Guid.Parse(root.Element("ApplicationGuid").Value);
            string stationId = root.Element("StationId").Value;
            string ip = root.Element("ServerIp").Value;
            string port = root.Element("ServerPort").Value;
            string authString = root.Element("AuthString").Value;

            Waiter.NetworkService.SetParameters(ip, port, authString);
            Waiter.SetStationId(stationId);
        }
    }
}