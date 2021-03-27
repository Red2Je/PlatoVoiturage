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

            bool canReset = false;
            // /!\/!\/!\/!\/!\/!\ BIEN VERIFIER QUE TOUS LES CHAMPS TEXTES NE SONT PAS NULL SOUS PEINE DE NULLPOINTEREXCEPTION /!\/!\/!\    
            if (NewPassword.Text == null || NewPasswordConfirmation == null)
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