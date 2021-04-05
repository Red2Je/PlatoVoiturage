using System;
using System.Collections.Generic;
using System.Text;

namespace PlatoVoiturage1.Models
{
    class Journey
    {


        public int eid { get; set; }
        public string adressDep { get; set; }
        public string adresseArr { get; set; }
        public string Hdep { get; set; }
        public string Harr { get; set; }
        public int km { get; set; }
        public int nbPlaces { get; set; }

        public Journey(int eid, string adressDep, string adresseArr, string hdep, string harr, int km, int nbPlaces)
        {
            this.eid = eid;
            this.adressDep = adressDep;
            this.adresseArr = adresseArr;
            Hdep = hdep;
            Harr = harr;
            this.km = km;
            this.nbPlaces = nbPlaces;
        }


    }
}
