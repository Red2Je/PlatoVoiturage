using System;
using System.Collections.Generic;
using System.Text;

namespace PlatoVoiturage1.Models
{
    public class Client
    {
        public string Email { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Telephone { get; set; }

        public Client(string email, string nom, string prenom, string telephone)
        {
            this.Email = email;
            this.Nom = nom;
            this.Prenom = prenom;
            this.Telephone = telephone;
        }

    }
}
