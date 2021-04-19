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

        public IList<Journey> reservedJourney { get; private set; }
        public IList<Journey> proposedJourney { get; private set; }
        private bool isAuthentified = false;
        private Client client;
        
        public HomePage()
        {
            try
            {
                reservedJourney = new List<Journey>();
                proposedJourney = new List<Journey>();
                if (isAuthentified)
                {
                    this.reservedJourney = DatabaseInteraction.GetReservedJourneyList(client.Email);
                    this.proposedJourney = DatabaseInteraction.GetProposedJourneyList(client.Email);
                }
                this.reservedJourney = DatabaseInteraction.GetReservedJourneyList("milaclim@gmail.com");
                Console.WriteLine(reservedJourney[0].ToString());
                


                BindingContext = this;
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
            await Shell.Current.Navigation.PushAsync(new LoginPage(this.client, isAuthentified));
        }
    }
}