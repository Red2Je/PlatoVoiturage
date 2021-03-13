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
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        private void SendForgotenPassword(object sender, EventArgs e)
        {
            //TODO : un mail avec le mot de passe
        }
        private async void GoToInscription(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new InscriptionPage());
        }

        private async void GoBack(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopAsync();   
        }
    }


    
}