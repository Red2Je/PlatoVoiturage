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
    public partial class InscriptionPage : ContentPage
    {
        public InscriptionPage()
        {
            InitializeComponent();
        }
        private async void GoBack(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopAsync();
        }

        private async void ConfirmInscription(object sender, EventArgs e)
        {
            bool isInscriptionAllowed = false;
            // /!\/!\/!\/!\/!\/!\ BIEN VERIFIER QUE TOUS LES CHAMPS TEXTES NE SONT PAS NULL SOUS PEINE DE NULLPOINTEREXCEPTION /!\/!\/!\    
            if (Password.Text == null || PasswordConfirmation.Text == null)
            {
                FailedPasswordText.Text = "Veuillez renseigner tous les champs";
                FailedPasswordText.TextColor = Color.Red;
            }
            else
            {
                if (Password.Text != PasswordConfirmation.Text)
                {
                    FailedPasswordText.Text = "Les deux mots de passes que vous avez entrés ne se correspondent pas, veuillez réessayer";
                    FailedPasswordText.TextColor = Color.Red;
                }
                if (Password.Text.Length == 0)
                {
                    FailedPasswordText.Text = "Votre mot de passe a une longueur de 0, veuillez rentrer un mot de passe plus long";
                }
            }

            

            //TODO avec la base de données : 
            /*
             * Vérifier que l'adresse mail n'est pas déja dans la base
             * Vérifier que le Téléphone n'est pas déja dans la base
             * 
             */


            if (isInscriptionAllowed)
            {
                await Shell.Current.Navigation.PopAsync(true);
                await Shell.Current.Navigation.PopAsync(true);
            }
        }
    }
}