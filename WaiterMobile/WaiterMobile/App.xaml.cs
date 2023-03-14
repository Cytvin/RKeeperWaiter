using System;
using System.IO;
using WaiterMobile;
using Xamarin.Forms;

namespace WaiterMobile
{
    public partial class App : Application
    {
        public static string FolderPath { get; private set; }
        public static string SettingsFile { get; private set; }

        public App()
        {
            InitializeComponent();
            FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            SettingsFile = Path.Combine(App.FolderPath, "settings.xml");
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}