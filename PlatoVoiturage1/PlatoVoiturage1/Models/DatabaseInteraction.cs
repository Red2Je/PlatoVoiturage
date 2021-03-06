using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Npgsql;
using System.Globalization;

namespace PlatoVoiturage1.Models
{
    /*
     * This class will contain all the static method for the database interaction. To create a new method, one must call the CheckDataBaseConnection before hand
     * One must also remember to close the connection once the transaction is done
     */
    class DatabaseInteraction
    {
        //The static connection. A connection must be used to make only one request, else an exception is thrown telling us that a transaction is already in progress
        private static NpgsqlConnection connection;

        //This method checks if the connection is already opened. It allows us to call any method in any order we want.
        private static void CheckDataBaseConnection()
        {
            if(connection == null)
            {
                connection = new NpgsqlConnection("Host=90.46.1.140;Port=5432;Username=postgres;Password=toto;DataBase=postgres;Pooling=false;Timeout=300;CommandTimeout=300");
            }
        }

        //This method asks the database for the journey that a certain user proposed. This method will also have detailed comments to show how npgsql works.
        //The scheme for working with npgsq is the same in every method
        public static List<Journey> GetProposedJourneyList(string userEmail)
        {
            //We check the data base connection in case this method is the first one to ever be called
            CheckDataBaseConnection();
            //We open the said connection
            connection.Open();
            //We create a new command to send to the database, and we link it with the current connection
            NpgsqlCommand comm = new NpgsqlCommand("SELECT * FROM propose as pr, utilisateur as ut, trajet as tr WHERE ut.email = (@email) AND ut.email = pr.email AND pr.eid = tr.eid;", connection);
            //In the the command, there could be some parameters (here @email) that we have to give. It is done with adding the value of the parameter
            comm.Parameters.AddWithValue("email",userEmail) ;
            //Here the command is sent to the database and the result is stored in an NpgsqlDataReader
            NpgsqlDataReader result = comm.ExecuteReader();

            List<Journey> output = new List<Journey>();
            //Like a file reading, we must load the next line in the read object, then we can process it.
            while (result.Read())
            {
                //The result object wotks similaraly as an object dictionnary.
                Journey j = new Journey((int)result["eid"], (string)result["adressedep"],(string)result["villedep"] ,(string)result["adressearr"],(string)result["villearr"],(string)result["hdep"],(string)result["harr"],(int)result["km"],(int)result["nbplaces"],(string)result["comment"],(bool)result["pets"], (bool)result["smoke"], (bool)result["music"], (bool)result["speak"]);
                output.Add(j);
            }
            //We close the connection. If we forget to do that, on the next connection opening, an exception will be thrown
            connection.Close();
            return (output);
        }
        
