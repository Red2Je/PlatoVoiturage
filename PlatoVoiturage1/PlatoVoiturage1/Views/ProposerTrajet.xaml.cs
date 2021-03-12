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
    public partial class ProposerTrajet : ContentPage
    {
        public ProposerTrajet()
        {
            InitializeComponent();
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (!Shell.Current.FlyoutIsPresented)
            {
                Shell.Current.FlyoutIsPresented = true;
            }
        }

        private void On_Return_Button_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new AppShell();
        }

        private void ChangeColor(object sender, EventArgs e)
        {
            ImageButton se = (ImageButton)sender;

            //TODO changer le modèle pour enregistrer les préférences
            //switch casing doesnt work as the color static values arent const            
            if(se.BackgroundColor == Color.White)
            {
                se.BackgroundColor = Color.Green;
            } else
                if(se.BackgroundColor == Color.Green)
                {
                    se.BackgroundColor = Color.Red;
                }
                else
                {
                    se.BackgroundColor = Color.White;
                }
        }

 
    }


}