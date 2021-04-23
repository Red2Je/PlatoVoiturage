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
    public partial class ProposerTrajet : ContentPage
    {
        

        public ProposerTrajet()
        {
            InitializeComponent();
            DepartureTime.Time = DateTime.Now.TimeOfDay;

        }

        private async void Valider(object sender, EventArgs e)
        {
            int passengers = 0;
            try
            {
                passengers = int.Parse(pasNum.Text);
            }
            catch
            {
                passengers = 0;
            }
            Journey j = new Journey(0, depAd.Text, arrAd.Text, DepartureTime.Time.ToString(), "LESSGO", 69, passengers);

            
            try
            {
                DatabaseInteraction.proposeNewJourney(EmailSender.Email, j);
                await DisplayAlert("Trajet ajouté", "Votre trajet a bien été pris en compte", "OK");
            }
            catch(Exception ex) { await DisplayAlert("Connexion requise!", "yo connecte toi on a dit", "OK"); Console.WriteLine("BRUH" + ex.Message); }
            

        }

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

        private void ChangeColor(object sender, EventArgs e)
        {
            ImageButton se = (ImageButton)sender;

            //TODO changer le modèle pour enregistrer les préférences
            //switch casing doesnt work as the color static values arent const            
            if(se.BackgroundColor == Color.Red)
            {
                se.BackgroundColor = Color.Green;
            } else
                se.BackgroundColor = Color.Red;
            }

 
    }


}