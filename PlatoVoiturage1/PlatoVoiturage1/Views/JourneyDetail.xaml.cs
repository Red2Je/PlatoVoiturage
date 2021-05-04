using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Globalization;

using PlatoVoiturage1.Models;

namespace PlatoVoiturage1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    //A journey detail page is used to display to the user some information about it's incoming journey
    public partial class JourneyDetail : ContentPage
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

        public string Numtel { get; set; }


        private DateTime TimeDeparture { get; set; }
        private DateTime TimeArrival { get; set; }
        private Journey j;
        //This attribute is used to modify the bindding context, allowing us to use the named items on the page
        private ContentPage HomePage;


        public JourneyDetail(Journey j, ContentPage homePage)
        {
            BindingContext = this;
            this.j = j;
            AddressDep = "Adresse de départ : " + j.AdressDep;
            AddressArr ="Adresse d'arrivée : " + j.AdresseArr;
            Km = "Détour toléré (km) : " + j.Km.ToString();
            Nbplaces = "Nombre de places restantes : " + j.NbPlaces.ToString();
            VilleDep = "Ville de départ : " + j.VilleDep;
            VilleArr = "Ville d'arrivée : " + j.VilleArr;
            Comm = "Commentaire : \n" + j.Comm;
            this.TimeDeparture = DateTime.ParseExact(j.Hdep, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            Hdep = "Heure de départ : " + TimeDeparture.ToString();
            this.TimeArrival = DateTime.ParseExact(j.Harr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            Harr = "Heure d'arrivée : " + TimeArrival.ToString();
            Numtel = "Numéro de téléphone du conducteur " + DatabaseInteraction.getPhoneNumber(j.Eid);
            //The components are initialized here to make sure that the data are already in there attributes before the components such as the labels load.
            InitializeComponent();
            //From here we are setting a component's attribute so the component have to be initialized
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



            this.HomePage = homePage;
            


        }


        //This method allows a user to open google map to be open on its device, with the journey schedulded
        private async void GoToMaps(object sender, EventArgs e)
        {
            string mapString = "http://maps.google.com/?daddr="+ j.AdresseArr + " " + j.VilleArr + "&saddr=" + j.AdressDep + " " + j.VilleDep;
            if(Device.RuntimePlatform == Device.Android)
            {
                await Launcher.OpenAsync(mapString);
            }
        }

        //Same as the InscriptionPage GoBack method
        private async void GoBack(object sender, EventArgs e)
        {
            BindingContext = HomePage;
            await Shell.Current.Navigation.PopAsync();
        }
    }
}