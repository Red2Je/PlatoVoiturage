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
    //This page allows an user to sign in the database
    public partial class InscriptionPage : ContentPage
    {
        public InscriptionPage()
        {
            InitializeComponent();
        }
        //This method allows the user to go back to the last page on the navigation stack (here the login page) by pushing the button on top left
        private async void GoBack(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopAsync();
        }

        //This method is called when the "s'inscrire" button is pushed.
        //The first thing one must ddo when handling entries is checking if neither's text attribute is null. Otherwise on any acces, a nullpointerexception will be raised
        //Once this verification is done, we check if what the user entered is valid
        private async void ConfirmInscription(object sender, EventArgs e)
        {
            bool isInscriptionAllowed = true;
            // /!\/!\/!\/!\/!\/!\ BIEN VERIFIER QUE TOUS LES CHAMPS TEXTES NE SONT PAS NULL SOUS PEINE DE NULLPOINTEREXCEPTION /!\/!\/!\    
            if (mail.Text == null || name.Text == null || surname.Text == null || telephone.Text == null || Password.Text == null || PasswordConfirmation.Text == null)
            {
                FailedPasswordText.Text = "Veuillez renseigner tous les champs";
                FailedPasswordText.TextColor = Color.Red;
                isInscriptionAllowed = false;
            }
            else
            {
                //The password must be the same as the password confirmation
                if (Password.Text != PasswordConfirmation.Text)
                {
                    FailedPasswordText.Text = "Les deux mots de passes que vous avez entrés ne se correspondent pas, veuillez réessayer";
                    FailedPasswordText.TextColor = Color.Red;
                    isInscriptionAllowed = false;
                }
                //The password's length musn't be 0
                if (Password.Text.Length == 0)
                {
                    FailedPasswordText.Text = "Votre mot de passe a une longueur de 0, veuillez rentrer un mot de passe plus long";
                    FailedPasswordText.TextColor = Color.Red;
                    isInscriptionAllowed = false;
                }
                //The email must contain an @
                if (!(mail.Text.Contains("@")))
                {
                    FailedPasswordText.Text = "Votre adresse email a un format invalide. (il manque un @)";
                    FailedPasswordText.TextColor = Color.Red;
                    isInscriptionAllowed = false;
                }
                //The client's email musn't already exist in the database
                if (DatabaseInteraction.CheckIfClientExist(mail.Text))
                {
                    FailedPasswordText.Text = "Votre adresse mail existe déjà dans la base, veuillez en saisir une nouvelle";
                    FailedPasswordText.TextColor = Color.Red;
                    isInscriptionAllowed = false;
                }
                //The phone number must be 10 digit long
                if (telephone.Text.Length != 10)
                {
                    FailedPasswordText.Text = "Votre numéro de téléphone ne fait pas 10 chiffres, veuillez réessayer";
                    FailedPasswordText.TextColor = Color.Red;
                    isInscriptionAllowed = false;
                }
                //The phone number musn't already exist in the database
                if (DatabaseInteraction.CheckIfPhoneExists(telephone.Text))
                {
                    FailedPasswordText.Text = "Votre numéro de téléphone existe déjà dans la base, veuillez réessayer";
                    FailedPasswordText.TextColor = Color.Red;
                    isInscriptionAllowed = false;
                }
            }

            

            //If the inscription is allowed, we add the new user to the database, and return on the homepage. The user will then have to login
            if (isInscriptionAllowed)
            {
                DatabaseInteraction.AddNewUser(mail.Text, name.Text, surname.Text, telephone.Text, Password.Text);
                await Shell.Current.Navigation.PopAsync(true);
                await Shell.Current.Navigation.PopAsync(true);
            }
        }
    }
}