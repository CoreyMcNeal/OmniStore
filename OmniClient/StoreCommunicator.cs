
using MySql.Data.MySqlClient;

namespace OmniClient;

public class StoreCommunicator
{
        //Remember to add SQLClient download through nuget in the readme on github
        //Check for user login info, validate that it exists with username and password matching
        
        //When item is chosen to buy, check if the store has stock, if it does. remove one from the stock amount. If
        //not, let the user know theres not stock remaining for that item.


        public static void Login()
    {
        const string serverAddress = "localhost";
        const string databaseName = "storeDB";
        const string databaseUsername = "root";
        const string databasePassword = "cm117670";
        string myConnectionString = $"Server={serverAddress};Database={databaseName};Uid={databaseUsername};Pwd={databasePassword};";

        using (MySqlConnection myConn = new MySqlConnection(myConnectionString))
        {
            myConn.Open();
            try
            {
                Console.WriteLine("Opened!");
                
                using MySqlCommand myCmd = new MySqlCommand();
                    myCmd.Connection = myConn;
                    myCmd.CommandText = "SELECT * FROM userInfo";

                    MySqlDataReader myReader = myCmd.ExecuteReader();

                    while (myReader.Read())
                    {
                        Console.WriteLine($"email: {myReader["email"]}");
                    }
                    myReader.Close();
                    
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                myConn.Close();
                throw;
            }
            
            myConn.Close();
        }

        Console.ReadKey();
    }


    //Attempts to login to database "userInfo". If the match exists, returns true. If not, return false.
    public string[] LoginDatabase(string? serverAddress, string? nameDatabase, string? usernameDatabase, string? passwordDatabase)
    {
        
        Console.Write("Username: ");
        //Gather user entry
        string? username = Console.ReadLine();
    
        Console.Write("Password: ");
        //Gather user password
        string? password = Console.ReadLine();

        //Check with SQL Database if entry is valid. If not, send user back to login/register options
        string myConnectionString = $"Server={serverAddress};Database={nameDatabase};Uid={usernameDatabase};Pwd={passwordDatabase};";
        
        using MySqlConnection myConn = new MySqlConnection(myConnectionString);
        
            myConn.Open();
            try
            {
                using MySqlCommand myCmd = new MySqlCommand();
                    myCmd.Connection = myConn;
                    myCmd.CommandText = $"SELECT * FROM userInfo WHERE email='{username}' AND pin_number = '{password}';";

                    MySqlDataReader myReader = myCmd.ExecuteReader();
                    if (!myReader.HasRows)
                    {
                        
                        Console.WriteLine("User doesn't exist.");
                        string[] userAndPin = {"", "", "false"};
                        return userAndPin;
                        
                    }

                    while (myReader.Read())
                    {
                        
                        Console.WriteLine($"Login successful.");
                        string[] userAndPin = {username!, password!, "true"};
                        return userAndPin;
                        
                    }
                    myReader.Close();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                string[] userAndPin = {"","", "error"};
                return userAndPin;
            }

            
            
            return Array.Empty<string>();
    }

    public bool RegisterDatabase(string? serverAddress, string? nameDatabase, string? usernameDatabase, string? passwordDatabase)
    {
        while (true)
        {
            //Gather email
            Console.Write("Enter Email: ");
            string? userEmail = Console.ReadLine();

            //Gather PIN
            Console.Write("Enter Pin: ");
            int passwordPin = Convert.ToInt32(Console.ReadLine());
            //make sure pin is a 4 number positive int
            
            
            string myConnectionString = $"Server={serverAddress};Database={nameDatabase};Uid={usernameDatabase};Pwd={passwordDatabase};";

            using MySqlConnection myConn = new MySqlConnection(myConnectionString);
                
                myConn.Open();
                try
                {
                    using MySqlCommand myCmd = new MySqlCommand();

                    myCmd.Connection = myConn;
                    myCmd.CommandText = $"SELECT * FROM userInfo WHERE email='{userEmail}';";

                    MySqlDataReader myReader = myCmd.ExecuteReader();
                    if (!myReader.HasRows)
                    {
                        
                        CreateUserSqlStatement(myConnectionString, userEmail, passwordPin);
                        
                        Console.WriteLine("User successfully created");
                        myReader.Close();
                        return true;
                    }

                    Console.WriteLine("User email is already registered, please try a new one.");
                    myReader.Close();
                    return false;


                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }




                //Add email and pin to userinfo database


        }
    }
    
    

    public string?[] SetupAndTestConnection()
    {
        while (true)
        {
            string? serverName;
            string? databaseName;
            string? databaseUsername;
            string? databasePassword;
            while (true)
            {
                Console.Write("Server Name: ");
                serverName = Console.ReadLine();
                if (!checkEmpty(serverName)) continue;
                //Usually localhost
        
                Console.Write("Database Name: ");
                databaseName = Console.ReadLine();
                if (!checkEmpty(databaseName)) continue;
                //take in database name
        
                Console.Write("Database UserName: ");
                databaseUsername = Console.ReadLine();
                if (!checkEmpty(databaseUsername)) continue;
                //Take in server name
        
                Console.Write("Database Password: ");
                databasePassword = Console.ReadLine();
                if (!checkEmpty(databasePassword)) continue;
                //take in database name

                break;
            }
        
            string myConnectionString = $"Server={serverName};Database={databaseName};Uid={databaseUsername};Pwd={databasePassword};";
        
            using MySqlConnection myConn = new MySqlConnection(myConnectionString);
            try
            {
                myConn.Open();
                Console.WriteLine("Connected!");
                myConn.Close();
                return new [] {serverName, databaseName, databaseUsername, databasePassword};
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Failed to Connect!");
                
            }
        }
        
    }

    private void CreateUserSqlStatement(string sqlConnectionString, string? userEmail, int passwordPin)
    {
        using MySqlConnection myConn = new MySqlConnection(sqlConnectionString);
                
            myConn.Open();
            try
            {
                using MySqlCommand myCmd = new MySqlCommand();

                myCmd.Connection = myConn;
                myCmd.CommandText = $"INSERT INTO userInfo VALUES (NULL, '{userEmail}', {passwordPin}, 0);";
                MySqlDataReader myReader = myCmd.ExecuteReader();
                myReader.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
    }

    private bool checkEmpty(string? entry)
    {
        if (!string.IsNullOrWhiteSpace(entry)) return true;
        
        Console.WriteLine("Empty entry detected, please make a valid entry");
        return false;

    }
    
    
}