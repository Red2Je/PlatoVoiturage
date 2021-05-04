using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Mail;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PlatoVoiturage1.Models;

namespace PlatoVoiturage1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    //The login page allows an user to enter its credentials and to log in the app
    public partial class LoginPage : ContentPage
    {
        public string Email{get;set;}
        public string Password { private get; set; }

        private ContentPage HomePage;

        public LoginPage(ContentPage HomePage)
        {
            InitializeComponent();
            this.HomePage = HomePage;

        }

        //This method is called whent the conenct button is pressed. Once more all oof the entry are checked not to be null
        private async void Connect(object sender, EventArgs e)
        {
            if(Em.Text==null || Pw.Text==null)
            {
                LoginError.Text = "L'adresse ou le mot de passe est vide";
                LoginError.TextColor = Color.Red;
                return;
            }
            Email = Em.Text;
            Password = Pw.Text;
            string result = DatabaseInteraction.Login(Email, Password);
            //Depending on the result of the request, we either log the user and take it back to the homepage, or we display an error
            if(result == "logged in")
            { 
                Client logged = DatabaseInteraction.GetClient(Email);
                //Removing the first homepage to replace it with the one with a client logged in
                Shell.Current.Navigation.RemovePage(HomePage);
                //Adding a page before this one in the navigation stack, logged in.
                InfoExchanger.Email = Email;
                InfoExchanger.IsAuthentified = true;
                InfoExchanger.User = logged;
                //Popping the navigation stack to the new page with the user logged in
                await Shell.Current.Navigation.PopToRootAsync();
            }
            else if(result == "invalid password")
            {
                LoginError.Text = "Mot de passe incorrect";
                LoginError.TextColor = Color.Red;
            }
            else if(result == "unknown user")
            {
                LoginError.Text = "Utilisateur inconnu";
                LoginError.TextColor = Color.Red;
            }
            else
            {
                LoginError.Text = "Une erreur s'est produite, veuillez réessayer";
                LoginError.TextColor = Color.Red;
            }
        }


        //This method is used to send the user to the forgotten password page
        private async void SendForgotenPassword(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new ResetPasswordPage());
        }
        //This method is used to send the user to the inscription page
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