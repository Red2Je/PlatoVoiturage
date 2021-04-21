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

        public IList<Journey> ReservedJourney { get; private set; }
        public IList<Journey> ProposedJourney { get; private set; }
        private bool IsAuthentified = false;
        private Client Client;
        
        public HomePage()
        {
            try
            {
                ReservedJourney = new List<Journey>();
                ProposedJourney = new List<Journey>();
                if (IsAuthentified)
                {
                    this.ReservedJourney = DatabaseInteraction.GetReservedJourneyList(Client.Email);
                    this.ProposedJourney = DatabaseInteraction.GetProposedJourneyList(Client.Email);
                }
                this.ReservedJourney = DatabaseInteraction.GetReservedJourneyList("milaclim@gmail.com");
                Console.WriteLine("bite"+ReservedJourney[0].ToString()) ;
                


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
            await Shell.Current.Navigation.PushAsync(new LoginPage(this.Client, IsAuthentified));
        }

        private async void GiveDetails(object sender, EventArgs e)
        {
            ImageButton s = (ImageButton)sender;

            await Shell.Current.Navigation.PushAsync(new JourneyDetail((Journey)s.BindingContext, this));
        }
    }
}