using PlatoVoiturage1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Windows.Input; 


namespace PlatoVoiturage1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {

        public static IList<Journey> ReservedJourney { get; private set; }
        public static IList<Journey> ProposedJourney { get; private set; }
        public Client Client { get; set; }

        public ICommand RefreshCommand => new Command(refreshPage);

        bool isRefreshing;
        public bool IsRefreshing
        {
            get {
                return isRefreshing;
            }
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
            }

        }

        public HomePage()
        {
            
            InitializeComponent();
            BindingContext = this;
            ReservedJourney = new List<Journey>();
            ProposedJourney = new List<Journey>();
            ReservedView.SetBinding(CollectionView.ItemsSourceProperty, nameof(ReservedJourney));
            ProposedView.SetBinding(CollectionView.ItemsSourceProperty, nameof(ProposedJourney));
            ReservedView.ItemsSource = ReservedJourney;
            ProposedView.ItemsSource = ProposedJourney;

            if (InfoExchanger.IsAuthentified)
            {
                this.Client = InfoExchanger.User;
                ConnectionButton.Text = "Se Déconnecter";
                ConnectionButton.Clicked -= GoToLoginPage;
                ConnectionButton.Clicked += Disconnect;

            }



            displayJourney();
        }

        protected override void OnAppearing()
        {
            IsRefreshing = false;
            ReservedJourney = new List<Journey>();
            ProposedJourney = new List<Journey>();

            if (InfoExchanger.IsAuthentified)
            {
                this.Client = InfoExchanger.User;
                ConnectionButton.Text = "Se Déconnecter";
                ConnectionButton.Clicked -= GoToLoginPage;
                ConnectionButton.Clicked += Disconnect;

            }


            displayJourney();
            base.OnAppearing();
        }




        private void displayJourney()
        {
            ReservedJourney = new List<Journey>();
            ProposedJourney = new List<Journey>();


            if (InfoExchanger.IsAuthentified)
            {
                ReservedJourney = DatabaseInteraction.GetReservedJourneyList(Client.Email);
                ProposedJourney = DatabaseInteraction.GetProposedJourneyList(Client.Email);
            }

            ReservedView.ItemsSource = ReservedJourney;
            ProposedView.ItemsSource = ProposedJourney;
        }

        void refreshPage()
        {
            IsRefreshing = true;
            displayJourney();
            if (InfoExchanger.IsAuthentified)
            {
                this.Client = InfoExchanger.User;
                ConnectionButton.Text = "Se Déconnecter";
                ConnectionButton.Clicked -= GoToLoginPage;
                ConnectionButton.Clicked += Disconnect;

            }
            else
            {
                ConnectionButton.Text = "Connexion";
                ConnectionButton.Clicked -= Disconnect;
                ConnectionButton.Clicked += GoToLoginPage;
            }

            IsRefreshing = false;

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

        private void Disconnect(object sender, EventArgs e)
        {
            InfoExchanger.Email = null;
            InfoExchanger.IsAuthentified = false;
            InfoExchanger.User = null;
            
            RefreshCommand.Execute(null);
        }

        private async void GiveDetails(object sender, EventArgs e)
        {
            ImageButton s = (ImageButton)sender;

            await Shell.Current.Navigation.PushAsync(new JourneyDetail((Journey)s.BindingContext, this));
        }
    }
}