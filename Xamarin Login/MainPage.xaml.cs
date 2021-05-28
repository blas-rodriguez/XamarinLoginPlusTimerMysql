using Akavache;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin_Login
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Akavache.Registrations.Start("XamarinLogin");
        }

        protected override async void OnAppearing()
        {
            await Task.Delay(3000);

            string name = "";
            try
            {
                name = await BlobCache.UserAccount.GetObject<string>("username");
            }
            catch (KeyNotFoundException ex)
            {
            }

            if (name.Length == 0)
            {
                Application.Current.MainPage = new Login();
            }
            else
            {
                Application.Current.MainPage = new SideNav();
            }

            base.OnAppearing();
        }
    }
}