        //This method asks the database for the journey that a certain user reserved.
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
                Journey j = new Journey((int)result["eid"], (string)result["adressedep"], (string)result["villedep"], (string)result["adressearr"], (string)result["villearr"], (string)result["hdep"], (string)result["harr"], (int)result["km"], (int)result["nbplaces"], (string)result["comment"], (bool)result["pets"], (bool)result["smoke"], (bool)result["music"], (bool)result["speak"]);
                output.Add(j);
            }
            connection.Close();
            return (output);
        }

        //This method put in the database a new journey, and link it in the table propose with the user proposing it.
        public static void ProposeNewJourney(string userEmail, Journey j)
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
            //Here we can see that the connection is checked and opened multiple times. It is done so to avoid two request being sent at once
            CheckDataBaseConnection();
            connection.Open();
            NpgsqlCommand comm = new NpgsqlCommand("INSERT INTO trajet VALUES ((@eid), (@adDep), (@vilDep), (@adArr), (@vilArr), (@hdep), (@harr), (@km), (@pets), (@smoke), (@music), (@talk), (@comm), (@nbPl));", connection);
            comm.Parameters.AddWithValue("eid", j.Eid); comm.Parameters.AddWithValue("km", j.Km); 
            comm.Parameters.AddWithValue("pets", j.Pets); comm.Parameters.AddWithValue("smoke", j.Smoke); 
            comm.Parameters.AddWithValue("comm", j.Comm); comm.Parameters.AddWithValue("adDep", j.AdressDep); 
            comm.Parameters.AddWithValue("vilDep", j.VilleDep); comm.Parameters.AddWithValue("harr", j.Harr); 
            comm.Parameters.AddWithValue("music", j.Music); comm.Parameters.AddWithValue("nbPl", j.NbPlaces);
            comm.Parameters.AddWithValue("adArr", j.AdresseArr); comm.Parameters.AddWithValue("vilArr", j.VilleArr); 
            comm.Parameters.AddWithValue("hdep", j.Hdep); comm.Parameters.AddWithValue("talk", j.Talk);
            comm.ExecuteReader();
            connection.Close();
            CheckDataBaseConnection();
            connection.Open();
            NpgsqlCommand comm2 = new NpgsqlCommand("INSERT INTO propose VALUES ((@email), (@eid));", connection);
            comm2.Parameters.AddWithValue("email", userEmail); comm2.Parameters.AddWithValue("eid", j.Eid);
            comm2.ExecuteReader();
            connection.Close();
            
        }


        //This method is used to check if a user can log in the app.
        //If the user can log, the return value is "logged in". If the user is unknown, the return value is "unknown user" and if the password is incorrect, the return value is "incorrect password"
        //This method will use the loggin function in the database
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


        //This method is used to get the client's information after logging him in. It returns a client object.
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

        //This method checks if a client exists in the database
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

        //This method checks if a phone number exists in the database
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

        //This methods allows us to add a new entry in the database, for a new user.
        //This method uses the insert_data procedure in the database
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

        //This method checks if it exists an user with both the email and the phone number in the database
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

        //A reference to the original date of computers, used in the next method
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        //This method converts a date in a timespan, representing the time elapsed since the 1970. This value will be used later to rank our journeys
        private static long ConvertToTimestamp(DateTime value)
        {
            TimeSpan elapsedTime = value - Epoch;
            return (long)elapsedTime.TotalSeconds;
        }

        //This method returns a list of journey corresponding to certain criteria.
        //This method uses the function rechercheTrajet in the database
        //The returned journey are ranked using the amount of times it takes to make the journey.
        public static List<Journey> SearchJourney(string startingCity, string targettedCity, string startingDate, string targettedDate)
        {

            CheckDataBaseConnection();
            connection.Open();

            NpgsqlCommand comm = new NpgsqlCommand("SELECT * FROM rechercheTrajet((@villeDepe), (@villeArre), (@hdepe), (@harre))", connection);
            comm.Parameters.AddWithValue("villeDepe", "%"+startingCity+"%");
            comm.Parameters.AddWithValue("villeArre", "%"+targettedCity+"%");
            comm.Parameters.AddWithValue("hdepe", startingDate);
            comm.Parameters.AddWithValue("harre", targettedDate);
            NpgsqlDataReader result = comm.ExecuteReader();
            Dictionary<Journey, double> journeys = new Dictionary<Journey, double>();

            List<Journey> temp = new List<Journey>();
            while (result.Read())
            {
               temp.Add(new Journey((int)result["eid"], (string)result["adresseDep"],(string)result["villeDep"], (string)result["adresseArr"], (string)result["villeArr"],(string)result["HDep"], (string)result["HArr"], (int)result["KM"], (int)result["Nb"],(string)result["comment"],(bool)result["pets"], (bool)result["smoke"], (bool)result["music"], (bool)result["speak"]));
            }
            connection.Close();

            foreach (Journey j in temp)
            {
                CheckDataBaseConnection();
                connection.Open();
                NpgsqlCommand comm2 = new NpgsqlCommand("SELECT * FROM distance((@ville1), (@ville2))", connection);
                comm2.Parameters.AddWithValue("ville1", startingCity);
                comm2.Parameters.AddWithValue("ville2", "%"+j.AdressDep+"%");
                NpgsqlDataReader distanceDep = comm2.ExecuteReader();
                distanceDep.Read();
                int distanceDepe = distanceDep.GetInt32(0);
                connection.Close();

                CheckDataBaseConnection();
                connection.Open();
                NpgsqlCommand comm3 = new NpgsqlCommand("SELECT * FROM distance((@ville1), (@ville2))", connection);
                comm3.Parameters.AddWithValue("ville1", targettedCity);
                comm3.Parameters.AddWithValue("ville2", "%"+j.AdresseArr+"%");
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


        //This method uses the procedure reinit_password in the database to give an user a new password, in case it lost it.
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

        //This method check in the table voisin if a city exists
        public static bool DoesCityExist(string city)
        {
            CheckDataBaseConnection();
            connection.Open();
            city = city.ToUpper();
            NpgsqlCommand comm = new NpgsqlCommand("SELECT * FROM voisin WHERE nom=(@ville);", connection);
            comm.Parameters.AddWithValue("ville", city.ToUpper());
            NpgsqlDataReader result = comm.ExecuteReader();
            bool output = false;
            if (result.HasRows)
            {
                output = true;
            }
            connection.Close();
            return (output);

        }

        //This method checks if the connection is closed, and close it if it isn't. It returns false if it was already close, and true ohterwise.
        public static bool ConnectionCloser()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                return false;
            connection.Close();
            return true;
                
        }

        //This method allow an user to reserve a journey.
        //This method calls the reserve procedure on the database, that links an user to a journey, and that reduce the amount of people a journey can take by one
        public static void Reserve(int Eid, string Email)
        {
            CheckDataBaseConnection();
            connection.Open();
            NpgsqlCommand comm = new NpgsqlCommand("CALL reserve((@eid),(@email))", connection);
            comm.Parameters.AddWithValue("eid", Eid);
            comm.Parameters.AddWithValue("email", Email);
            comm.ExecuteReader();
            connection.Close();
        }

        //This method returns the phone number of the user who proposed the journey.
        //If there are no entry, it means that the user entered an eid of journey he proposed, so we return it's own phone number
        public static string getPhoneNumber(int eid)
        {
            CheckDataBaseConnection();
            connection.Open();
            string email;
            NpgsqlCommand comm = new NpgsqlCommand("SELECT * FROM reserve WHERE eid = (@eid)", connection);
            comm.Parameters.AddWithValue("eid", eid);
            NpgsqlDataReader result = comm.ExecuteReader();
            string output;
            if (result.HasRows)
            {
                result.Read();
                email = (string)result["email"];
                connection.Close();
                CheckDataBaseConnection();
                connection.Open();
                NpgsqlCommand comm3 = new NpgsqlCommand("SELECT numtel FROM utilisateur WHERE email = (@email)", connection);
                comm3.Parameters.AddWithValue("email", email);
                var result3 = comm3.ExecuteReader();
                result3.Read();
                output = (string)result3["numtel"];
                connection.Close();
            }
            else
            {
                output = InfoExchanger.User.Telephone;
            }
            

            connection.Close();
            return output;
        }

    }




}
