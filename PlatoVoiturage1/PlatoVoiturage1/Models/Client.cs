using System;
using System.Collections.Generic;
using System.Text;

namespace PlatoVoiturage1.Models
{
    class Client
    {
        private string email { get; set; }
        private string nom { get; set; }
        private string prenom { get; set; }
        private string telephone { get; set; }

        public Client(string email, string nom, string prenom, string telephone)
        {
            this.email = email;
            this.nom = nom;
            this.prenom = prenom;
            this.telephone = telephone;
        }

    }
}
