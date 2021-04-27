using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PlatoVoiturage1.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlatoVoiturage1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JourneySearchPage : ContentPage
    {
        public IList<Journey> Result { get; set; }
        public JourneySearchPage()
        {
            InitializeComponent();
            BindingContext = this;
            DepartureTime.Time = DateTime.Now.TimeOfDay;
            ArrivalTime.Time = DateTime.Now.TimeOfDay.Add(new TimeSpan(1,0,0));
            DepartureDay.Date = DateTime.Now.Date;
            ArrivalDay.Date = DateTime.Now.Date;
        }

        private async void GiveDetails(object sender, EventArgs e)
        {
            ImageButton s = (ImageButton)sender;

            await Shell.Current.Navigation.PushAsync(new JourneyDetail((Journey)s.BindingContext, this));
        }

        private void Validate(object sender, EventArgs e)
        {
            bool canValidate = true;
            if(AddDep == null || AddArr == null || DepartureTime == null || ArrivalTime == null || DepartureDay == null || ArrivalDay == null)
            {
                Error.Text = "Veuillez renseigner tous les champs";
                canValidate = false;
            }
            else
            {
                if (!DatabaseInteraction.doesCityExist(AddDep.Text.ToUpper()))
                {
                    Error.Text = "La ville de " + AddDep.Text + " n'existe pas dans notre base";
                    canValidate = false;
                }
                if (!DatabaseInteraction.doesCityExist(AddArr.Text.ToUpper()))
                {
                    Error.Text = "La ville de " + AddArr.Text + " n'existe pas dans notre base";
                    canValidate = false;
                }
                if(DepartureTime.Time.Hours > ArrivalTime.Time.Hours)
                {
                    Error.Text = "Veuillez renseigner une heure d'arrivée après celle de départ";
                    canValidate = false;
                }
                if(DepartureTime.Time.Hours == ArrivalTime.Time.Hours && DepartureTime.Time.Minutes > ArrivalTime.Time.Minutes)
                {
                    Error.Text = "Veuillez renseigner une heure d'arrivée après celle de départ";
                    canValidate = false;
                }

                if (canValidate)
                {
                    Error.Text = "";
                    TimeSpan departureTime = DepartureTime.Time;
                    TimeSpan arrivalTime = ArrivalTime.Time;
                    DateTime departureDate = DepartureDay.Date;
                    DateTime arrivalDate = ArrivalDay.Date;
                    string depTime = departureDate.Year + "-" + departureDate.Month + "-" + departureDate.Day + " " + departureTime.Hours + ":" + departureTime.Minutes + ":" + departureTime.Seconds;
                    string arrTime = arrivalDate.Year + "-" + arrivalDate.Month + "-" + arrivalDate.Day + " " + arrivalTime.Hours + ":" + arrivalTime.Minutes + ":" + arrivalTime.Seconds;
                    Result = DatabaseInteraction.SearchJourney(AddDep.Text.ToUpper(), AddArr.Text.ToUpper(), depTime, arrTime);
                    ResearchDisplay.ItemsSource = Result;
                }
            }

        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (!Shell.Current.FlyoutIsPresented)
            {
                Shell.Current.FlyoutIsPresented = true;
            }
        }
    }
}