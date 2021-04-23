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
            //TODO :
            //Vérifier que les informations, à part le mot de passe, sont bien coéhrentes
            //Changer le mot de passe dans la base de donnée

            string Email = mail.Text;
            string numtel = telephone.Text;

            bool canReset = false;
            // /!\/!\/!\/!\/!\/!\ BIEN VERIFIER QUE TOUS LES CHAMPS TEXTES NE SONT PAS NULL SOUS PEINE DE NULLPOINTEREXCEPTION /!\/!\/!\    
            if (NewPassword.Text == null || NewPasswordConfirmation == null || mail == null || telephone == null)
            {
                IncorrectPasswordText.Text = "Veuillez renseigner tous les champs";
                IncorrectPasswordText.TextColor = Color.Red;
            }
            else
            {
                if (NewPassword.Text != NewPasswordConfirmation.Text)
                { 
                    IncorrectPasswordText.Text = "Les deux mots de passes que vous avez entrés ne se correspondent pas, veuillez réessayer";
                    IncorrectPasswordText.TextColor = Color.Red;
                }
                if (NewPassword.Text.Length == 0)
                {
                    IncorrectPasswordText.Text = "Votre mot de passe a une longueur de 0, veuillez rentrer un mot de passe plus long";
                    IncorrectPasswordText.TextColor = Color.Red;
                }

                if (!(mail.Text.Contains("@")))
                {
                    IncorrectPasswordText.Text = "Votre adresse email a un format invalide. (il manque un @)";
                    IncorrectPasswordText.TextColor = Color.Red;
                    canReset = false;
                }
                if (DatabaseInteraction.checkIfClientExist(mail.Text))
                {
                    IncorrectPasswordText.Text = "Votre adresse mail existe déjà dans la base, veuillez en saisir une nouvelle";
                    IncorrectPasswordText.TextColor = Color.Red;
                    canReset = false;
                }

                if (telephone.Text.Length != 10)
                {
                    IncorrectPasswordText.Text = "Votre numéro de téléphone ne fait pas 10 chiffres, veuillez réessayer";
                    IncorrectPasswordText.TextColor = Color.Red;
                    canReset = false;
                }

                if (DatabaseInteraction.checkIfPhoneExists(telephone.Text))
                {
                    IncorrectPasswordText.Text = "Votre numéro de téléphone existe déjà dans la base, veuillez réessayer";
                    IncorrectPasswordText.TextColor = Color.Red;
                    canReset = false;
                }

                if (!(DatabaseInteraction.checkEmailAndPhone(Email, numtel)))
                {
                    IncorrectPasswordText.Text = "Aucun lien entre l'adresse mail et le mot de passe, veuillez réessayer";
                    IncorrectPasswordText.TextColor = Color.Red;
                    canReset = false;
                }

            }
            //Une fois que la transaction a été faite avec la base de donnée, mettre la variable can reset à true
            if (canReset)
            {
                await DisplayAlert("Réinitialisation du mot de passe", "Votre mot de passe a bien été changé", "OK");
                await Shell.Current.Navigation.PopAsync();
            }
            
        }
    }
}