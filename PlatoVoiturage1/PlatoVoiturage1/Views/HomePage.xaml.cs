using PlatoVoiturage1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace PlatoVoiturage1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {

        private List<Journey> journey = new List<Journey>();
        private bool isAuthentified = false;
        private Client client;
        
        public HomePage()
        {
            try
            {
                this.journey = DatabaseInteraction.getReservedJourneyList("milaclim@gmail.com");
                Console.WriteLine("bite : " + journey.ElementAt(0).adresseArr);
            }
            catch (Exception e)
            {
                Console.WriteLine("Avant erreur");
                System.Diagnostics.Debug.WriteLine("Type " + e.GetType().ToString());
                System.Diagnostics.Debug.WriteLine("StackTrace " + e.StackTrace);
                System.Diagnostics.Debug.WriteLine("Message " + e.Message);
                Console.WriteLine("Après erreur");
            }
            InitializeComponent();


            //DataBase test
            
            
           
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