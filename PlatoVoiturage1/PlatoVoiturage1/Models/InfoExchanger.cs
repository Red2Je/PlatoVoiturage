using System;
using System.Collections.Generic;
using System.Text;
using PlatoVoiturage1.Views;
using Xamarin.Forms;

namespace PlatoVoiturage1.Models
{
    public static class InfoExchanger
    {
        public static string Email { get; set; }
        public static bool IsAuthentified { get; set; }
        public static Client User { get; set; }

    }
}
