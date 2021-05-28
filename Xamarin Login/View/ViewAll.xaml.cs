using MySqlConnector;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin_Login.Model;

namespace Xamarin_Login.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewAll : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        public ViewAll()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            ObservableCollection<DeviceInfo> Items = new ObservableCollection<DeviceInfo>();

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
                    command.CommandText = $"SELECT * FROM deviceinfo_tbl;";
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Items.Add(new DeviceInfo
                            {
                                Username = $"Submitted by {reader.GetString(0)}",
                                DeviceModel = $"Device Model: {reader.GetString(1)}",
                                DeviceBrand = $"Device Brand: {reader.GetString(2)}",
                                DeviceOS = $"Device OS: {reader.GetString(3)}",
                                DateSubmitted = $"Submitted on {reader.GetString(4)}"
                            });
                        }
                    }
                }
            }

            MyListView.ItemsSource = Items;
        }

        private void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}