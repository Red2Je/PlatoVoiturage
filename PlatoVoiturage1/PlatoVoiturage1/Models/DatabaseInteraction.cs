using System;
using System.Collections.Generic;
using System.Text;

using Npgsql;

namespace PlatoVoiturage1.Models
{
    /*
     * This class will contain all the static method for the database interaction. To create a new method, one must call the CheckDataBaseConnection beforeHand
     * One must also remember to close the connection once the transaction is done
     */
    class DatabaseInteraction
    {
        private static NpgsqlConnection connection;


        private static void CheckDataBaseConnection()
        {
            if(connection == null)
            {
                connection = new NpgsqlConnection("Host=91.166.143.227;Port=32769;Username=postgres;Password=platovoiturage;DataBase=postgres");
            }
        }

        public static List<Journey> GetProposedJourneyList(string userEmail)
        {
            CheckDataBaseConnection();
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

        public static List<Journey> GetReservedJourneyList(string userEmail)
        {
            CheckDataBaseConnection();
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
            CheckDataBaseConnection();
            connection.Open();

            NpgsqlCommand pullData = new NpgsqlCommand("SELECT MAX(eid) FROM trajet;", connection);
            NpgsqlDataReader result =  pullData.ExecuteReader();
            if (result.HasRows)
            {
                result.Read();
                j.Eid = result.GetInt32(0) + 1;
            }else { j.Eid = 1; }
            connection.Close();
            
            CheckDataBaseConnection();
            connection.Open();
            NpgsqlCommand comm = new NpgsqlCommand("INSERT INTO trajet VALUES (" + j.Eid + ", '" + j.AdressDep + "', '" + j.AdresseArr + "', '" + j.Hdep + "', '" + j.Harr + "', " + j.Km + ", " + j.NbPlaces + ");", connection);
            comm.ExecuteReader();
            connection.Close();
            CheckDataBaseConnection();
            connection.Open();
            NpgsqlCommand comm2 = new NpgsqlCommand("INSERT INTO propose VALUES ('" + userEmail + "', " + j.Eid + ");", connection);
            comm2.ExecuteReader();
            connection.Close();
            
        }



        public static string login(string Email, string Password)
        {
            CheckDataBaseConnection();
            connection.Open();
            NpgsqlCommand comm = new NpgsqlCommand("SELECT loggin((@email),(@password))",connection);
            comm.Parameters.AddWithValue("email", Email);
            comm.Parameters.AddWithValue("password", Password);
            NpgsqlDataReader result = comm.ExecuteReader();
            List<string> r = new List<string>();

            while (result.Read())
            {
                r.Add(result.GetString(0));
            }
            connection.Close();
            if(r.Count != 1)
            {
                throw new FormatException("Plus d'un résultat a été récupéré pour la commande login");
            }
            else
            {
                return (r[0]);
            }
        }



        public static Client getClient(string Email)
        {
            CheckDataBaseConnection();
            connection.Open();
            NpgsqlCommand comm = new NpgsqlCommand("SELECT nom, prenom, numtel FROM utilisateur WHERE email=(@email) ",connection);
            comm.Parameters.AddWithValue("email", Email);
            NpgsqlDataReader result = comm.ExecuteReader();
            Client output = null;
            if (result.HasRows)
            {
                result.Read();
                output = new Client(Email, (string)result["nom"], (string)result["prenom"], (string)result["numtel"]);
            }
            connection.Close();
            return output;
        }

        public static bool checkIfClientExist(string Email)
        {
            bool exists;
            CheckDataBaseConnection();
            connection.Open();
            NpgsqlCommand comm = new NpgsqlCommand("SELECT * FROM utilisateur WHERE email=(@email)",connection);
            comm.Parameters.AddWithValue("email",Email);
            NpgsqlDataReader result = comm.ExecuteReader();
            if (result.HasRows)
            {
                exists = true;
            }
            else
            {
                exists = false;
            }
            connection.Close();
            return (exists);
        }

        public static bool checkIfPhoneExists(string numtel)
        {
            bool exists;
            CheckDataBaseConnection();
            connection.Open();
            NpgsqlCommand comm = new NpgsqlCommand("SELECT * FROM utilisateur WHERE numtel=(@numtel)", connection);
            comm.Parameters.AddWithValue("numtel", numtel);
            NpgsqlDataReader result = comm.ExecuteReader();
            if (result.HasRows)
            {
                exists = true;
            }
            else
            {
                exists = false;
            }
            connection.Close();
            return (exists);
        }

        public static void addNewUser(string Email, string nom, string prenom, string numtel, string password)
        {
            CheckDataBaseConnection();
            connection.Open();
            NpgsqlCommand comm = new NpgsqlCommand("CALL insert_data((@email),(@password),(@surname),(@name),(@numtel))", connection);
            comm.Parameters.AddWithValue("email", Email);
            comm.Parameters.AddWithValue("password", password);
            comm.Parameters.AddWithValue("surname", prenom);
            comm.Parameters.AddWithValue("name", nom);
            comm.Parameters.AddWithValue("numtel", numtel);
            comm.ExecuteReader();
            connection.Close();
        }
    }
}
