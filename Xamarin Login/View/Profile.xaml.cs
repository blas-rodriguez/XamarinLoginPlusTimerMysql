using Akavache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Timers;
using Xceed.Wpf.Toolkit;
using System.Threading;
using Timer = System.Timers.Timer;
using MySqlConnector;
using Android.Content;
using Android.App;

namespace Xamarin_Login
{

    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class Profile : ContentPage
    {
        Timer timer;
        public int hour = 0, minute = 0, second = 0, id_equipo, usuarioPuntos=0, teamPuntos=0, equipoID;
        public string  iduser, FirstName_1, dateborn_1, nameTeam, appcolectivo_, apptrivia_, video_;
        public  Profile()
        {
            InitializeComponent();
            Akavache.Registrations.Start("XamarinLogin");
            
        }

       

        private    void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
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
                    command.CommandText = $"SELECT * FROM users WHERE ( id = {iduser}) ;";
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                             FirstName_1 = reader.GetString(2);
                             dateborn_1 = reader.GetDateTime(4).ToString();
                             id_equipo = reader.GetInt32(14);
                            
                             usuarioPuntos = reader.GetInt32(5);

                        }
                        reader.Close();
                    }
                    command.CommandText = $"SELECT * FROM equipos WHERE ( id_equipo = {id_equipo}) ;";
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            nameTeam = reader.GetString(2);
                            equipoID = reader.GetInt32(0);
                        }
                        reader.Close();
                    }
                    command.CommandText = $"SELECT * FROM puntajes WHERE ( equipo_id  = {equipoID}) ;";
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            teamPuntos = reader.GetInt32(2);
                        }
                        reader.Close();
                    }
                    command.CommandText = $"SELECT * FROM operaciones  WHERE ( id  = 1) ;";
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if( reader.GetInt32(1)==1)
                                video_ = "Activado";
                            else
                                video_ = "Desactivado";

                            if (reader.GetInt32(2) == 1)
                                apptrivia_ = "Activado";
                            else
                                apptrivia_ = "Desactivado";

                            if (reader.GetInt32(3) == 1)
                            {

                                appcolectivo_ = "Activado";
                            }
                                
                            else
                                appcolectivo_ = "Desactivado";
                        }
                        reader.Close();
                    }

                }
                conn.Close();
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                //firstnameLabel.Text = DateTime.Now.ToString("hh:mm:ss");
                firstnameLabel.Text = FirstName_1;
                lastnameLabel.Text = "Puntos:"+ usuarioPuntos;
                usernameLabel.Text = ""+ nameTeam;
                emailLabel.Text = "Puntos:"+ teamPuntos;
                appcolectivo.Text = "APP colectivo:"+ appcolectivo_;
                apptrivia.Text = "APP trivia:"+ apptrivia_;
                video.Text = "Video:"+ video_;
            });

        }

        

        protected override async void OnAppearing()
        {
            
            CancellationTokenSource source = new CancellationTokenSource();
            // Este proceso es usando un task para ejecutar una tarea cada lapso de tiempo
            //int second = 0;
            //var timer = new System.Timers.Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);
            //timer.Elapsed += async (sender, e) => {

            //    //int second = 0;                ;
            //    second++;
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        firstnameLabel.Text = "ejecutado: " + second.ToString();
            //    });
            //};
            //timer.Start();
            iduser = await BlobCache.UserAccount.GetObject<string>("userid");
                timer = new Timer();
                timer.Interval = 2000;
                timer.Elapsed += Timer_Elapsed;
                timer.AutoReset = true;
                timer.Enabled = true;

                timer.Start();

            

            base.OnAppearing();
        }

        private void Logout_Button_Clicked(object sender, EventArgs e)
        {
            //Application.Current.MainPage = new NavigationPage(new Login());
        }

        private void Login_Button_Clicked1(object sender, EventArgs e)
        {
            //string whatsapp = "com.whatsapp";
            string whatsapp = "com.DefaultCompany.WayraColeCliente";
            Intent intent = new Intent(Intent.ActionView);
            intent.AddFlags(ActivityFlags.NewTask);
            Intent mainIntent = Android.App.Application.Context.PackageManager.GetLaunchIntentForPackage(whatsapp);
            
            Android.App.Application.Context.StartActivity(mainIntent);


        }
            private void Login_Button_Clicked(object sender, EventArgs e)
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
                    command.CommandText = $"UPDATE users SET puntaje=10+{usuarioPuntos} WHERE (id = {iduser}) ;";
                    command.ExecuteNonQuery();
                    command.CommandText = $"UPDATE puntajes SET puntaje=10+{teamPuntos} WHERE (equipo_id  = {equipoID}) ;";
                    command.ExecuteNonQuery();
                    LoadingIndicator.IsRunning = false;
                    LoadingIndicator.IsVisible = false;
                    LoginButton.IsVisible = true;



                }
                conn.Close();
            }
        }



    }
}