using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            int userCodeLenth = _userCode.Text.Length;

            if (userCodeLenth == 0)
            {
                return;
            }

            _userCode.Text = _userCode.Text.Remove(userCodeLenth - 1);
        }

        private void OnButtonSettingsClick(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync($"{nameof(Settings)}");
        }
    }
}