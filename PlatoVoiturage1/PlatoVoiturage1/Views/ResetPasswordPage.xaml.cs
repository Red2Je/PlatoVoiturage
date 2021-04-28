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
    public partial class ResetPasswordPage : ContentPage
    {
        public ResetPasswordPage()
        {
            InitializeComponent();
        }

        private async void GoBack(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopAsync();
        }


        private async void ConfirmPasswordReset(object sender, EventArgs e)
        {

            string Email = mail.Text;
            string numtel = telephone.Text;

            bool canReset = true;
            // /!\/!\/!\/!\/!\/!\ BIEN VERIFIER QUE TOUS LES CHAMPS TEXTES NE SONT PAS NULL SOUS PEINE DE NULLPOINTEREXCEPTION /!\/!\/!\    
            if (NewPassword.Text == null || NewPasswordConfirmation == null || mail == null || telephone == null)
            {
                IncorrectPasswordText.Text = "Veuillez renseigner tous les champs";
                IncorrectPasswordText.TextColor = Color.Red;
                canReset = false;
            }
            else
            {
                if (NewPassword.Text != NewPasswordConfirmation.Text)
                { 
                    IncorrectPasswordText.Text = "Les deux mots de passes que vous avez entrés ne se correspondent pas, veuillez réessayer";
                    IncorrectPasswordText.TextColor = Color.Red;
                    canReset = false;
                }
                if (NewPassword.Text.Length == 0)
                {
                    IncorrectPasswordText.Text = "Votre mot de passe a une longueur de 0, veuillez rentrer un mot de passe plus long";
                    IncorrectPasswordText.TextColor = Color.Red;
                    canReset = false;
                }

                if (!(mail.Text.Contains("@")))
                {
                    IncorrectPasswordText.Text = "Votre adresse email a un format invalide. (il manque un @)";
                    IncorrectPasswordText.TextColor = Color.Red;
                    canReset = false;
                }

                if (telephone.Text.Length != 10)
                {
                    IncorrectPasswordText.Text = "Votre numéro de téléphone ne fait pas 10 chiffres, veuillez réessayer";
                    IncorrectPasswordText.TextColor = Color.Red;
                    canReset = false;
                }

                if (!(DatabaseInteraction.CheckEmailAndPhone(Email, numtel)))
                {
                    IncorrectPasswordText.Text = "Aucun lien entre l'adresse mail et le mot de passe, veuillez réessayer";
                    IncorrectPasswordText.TextColor = Color.Red;
                    canReset = false;
                }

            }
            if (canReset)
            {
                DatabaseInteraction.ChangePassword(Email, NewPassword.Text);
                await DisplayAlert("Réinitialisation du mot de passe", "Votre mot de passe a bien été changé", "OK");
                await Shell.Current.Navigation.PopAsync();
            }
            
        }
    }
}