using System;
using System.Collections.Generic;
using System.Text;

namespace PlatoVoiturage1.Models
{
    public class Journey
    {


        public int Eid { get; set; }
        public string AdressDep { get; set; }
        public string AdresseArr { get; set; }
        public string Hdep { get; set; }
        public string Harr { get; set; }
        public int Km { get; set; }
        public int NbPlaces { get; set; }

        public string TransformedJourney { get; set; }

        public Journey(int eid, string adressDep, string adresseArr, string hdep, string harr, int km, int nbPlaces)
        {
            this.Eid = eid;
            this.AdressDep = adressDep;
            this.AdresseArr = adresseArr;
            Hdep = hdep;
            Harr = harr;
            this.Km = km;
            this.NbPlaces = nbPlaces;
            this.TransformedJourney = adressDep + "\n -> \n" + adresseArr;
        }


        public override string ToString()
        {
            return ("Eid = " + Eid + " adressDep = " + AdressDep + " adressArr = " + AdresseArr + " hdep = " + Hdep + " harr = " + Harr + " km = " + Km + " nbplaces = " + NbPlaces);
        }

    }
}
