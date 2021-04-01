using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Npgsql;

namespace PlatoVoiturage1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {

        private List<int> journey = new List<int>();
        
        public HomePage()
        {
            InitializeComponent();

            for(int i = 0; i<10; i++)
            {
                journey.Add(i);
            }

            //DataBase test
           
        }


        private async Task connectToDatabase()
        {
            var cs = "Host=192.168.1.139;Username=postgres;Password=platovoiturage;Database=postgres";

            try
            {
                var con = new NpgsqlConnection(cs);
                await con.OpenAsync();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connection open");
                    var command = con.CreateCommand();
                    command.CommandText = "SELECT * FROM trajet;";
                    try
                    {
                        var reader = command.ExecuteReaderAsync();

                        con.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("plouf2");
                        Console.WriteLine(e.ToString());
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine("plouf1");
                Console.WriteLine(e.ToString());
            }

        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (!Shell.Current.FlyoutIsPresented)
            {
                Shell.Current.FlyoutIsPresented = true;
            }
        }

        private async void GoToLoginPage(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new LoginPage());
        }
    }
}