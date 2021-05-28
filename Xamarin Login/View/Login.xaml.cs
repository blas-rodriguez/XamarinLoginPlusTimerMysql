using Akavache;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin_Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            Akavache.Registrations.Start("XamarinLogin");
        }

        private async void Login_Button_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Entry_User.Text) || String.IsNullOrWhiteSpace(Entry_User.Text))
            {
                await DisplayAlert("Login Failed", "Please enter username or email", "OK");
            }
            //else if (String.IsNullOrEmpty(Entry_Pass.Text) || String.IsNullOrWhiteSpace(Entry_Pass.Text))
            //{
            //    await DisplayAlert("Login Failed", "Please enter password", "OK");
            //}
            else
            {
                LoadingIndicator.IsRunning = true;
                LoadingIndicator.IsVisible = true;
                LoginButton.IsVisible = false;

                var builder = new MySqlConnectionStringBuilder
                {
                    Server = "192.168.1.18",
                    Port = 3306,
                    Database = "sislab",
                    UserID = "blas2",
                    Password = "larioja12345",
                };

                using (var conn = new MySqlConnection(builder.ConnectionString))
                {
                    conn.Open();

                    using (var command = conn.CreateCommand())
                    {
                        //command.CommandText = $"SELECT * FROM users WHERE (username = '{Entry_User.Text}' or email = '{Entry_User.Text}') AND password='{Entry_Pass.Text}';";
                        command.CommandText = $"SELECT * FROM users WHERE ( name = '{Entry_User.Text}') ;";
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                LoadingIndicator.IsRunning = false;
                                LoadingIndicator.IsVisible = false;
                                LoginButton.IsVisible = true;

                                await BlobCache.UserAccount.InsertObject("userid", reader.GetValue(0));
                                await BlobCache.UserAccount.InsertObject("firstname", reader.GetString(3));
                                await BlobCache.UserAccount.InsertObject("lastname", reader.GetString(3));
                                await BlobCache.UserAccount.InsertObject("username", reader.GetString(3));
                                await BlobCache.UserAccount.InsertObject("email", reader.GetString(3));

                                Application.Current.MainPage = new SideNav();
                            }
                            else
                            {
                                await DisplayAlert("Login Failed", "Credentials Doesn't Match", "OK");

                                LoadingIndicator.IsRunning = false;
                                LoadingIndicator.IsVisible = false;
                                LoginButton.IsVisible = true;
                            }
                        }
                    }
                }
            }
        }

        private void Register_Button_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new Register());
        }
    }
}