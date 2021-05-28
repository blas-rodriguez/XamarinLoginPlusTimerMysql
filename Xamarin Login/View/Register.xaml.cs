using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin_Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        public Register()
        {
            InitializeComponent();
        }

        private async void Register_Button_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Entry_firstname.Text) || String.IsNullOrWhiteSpace(Entry_firstname.Text))
            {
                await DisplayAlert("Login Failed", "Please enter your First Name", "OK");
            }
            else if (String.IsNullOrEmpty(Entry_lastname.Text) || String.IsNullOrWhiteSpace(Entry_lastname.Text))
            {
                await DisplayAlert("Login Failed", "Please enter your Last Name", "OK");
            }
            else if (String.IsNullOrEmpty(Entry_username.Text) || String.IsNullOrWhiteSpace(Entry_username.Text))
            {
                await DisplayAlert("Login Failed", "Please enter your Username", "OK");
            }
            else if (String.IsNullOrEmpty(Entry_email.Text) || String.IsNullOrWhiteSpace(Entry_email.Text))
            {
                await DisplayAlert("Login Failed", "Please enter your Email", "OK");
            }
            else if (String.IsNullOrEmpty(Entry_password.Text) || String.IsNullOrWhiteSpace(Entry_password.Text) && Entry_password.Text.Length >= 8)
            {
                await DisplayAlert("Login Failed", "Please enter your Password", "OK");
            }
            else if (Entry_password.Text.Length < 8)
            {
                await DisplayAlert("Login Failed", "Your password is not 8 characters and above", "OK");
            }
            else
            {
                LoadingIndicator.IsRunning = true;
                LoadingIndicator.IsVisible = true;
                RegisterButton.IsVisible = false;

                var builder = new MySqlConnectionStringBuilder
                {
                    Server = "vm-streaming.mysql.database.azure.com",
                    Database = "xamarin_db",
                    UserID = "haliknihudas@vm-streaming",
                    Password = "Hudas@0228",
                    SslMode = MySqlSslMode.Required,
                };

                using (var conn = new MySqlConnection(builder.ConnectionString))
                {
                    conn.Open();

                    using (var command = conn.CreateCommand())
                    {
                        bool exist = false;

                        command.CommandText = $"SELECT * FROM credentials WHERE username = '{Entry_username.Text}';";
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                exist = true;
                                await DisplayAlert("Register Failed", "Username Exist", "OK");
                                LoadingIndicator.IsRunning = false;
                                LoadingIndicator.IsVisible = false;
                                RegisterButton.IsVisible = true;
                            }
                        }

                        command.CommandText = $"SELECT * FROM credentials WHERE email = '{Entry_email.Text}';";
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                exist = true;
                                await DisplayAlert("Register Failed", "Email Exist", "OK");
                                LoadingIndicator.IsRunning = false;
                                LoadingIndicator.IsVisible = false;
                                RegisterButton.IsVisible = true;
                            }
                        }

                        if (!exist)
                        {
                            command.CommandText = $"INSERT INTO credentials (firstname, lastname, username, email, password) VALUES ('{Entry_firstname.Text}','{Entry_lastname.Text}','{Entry_username.Text}', '{Entry_email.Text}', '{Entry_password.Text}');";
                            command.ExecuteNonQuery();

                            command.CommandText = $"SELECT * FROM credentials WHERE username='{Entry_username.Text}' AND email='{Entry_email.Text}';";
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    await DisplayAlert("Register Success", $"Welcome, {reader.GetString(0)}!", "OK");
                                    LoadingIndicator.IsRunning = false;
                                    LoadingIndicator.IsVisible = false;
                                    RegisterButton.IsVisible = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Login_Button_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new Login());
        }
    }
}