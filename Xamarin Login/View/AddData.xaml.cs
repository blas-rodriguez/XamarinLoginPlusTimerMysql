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

namespace Xamarin_Login.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddData : ContentPage
    {
        public AddData()
        {
            InitializeComponent();
        }

        private async void AddDataButton_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Entry_devicemodel.Text) || String.IsNullOrWhiteSpace(Entry_devicemodel.Text))
            {
                await DisplayAlert("Login Failed", "Please enter your Device Model", "OK");
            }
            else if (String.IsNullOrEmpty(Entry_devicebrand.Text) || String.IsNullOrWhiteSpace(Entry_devicebrand.Text))
            {
                await DisplayAlert("Login Failed", "Please enter your Device Brand", "OK");
            }
            else if (String.IsNullOrEmpty(Entry_deviceos.Text) || String.IsNullOrWhiteSpace(Entry_deviceos.Text))
            {
                await DisplayAlert("Login Failed", "Please enter your Device OS", "OK");
            }
            else
            {
                LoadingIndicator.IsRunning = true;
                LoadingIndicator.IsVisible = true;
                AddDataButton.IsVisible = false;

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
                        var username = await BlobCache.UserAccount.GetObject<string>("username");
                        command.CommandText = $"INSERT INTO deviceinfo_tbl (username, devicemodel, devicebrand, deviceos, insertdate) VALUES ('{username}','{Entry_devicemodel.Text}','{Entry_devicebrand.Text}', '{Entry_deviceos.Text}', '{DateTime.Now.ToShortDateString()}');";
                        command.ExecuteNonQuery();

                        command.CommandText = $"SELECT * FROM deviceinfo_tbl;";
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Console.WriteLine(string.Format(
                                    "Username = {0}, Model = {1}, Brand = {2}, OS = {3}, Date = {4})",
                                    reader.GetString(0),
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    reader.GetString(3),
                                    reader.GetString(4)));
                            }
                        }

                        await DisplayAlert("Add Success!", "You've submitted your device info.", "OK");

                        Entry_devicemodel.Text = String.Empty;
                        Entry_devicebrand.Text = String.Empty;
                        Entry_deviceos.Text = String.Empty;

                        LoadingIndicator.IsRunning = false;
                        LoadingIndicator.IsVisible = false;
                        AddDataButton.IsVisible = true;
                    }
                }
            }
        }
    }
}