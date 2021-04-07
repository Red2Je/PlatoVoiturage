using System;
using System.Collections.Generic;
using System.Text;

using Npgsql;

namespace PlatoVoiturage1.Models
{
    /*
     * This class will contain all the static method for the database interaction. To create a new method, one must call the checkDataBaseConnection beforeHand
     * One must also remember to close the connection once the transaction is done
     */
    class DatabaseInteraction
    {
        private static NpgsqlConnection connection;


        private static void checkDataBaseConnection()
        {
            if(connection == null)
            {
                connection = new NpgsqlConnection("Host=91.160.176.22;Username=postgres;Password=platovoiturage;DataBase=postgres");
            }
        }

        public static List<Journey> getProposedJourneyList(string userEmail)
        {
            checkDataBaseConnection();
            connection.Open();

            NpgsqlCommand comm = new NpgsqlCommand("SELECT * FROM propose as pr, utilisateur as ut, trajet as tr WHERE ut.email = (@email) AND ut.email = pr.email AND pr.eid = tr.eid;", connection);
            comm.Parameters.AddWithValue("email",userEmail) ;

            NpgsqlDataReader result = comm.ExecuteReader();

            List<Journey> output = new List<Journey>();

            while (result.Read())
            {
                Journey j = new Journey((int)result["eid"], (string)result["adressedep"], (string)result["adressearr"],(string)result["hdep"],(string)result["harr"],(int)result["km"],(int)result["nbplaces"]);
                output.Add(j);
            }
            connection.Close();
            return (output);
        }

        public static List<Journey> getReservedJourneyList(string userEmail)
        {
            checkDataBaseConnection();
            connection.Open();

            NpgsqlCommand comm = new NpgsqlCommand("SELECT * FROM reserve as re, utilisateur as ut, trajet as tr WHERE ut.email = (@email) AND ut.email = re.email AND re.eid = tr.eid;", connection);
            comm.Parameters.AddWithValue("email",userEmail);

            NpgsqlDataReader result = comm.ExecuteReader();

            List<Journey> output = new List<Journey>();

            while (result.Read())
            {
                Journey j = new Journey((int)result["eid"], (string)result["adressedep"], (string)result["adressearr"], (string)result["hdep"], (string)result["harr"], (int)result["km"], (int)result["nbplaces"]);
                output.Add(j);
            }
            connection.Close();
            return (output);
        }

        public static void proposeNewJourney(string userEmail, Journey j)
        {
            checkDataBaseConnection();
            connection.Open();

            NpgsqlCommand comm = new NpgsqlCommand("INSERT INTO trajet VALUES (" + j.Eid + ", '" + j.AdressDep + "', '" + j.AdresseArr + "', '" + j.Hdep + "', '" + j.Harr + "', " + j.Km + ", " + j.NbPlaces + ");", connection);

            NpgsqlCommand comm2 = new NpgsqlCommand("INSERT INTO propose VALUES ('" + userEmail + "', " + j.Eid +");", connection);
        }
    }
}
