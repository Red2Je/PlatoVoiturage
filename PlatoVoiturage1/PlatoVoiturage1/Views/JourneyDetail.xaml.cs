using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PlatoVoiturage1.Models;

namespace PlatoVoiturage1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JourneyDetail : ContentPage
    {
        private Journey journey;
        private DateTime timeDeparture;
        public JourneyDetail(Journey j)
        {
            this.journey = j;
            string departureDate = journey.Hdep;
            string modifiedDepartureDate = "20" + departureDate.Substring(6, 2) + departureDate.Substring(3, 2) + departureDate.Substring(0, 2) + "T" + departureDate.Substring(9, 2) + ":" + departureDate.Substring(11, 2) + ":00Z";
            //this.timeDeparture = DateTime.ParseExact(modifiedDepartureDate, "yyyyMMddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture);
            //Console.WriteLine("Bite : " + timeDeparture.ToString());
            timeDeparture = new DateTime();
            InitializeComponent();

        }



        private async void GoBack(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopAsync();
        }
    }
}