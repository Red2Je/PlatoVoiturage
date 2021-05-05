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
    //This page allows the user to create a new journey, and registers it into the database.
    public partial class ProposerTrajet : ContentPage
    {
        

        public ProposerTrajet()
        {
            InitializeComponent();
            //For the time, we need it in the format HH:mm:ss
            TimeSpan initTime = new TimeSpan(DateTime.Now.TimeOfDay.Hours, DateTime.Now.TimeOfDay.Minutes, 0);
            DepartureTime.Time = initTime;
            DepartureDate.MinimumDate = DateTime.Now.Date;
            ArrTime.Time = initTime;
            ArrDate.MinimumDate = DepartureDate.Date;
            Console.WriteLine(DepartureTime.Time);
            //We set up the commentary to "" so that it isn't null, as it is the only value that isn't required to be filled by the user
            comm.Text = "";
        }

        //This method handles the validation process of a journey. It checks if it is valid, and if it is, registers it into the database. If it isn't, an error message appears for the user, indicating what he must change.
        private async void Valider(object sender, EventArgs e)
        {
            //First check : the user must fill the values
            if(depAd.Text == null || arrAd.Text == null || pasNum == null || depVil == null || arrVil == null || disNum == null)
            {
                depAd.PlaceholderColor = Color.PaleVioletRed;
                arrAd.PlaceholderColor = Color.PaleVioletRed;
                depVil.PlaceholderColor = Color.PaleVioletRed;
                arrVil.PlaceholderColor = Color.PaleVioletRed;
                pasNum.PlaceholderColor = Color.PaleVioletRed;
                disNum.PlaceholderColor = Color.PaleVioletRed;
                await DisplayAlert("Erreur", "Veuillez remplir tous les champs.", "OK");
                return;
            }
            //Second check : the cities must exist
            if(!DatabaseInteraction.DoesCityExist(depVil.Text) || !DatabaseInteraction.DoesCityExist(arrVil.Text))
            {
                await DisplayAlert("Erreur", "Une des villes entrées n'existe pas.", "OK");
                return;
            }
            
            int passengers = 0;
            int km = 0;
            try
            {
                passengers = int.Parse(pasNum.Text);
                km = int.Parse(disNum.Text);
            }
            catch
            {
                await DisplayAlert("Erreur", "Les champs \"Passagers\" et/ou \"KM\" sont incorrects.", "OK");
                return;
            }
 
            Journey j = new Journey(0, depAd.Text, depVil.Text.ToUpper(), arrAd.Text, arrVil.Text.ToUpper(), DepartureDate.Date.ToString("yyyy-MM-dd") +" "+ DepartureTime.Time.ToString(), ArrDate.Date.ToString("yyyy-MM-dd") +" "+ ArrTime.Time.ToString(), km, passengers, comm.Text, dog.BackgroundColor == Color.Green, smoke.BackgroundColor == Color.Green, music.BackgroundColor == Color.Green, talk.BackgroundColor == Color.Green);
            
            try
            {   //Third check : the user must be an existing one on the database
                if (DatabaseInteraction.CheckIfClientExist(InfoExchanger.Email))
                {
                    DatabaseInteraction.ProposeNewJourney(InfoExchanger.Email, j);
                    await DisplayAlert("Trajet ajouté", "Votre trajet a bien été pris en compte", "OK");
                    depAd.Text = ""; arrAd.Text = ""; DepartureTime.Time = DateTime.Now.TimeOfDay; pasNum.Text = ""; comm.Text = ""; depVil.Text = ""; arrVil.Text = ""; disNum.Text = "";
                    dog.BackgroundColor = Color.Green; smoke.BackgroundColor = Color.Green; music.BackgroundColor = Color.Green; talk.BackgroundColor = Color.Green;
                }
                else { await DisplayAlert("Erreur de connexion", "Il semblerait que vous n'êtes pas connecté à un compte existant. Vérifiez et réessayez.", "OK"); }
            }   //If something goes wrong with the databse, we display an alert popup to the user 
            catch(Exception ex) {DatabaseInteraction.ConnectionCloser(); await DisplayAlert("Erreur de traitement", "Une erreur est survenue pendant le traitement de votre requête. Vérifiez que vous soyez connecté.", "OK"); }

        }


        //This methods handles the click on the burger icon image, showing the navigation tab
        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (!Shell.Current.FlyoutIsPresented)
            {
                Shell.Current.FlyoutIsPresented = true;
            }
        }

        private void On_Return_Button_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new AppShell();
        }

        //This method allows to change the background color of an imageButton on click, changing between red and green.
        private void ChangeColor(object sender, EventArgs e)
        {
            ImageButton se = (ImageButton)sender;

            //switch casing doesnt work as the color static values arent const            
            if(se.BackgroundColor == Color.Red)
            {
                se.BackgroundColor = Color.Green;
            } else
                se.BackgroundColor = Color.Red;
            }

        //This method is there to avoid time-travel journeys, by forcing the arrival date after the departure date.
        private void DateChange(object sender, EventArgs e)
        {
            if(ArrDate.Date < DepartureDate.Date)
                ArrDate.Date = DepartureDate.Date;
            ArrDate.MinimumDate = DepartureDate.Date;
        }
    }


}