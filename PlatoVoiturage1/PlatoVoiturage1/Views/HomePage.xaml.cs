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

        public static IList<Journey> ReservedJourney { get; private set; }
        public static IList<Journey> ProposedJourney { get; private set; }
        public bool IsAuthentified = false;
        public Client Client { get; set; }
        
        public HomePage()
        {
            
            InitializeComponent();
            BindingContext = this;
            ReservedJourney = new List<Journey>();
            ProposedJourney = new List<Journey>();
            displayJourney();
        }

        public HomePage(Client c, bool isAuthentified)
        {
            InitializeComponent();
            BindingContext = this;
            this.Client = c;
            this.IsAuthentified = isAuthentified;
            displayJourney();
            ConnectionButton.Text = "Se Déconnecter";
            ConnectionButton.Clicked -= GoToLoginPage;
            ConnectionButton.Clicked += Disconnect;
        }

        private void displayJourney()
        {
            ReservedJourney.Clear();
            ProposedJourney.Clear();

            if (IsAuthentified)
            {
                Console.WriteLine("Actualising");
                ReservedJourney = DatabaseInteraction.GetReservedJourneyList(Client.Email);
                ProposedJourney = DatabaseInteraction.GetProposedJourneyList(Client.Email);
                ReservedView.ItemsSource = ReservedJourney;
                ProposedView.ItemsSource = ProposedJourney;
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
            await Shell.Current.Navigation.PushAsync(new LoginPage(this));
        }

        private async void Disconnect(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new HomePage(),this);
            await Navigation.PopToRootAsync();
        }

        private async void GiveDetails(object sender, EventArgs e)
        {
            ImageButton s = (ImageButton)sender;

            await Shell.Current.Navigation.PushAsync(new JourneyDetail((Journey)s.BindingContext, this));
        }
    }
}