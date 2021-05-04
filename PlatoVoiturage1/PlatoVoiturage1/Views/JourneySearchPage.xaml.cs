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
    //The journey search page allows an user to look for a journey with the departure city, the departure time, the arrival city and the arrival time
    public partial class JourneySearchPage : ContentPage
    {
        //This attribute is binded to the view displaying the result of the research
        public IList<Journey> Result { get; set; }
        public JourneySearchPage()
        {
            InitializeComponent();
            BindingContext = this;
            //We add default values to our date pickers and time pickers
            DepartureTime.Time = DateTime.Now.TimeOfDay;
            ArrivalTime.Time = DateTime.Now.TimeOfDay.Add(new TimeSpan(1,0,0));
            DepartureDay.MinimumDate = DateTime.Now.Date;
            ArrivalDay.MinimumDate = DateTime.Now.Date;
        }

        //This method displays a journey reservation page, allowing the user to consult the journey, and possibly make a reservation
        private async void GiveDetails(object sender, EventArgs e)
        {
            ImageButton s = (ImageButton)sender;

            await Shell.Current.Navigation.PushAsync(new JourneyReservation((Journey)s.BindingContext, this));
        }

        //This method is called when the journey is ready to be researched in the database. As for the Inscription page, all entries musnt be null
        private void Validate(object sender, EventArgs e)
        {
            bool canValidate = true;
            if(AddDep.Text == null || AddArr.Text == null || DepartureTime == null || ArrivalTime == null || DepartureDay == null || ArrivalDay == null)
            {
                Error.Text = "Veuillez renseigner tous les champs";
                canValidate = false;
            }
            else
            {
                //We check if both departure and arrival cities exists in the database
                if (!DatabaseInteraction.DoesCityExist(AddDep.Text.ToUpper()))
                {
                    Error.Text = "La ville de " + AddDep.Text + " n'existe pas dans notre base";
                    canValidate = false;
                }
                if (!DatabaseInteraction.DoesCityExist(AddArr.Text.ToUpper()))
                {
                    Error.Text = "La ville de " + AddArr.Text + " n'existe pas dans notre base";
                    canValidate = false;
                }
                //We check that the hour of arrival is later than the the hour of departure
                if(DepartureTime.Time.Hours >= ArrivalTime.Time.Hours)
                {
                    Error.Text = "Veuillez renseigner une heure d'arrivée après celle de départ";
                    canValidate = false;
                }
                //If the hour of arrival is the same as the our of departure, we check that the minutes given are different
                if(DepartureTime.Time.Hours == ArrivalTime.Time.Hours && DepartureTime.Time.Minutes > ArrivalTime.Time.Minutes)
                {
                    Error.Text = "Veuillez renseigner une heure d'arrivée après celle de départ";
                    canValidate = false;
                }
                //If all the tests passed, the journey can be reserved and the interaction with the database can be done
                if (canValidate)
                {
                    Error.Text = "";
                    //From here all the given dates are transformed to match the database format, ie. "yyyy-MM-dd HH:mm:ss"
                    TimeSpan departureTime = DepartureTime.Time;
                    TimeSpan arrivalTime = ArrivalTime.Time;
                    DateTime departureDate = DepartureDay.Date;
                    DateTime arrivalDate = ArrivalDay.Date;
                    //string depTime = departureDate.Year + "-" + departureDate.Month.ToString("dd") + "-" + departureDate.Day.ToString("dd") + " " + departureTime.Hours.ToString("dd") + ":" + departureTime.Minutes.ToString("dd") + ":" + departureTime.Seconds.ToString("dd");
                    string depDate = departureDate.ToString("yyyy-MM-dd");
                    string depTime = departureTime.ToString();
                    string finalDep = depDate + " " + depTime;

                    //string arrTime = arrivalDate.Year + "-" + arrivalDate.Month.ToString("dd") + "-" + arrivalDate.Day.ToString("dd") + " " + arrivalTime.Hours.ToString("dd") + ":" + arrivalTime.Minutes.ToString("dd") + ":" + arrivalTime.Seconds.ToString("dd");
                    string arrDate = arrivalDate.ToString("yyyy-MM-dd");
                    string arrTime = arrivalTime.ToString();
                    string finalArr = arrDate + " " + arrTime;
                    Result = DatabaseInteraction.SearchJourney(AddDep.Text.ToUpper(), AddArr.Text.ToUpper(), finalDep, finalArr);
                    ResearchDisplay.ItemsSource = Result;
                }
            }

        }

        //see homepage
        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (!Shell.Current.FlyoutIsPresented)
            {
                Shell.Current.FlyoutIsPresented = true;
            }
        }
    }
}