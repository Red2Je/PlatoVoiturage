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
    //This page is the first page presented to the user.
    public partial class HomePage : ContentPage
    {
        //The two lists below are binded to the views that shows the journeys the user reserved and proposed
        public static IList<Journey> ReservedJourney { get; private set; }
        public static IList<Journey> ProposedJourney { get; private set; }
        //A client can be binded to the homepage, modifying it to display the client's journeys
        public Client Client { get; set; }
        //This command is used to refresh the journeys views
        public ICommand RefreshCommand => new Command(RefreshPage);
        //This bool determines wether the page is refreshing or not. Setting this bool to true makes the page refresh
        bool isRefreshing;
        //This attribute allows the page to refresh each time its value is changed
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
            //Always initialize components before doing anything if you want to avoid a LOT of nullpointerexception
            InitializeComponent();
            BindingContext = this;
            ReservedJourney = new List<Journey>();
            ProposedJourney = new List<Journey>();
            //We bind the collection view's properties to their respective watching lists
            ReservedView.SetBinding(CollectionView.ItemsSourceProperty, nameof(ReservedJourney));
            ProposedView.SetBinding(CollectionView.ItemsSourceProperty, nameof(ProposedJourney));
            ReservedView.ItemsSource = ReservedJourney;
            ProposedView.ItemsSource = ProposedJourney;
            //We check if the user is authentified. If so we set the connection button to disconnect, and change the method associated to the button's action
            if (InfoExchanger.IsAuthentified)
            {
                this.Client = InfoExchanger.User;
                ConnectionButton.Text = "Se Déconnecter";
                ConnectionButton.Clicked -= GoToLoginPage;
                ConnectionButton.Clicked += Disconnect;

            }


            //We display the journey if there is some to display
            DisplayJourney();
        }

        //This overriden event handler is used to refresh the page each times an user displays it.
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


            DisplayJourney();
            base.OnAppearing();
        }



        //This methods allows us to interact with the database and to display the journeys the user proposed or reserved
        private void DisplayJourney()
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

        //The main method to refresh a page. The collection views are refreshed alongside with the connection button
        void RefreshPage()
        {
            IsRefreshing = true;
            DisplayJourney();
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

        //This methods handle the click on the burger icon image, showing the navigation tab
        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (!Shell.Current.FlyoutIsPresented)
            {
                Shell.Current.FlyoutIsPresented = true;
            }
        }

        //This method allows us to push a login page to the navigation stack for the user to connect or to create a new account
        private async void GoToLoginPage(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new LoginPage(this));
        }

        //This methods allows an user to disconnect, setting all the attributes of the infoExchanger class to null. The refreshing of the page is also called at the end of the execution of this method
        private void Disconnect(object sender, EventArgs e)
        {
            InfoExchanger.Email = null;
            InfoExchanger.IsAuthentified = false;
            InfoExchanger.User = null;
            
            RefreshCommand.Execute(null);
        }
        
        //This method allows a page containing the details of a journey to be pushed on the navigation stack, thus displaying it
        private async void GiveDetails(object sender, EventArgs e)
        {
            ImageButton s = (ImageButton)sender;

            await Shell.Current.Navigation.PushAsync(new JourneyDetail((Journey)s.BindingContext, this));
        }
    }
}