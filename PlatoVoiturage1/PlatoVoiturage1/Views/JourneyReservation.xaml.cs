using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

using PlatoVoiturage1.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace PlatoVoiturage1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    //This page is similar to the journey detail page, except it allows the user to reserve the journey he is looking at. This page is used in the journey search page
    public partial class JourneyReservation : ContentPage
    {
        public string AddressDep { get; set; }
        public string VilleDep { get; set; }
        public string AddressArr { get; set; }
        public string VilleArr { get; set; }
        public string Hdep { get; set; }
        public string Harr { get; set; }
        public string Km { get; set; }
        public string Nbplaces { get; set; }
        public string Comm { get; set; }

        private DateTime TimeDeparture { get; set; }
        private DateTime TimeArrival { get; set; }
        private Journey j;
        private JourneySearchPage searchPage;
        //see journey details page
        public JourneyReservation(Journey j, JourneySearchPage searchPage)
        {
            BindingContext = this;

            this.j = j;
            AddressDep = "Adresse de départ : " + j.AdressDep;
            AddressArr = "Adresse d'arrivée : " + j.AdresseArr;
            Km = "Détour toloéré (km) : " + j.Km.ToString();
            Nbplaces = "Nombre de places restantes : " + j.NbPlaces.ToString();
            VilleDep = "Ville de départ : " + j.VilleDep;
            VilleArr = "Ville d'arrivée : " + j.VilleArr;
            Comm = "Commentaire : \n" + j.Comm;
            this.TimeDeparture = DateTime.ParseExact(j.Hdep, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            Hdep = "Heure de départ : " + TimeDeparture.ToString();
            this.TimeArrival = DateTime.ParseExact(j.Harr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            Harr = "Heure d'arrivée : " + TimeArrival.ToString();
            InitializeComponent();
            if (j.Pets)
            {
                dog.BackgroundColor = Color.Green;
            }
            if (j.Smoke)
            {
                smoke.BackgroundColor = Color.Green;
            }
            if (j.Music)
            {
                music.BackgroundColor = Color.Green;
            }
            if (j.Talk)
            {
                talk.BackgroundColor = Color.Green;
            }



            
            this.searchPage = searchPage;
        }

        //This overriden event handler allows the user to reserve a journey only if he is logged in
        protected override void OnAppearing()
        {
            if (InfoExchanger.IsAuthentified)
            {
                ReservationButton.IsEnabled = true;
                ReservationButton.Text = "Réserver ce trajet";
            }
            else
            {
                ReservationButton.Text = "Veuillez vous connecter pour réserver ce trajet";
            }
            base.OnAppearing();
        }

        //This method allows the user to make a reservation on the journey
        private async void MakeReservation(object sender, EventArgs e)
        {
            DatabaseInteraction.Reserve(j.Eid, InfoExchanger.Email);
            await DisplayAlert("Votre trajet a bien été réservé", "Vous pouvez continuer vos recherches", "OK");
            await Shell.Current.Navigation.PopAsync();

        }
        //see journey details page
        private async void GoToMaps(object sender, EventArgs e)
        {
            string mapString = "http://maps.google.com/?daddr=" + j.AdressDep+" "+j.VilleDep + "&saddr=" + j.AdresseArr+" "+j.VilleArr;
            if (Device.RuntimePlatform == Device.Android)
            {
                await Launcher.OpenAsync(mapString);
            }
        }
        //see journey details page
        private async void GoBack(object sender, EventArgs e)
        {
            BindingContext = searchPage;
            await Shell.Current.Navigation.PopAsync();
        }
    }
}