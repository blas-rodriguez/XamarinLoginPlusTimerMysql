using Microcharts;
using MySqlConnector;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin_Login.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        private List<Microcharts.ChartEntry> deviceModelChart = new List<Microcharts.ChartEntry>();
        private List<Microcharts.ChartEntry> deviceBrandChart = new List<Microcharts.ChartEntry>();
        private List<Microcharts.ChartEntry> deviceOSChart = new List<Microcharts.ChartEntry>();

        public Dashboard()
        {
            InitializeComponent();
        }

        private string generateColor()
        {
            Random random = new Random();
            string color = string.Empty;
            do
            {
                color = string.Format("#{0:X6}", random.Next(0x1000000));
            }
            while (color == "#000000");

            return color;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var builder = new MySqlConnector.MySqlConnectionStringBuilder
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
                    command.CommandText = $"SELECT devicemodel, COUNT(*) FROM xamarin_db.deviceinfo_tbl GROUP BY devicemodel;";
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string currentcolor = generateColor();
                            deviceModelChart.Add(new ChartEntry(reader.GetInt32(1))
                            {
                                Label = reader.GetString(0),
                                ValueLabel = reader.GetInt32(1).ToString(),
                                ValueLabelColor = SKColor.Parse(currentcolor),
                                Color = SKColor.Parse(currentcolor)
                            });
                        }
                    }

                    command.CommandText = $"SELECT devicebrand, COUNT(*) FROM xamarin_db.deviceinfo_tbl GROUP BY devicebrand;";
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string currentcolor = generateColor();
                            deviceBrandChart.Add(new ChartEntry(reader.GetInt32(1))
                            {
                                Label = reader.GetString(0),
                                ValueLabel = reader.GetInt32(1).ToString(),
                                ValueLabelColor = SKColor.Parse(currentcolor),
                                Color = SKColor.Parse(currentcolor)
                            });
                        }
                    }

                    command.CommandText = $"SELECT deviceos, COUNT(*) FROM xamarin_db.deviceinfo_tbl GROUP BY deviceos;";
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string currentcolor = generateColor();
                            deviceOSChart.Add(new ChartEntry(reader.GetInt32(1))
                            {
                                Label = reader.GetString(0),
                                ValueLabel = reader.GetInt32(1).ToString(),
                                ValueLabelColor = SKColor.Parse(currentcolor),
                                Color = SKColor.Parse(currentcolor)
                            });
                        }
                    }
                }
            }

            DeviceModelChart.Chart = new PieChart { Entries = deviceModelChart };
            DeviceModelChart.Chart.LabelTextSize = 40f;
            DeviceBrandChart.Chart = new PieChart { Entries = deviceBrandChart };
            DeviceBrandChart.Chart.LabelTextSize = 40f;
            DeviceOSChart.Chart = new PieChart { Entries = deviceOSChart };
            DeviceOSChart.Chart.LabelTextSize = 40f;
        }
    }
}