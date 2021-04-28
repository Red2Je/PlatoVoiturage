using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Npgsql;
using System.Globalization;

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
                connection = new NpgsqlConnection("Host=92.170.106.48;Port=5432;Username=postgres;Password=toto;DataBase=postgres;Pooling=false;Timeout=300;CommandTimeout=300");
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
            NpgsqlCommand comm = new NpgsqlCommand("INSERT INTO trajet VALUES (" + j.Eid + ", '" + j.AdressDep + "', '" + j.AdresseArr + "', '" + j.Hdep + "', '" + j.Harr + "', " + j.Km + ", " + j.Pets + ", " + j.Smoke + ", " + j.Music + ", " + j.Talk + ", '" + j.Comm + "', " + j.NbPlaces + ");", connection);
            comm.ExecuteReader();
            connection.Close();
            CheckDataBaseConnection();
            connection.Open();
            NpgsqlCommand comm2 = new NpgsqlCommand("INSERT INTO propose VALUES ('" + userEmail + "', " + j.Eid + ");", connection);
            comm2.ExecuteReader();
            connection.Close();
            
        }



        public static string Login(string Email, string Password)
        {
            Console.WriteLine("Email : "+Email);
            Console.WriteLine("Password : " + Password);
            try
            {
                CheckDataBaseConnection();
                connection.Open();
                NpgsqlCommand comm = new NpgsqlCommand("SELECT loggin((@email),(@password))", connection);
                comm.Parameters.AddWithValue("email", Email);
                comm.Parameters.AddWithValue("password", Password);
                NpgsqlDataReader result = comm.ExecuteReader();
                List<string> r = new List<string>();

                while (result.Read())
                {
                    r.Add(result.GetString(0));
                }
                connection.Close();
                if (r.Count != 1)
                {
                    throw new FormatException("Plus d'un résultat a été récupéré pour la commande login");
                }
                else
                {
                    return (r[0]);
                }
            }catch(Exception e)
            {
                Console.WriteLine("Nouvelle exception : ");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.GetType().ToString());
                throw e;
            }
            
        }



        public static Client GetClient(string Email)
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

        public static bool CheckIfClientExist(string Email)
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

        public static bool CheckIfPhoneExists(string numtel)
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

        public static void AddNewUser(string Email, string nom, string prenom, string numtel, string password)
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

        public static bool CheckEmailAndPhone(string email, string phoneNumber)
        {
            CheckDataBaseConnection();
            connection.Open();
            NpgsqlCommand comm = new NpgsqlCommand("SELECT * FROM utilisateur WHERE email=(@email) AND numtel=(@numtel);", connection);
            comm.Parameters.AddWithValue("email", email);
            comm.Parameters.AddWithValue("numtel", phoneNumber);
            NpgsqlDataReader result = comm.ExecuteReader();
            bool exists = false ;
            if (result.HasRows)
            {
                exists = true;
            }
            connection.Close();
            return (exists);
        }
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static long ConvertToTimestamp(DateTime value)
        {
            TimeSpan elapsedTime = value - Epoch;
            return (long)elapsedTime.TotalSeconds;
        }

        public static List<Journey> SearchJourney(string startingCity, string targettedCity, string startingDate, string targettedDate)
        {
            CheckDataBaseConnection();
            connection.Open();

            NpgsqlCommand comm = new NpgsqlCommand("SELECT rechercheTrajet((@villeDepe), (@villeArre), (@hdepe), (@harre))", connection);
            comm.Parameters.AddWithValue("%villeDepe%", startingCity);
            comm.Parameters.AddWithValue("%villeArre%", targettedCity);
            comm.Parameters.AddWithValue("hdepe", startingDate);
            comm.Parameters.AddWithValue("harre", targettedDate);
            NpgsqlDataReader result = comm.ExecuteReader();
            Dictionary<Journey, double> journeys = new Dictionary<Journey, double>();

            List<Journey> temp = new List<Journey>();
            while (result.Read())
            {
                temp.Add(new Journey((int)result["eid"], (string)result["adresseDep"], (string)result["adresseArr"], (string)result["hdep"], (string)result["harr"], (int)result["km"], (int)result["nbPlaces"]));
            }

            connection.Close();

            foreach (Journey j in temp)
            {
                CheckDataBaseConnection();
                connection.Open();
                NpgsqlCommand comm2 = new NpgsqlCommand("SELECT distance((@ville1), (@ville2))", connection);
                comm2.Parameters.AddWithValue("%ville1%", startingCity);
                comm2.Parameters.AddWithValue("%ville2%", j.AdressDep);
                NpgsqlDataReader distanceDep = comm2.ExecuteReader();
                distanceDep.Read();
                int distanceDepe = distanceDep.GetInt32(0);
                connection.Close();

                CheckDataBaseConnection();
                connection.Open();
                NpgsqlCommand comm3 = new NpgsqlCommand("SELECT distance((@ville1), (@ville2))", connection);
                comm3.Parameters.AddWithValue("%ville1%", targettedCity);
                comm3.Parameters.AddWithValue("%ville2%", j.AdresseArr);
                NpgsqlDataReader distanceArr = comm3.ExecuteReader();
                distanceArr.Read();
                int distanceArre = distanceArr.GetInt32(0);
                connection.Close();

                long d1 = ConvertToTimestamp(DateTime.ParseExact(j.Hdep, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                long a1 = ConvertToTimestamp(DateTime.ParseExact(j.Harr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                long d2 = ConvertToTimestamp(DateTime.ParseExact(startingDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                long a2 = ConvertToTimestamp(DateTime.ParseExact(targettedDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));

                journeys[j] = Math.Exp(Math.Abs(d1 - d2) / 180) + Math.Exp(Math.Abs(a1 - a2) / 180) + Math.Exp(distanceDepe) + Math.Exp(distanceArre);

            }

            List<Journey> searchResult = new List<Journey>();
            foreach (KeyValuePair<Journey, double> pair in journeys.OrderBy(key => key.Value))
            {
                searchResult.Add(pair.Key);
            }
            return searchResult;
        }



        public static void ChangePassword(string Email, string password)
        {
            CheckDataBaseConnection();
            connection.Open();
            NpgsqlCommand comm = new NpgsqlCommand("CALL reinit_password((@email),(@password))", connection);
            comm.Parameters.AddWithValue("email", Email);
            comm.Parameters.AddWithValue("password", password);
            comm.ExecuteReader();
            connection.Close();

        }

        public static bool DoesCityExist(string city)
        {
            CheckDataBaseConnection();
            connection.Open();
            city = city.ToUpper();
            NpgsqlCommand comm = new NpgsqlCommand("SELECT * FROM voisin WHERE nom=(@ville);", connection);
            comm.Parameters.AddWithValue("ville", city);
            NpgsqlDataReader result = comm.ExecuteReader();
            bool output = false;
            if (result.HasRows)
            {
                output = true;
            }
            connection.Close();
            return (output);

        }

    }




}
