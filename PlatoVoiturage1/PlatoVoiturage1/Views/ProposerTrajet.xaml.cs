﻿using PlatoVoiturage1.Models;
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
            TimeSpan initTime = new TimeSpan(DateTime.Now.TimeOfDay.Hours, DateTime.Now.TimeOfDay.Minutes, 0);
            DepartureTime.Time = initTime;
            DepartureDate.MinimumDate = DateTime.Now.Date;
            ArrTime.Time = initTime;
            ArrDate.MinimumDate = DepartureDate.Date;
            Console.WriteLine(DepartureTime.Time);
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

            int km = 0;
            try
            {
                km = int.Parse(disNum.Text);
            }
            catch
            {
                km = 0;
            }
 
            Journey j = new Journey(0, depAd.Text, depVil.Text, arrAd.Text, arrVil.Text, DepartureDate.Date.ToString("yyyy-MM-dd") +" "+ DepartureTime.Time.ToString(), ArrDate.Date.ToString("yyyy-MM-dd") +" "+ ArrTime.Time.ToString(), km, passengers, comm.Text, dog.BackgroundColor == Color.Green, smoke.BackgroundColor == Color.Green, music.BackgroundColor == Color.Green, talk.BackgroundColor == Color.Green);
            
            
            try
            {
                if (DatabaseInteraction.CheckIfClientExist(InfoExchanger.Email))
                {
                    DatabaseInteraction.proposeNewJourney(InfoExchanger.Email, j);
                    await DisplayAlert("Trajet ajouté", "Votre trajet a bien été pris en compte", "OK");
                    depAd.Text = ""; arrAd.Text = ""; DepartureTime.Time = DateTime.Now.TimeOfDay; pasNum.Text = ""; comm.Text = ""; depVil.Text = ""; arrVil.Text = ""; disNum.Text = "";
                    dog.BackgroundColor = Color.Green; smoke.BackgroundColor = Color.Green; music.BackgroundColor = Color.Green; talk.BackgroundColor = Color.Green;
                }
                else { await DisplayAlert("Erreur de connexion", "Il semblerait que vous n'êtes pas connecté à votre compte. Vérifiez et réessayez.", "OK"); }
            }
            catch(Exception ex) {Console.WriteLine("OUPS" + ex.Message); DatabaseInteraction.ConnectionCloser(); await DisplayAlert("Erreur de traitement", "Une erreur est survenue pendant le traitement de votre requête. Vérifiez que vous soyez connecté.", "OK"); }
            

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

        private void DateChange(object sender, EventArgs e)
        {
            if(ArrDate.Date < DepartureDate.Date)
                ArrDate.Date = DepartureDate.Date;
            ArrDate.MinimumDate = DepartureDate.Date;
        }
    }


}