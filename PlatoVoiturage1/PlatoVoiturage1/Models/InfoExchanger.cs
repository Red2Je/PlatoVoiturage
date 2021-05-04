using System;
using System.Collections.Generic;
using System.Text;
using PlatoVoiturage1.Views;
using Xamarin.Forms;

namespace PlatoVoiturage1.Models
{
    //This static class is used to transmit information between all the pages of our app.
    //The usual way of doing this is to give attributes to the pages and give these informations with a constructor.
    //However, we haven't found a solution to make the xaml use a constructor with an user-created object , hence this workaround
    public static class InfoExchanger
    {
        public static string Email { get; set; }
        public static bool IsAuthentified { get; set; }
        public static Client User { get; set; }

    }
}
