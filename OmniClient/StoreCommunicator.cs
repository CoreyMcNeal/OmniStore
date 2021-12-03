
using MySql.Data.MySqlClient;

namespace OmniClient;

public class StoreCommunicator
{
        //Remember to add SQLClient download through nuget in the readme on github
        //Check for user login info, validate that it exists with username and password matching
        
        //When item is chosen to buy, check if the store has stock, if it does. remove one from the stock amount. If
        //not, let the user know theres not stock remaining for that item.
    

        
    public static string?[] SetupAndTestConnection()
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
                if (!CheckEmpty(serverName)) continue;
                //Usually localhost
    
                Console.Write("Database Name: ");
                databaseName = Console.ReadLine();
                if (!CheckEmpty(databaseName)) continue;
                //take in database name
    
                Console.Write("Database UserName: ");
                databaseUsername = Console.ReadLine();
                if (!CheckEmpty(databaseUsername)) continue;
                //Take in server name
    
                Console.Write("Database Password: ");
                databasePassword = Console.ReadLine();
                if (!CheckEmpty(databasePassword)) continue;
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

    //Attempts to login to database "userInfo". If the match exists, returns true in array with info. If not, return false in array.
    public static string[] LoginDatabase(string? serverAddress, string? nameDatabase, string? usernameDatabase, string? passwordDatabase)
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

    public static void RegisterDatabase(string? serverAddress, string? nameDatabase, string? usernameDatabase, string? passwordDatabase)
    {
        while (true)
        {
            //Gather email
            string? userEmail = GetEmail();

            //Gather PIN
            int passwordPin = GetPin();

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
                        
                        Console.WriteLine("User successfully created. Login with the newly created account.");
                        myReader.Close();
                        return;
                    }

                    Console.WriteLine("User email is already registered, please try a new one.");
                    myReader.Close();
                    return;


                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }


        }
    }
    
    private static void CreateUserSqlStatement(string sqlConnectionString, string? userEmail, int passwordPin)
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

    
    
    
    
    
    
    private static bool CheckEmpty(string? entry)
    {
        if (!string.IsNullOrWhiteSpace(entry)) return true;
        
        Console.WriteLine("Empty entry detected, please make a valid entry");
        return false;

    }
    
    private static string GetEmail()
    {
        while (true)
        {
            Console.Write("Enter UserEmail: ");

            string? useremail = Console.ReadLine();
            if (useremail == null || useremail.Contains(' ') || !useremail.Contains('@'))
            {
                Console.WriteLine("Please enter a valid formatted email.");
                continue;
            }

            return useremail;

        }
    }
    
    private static int GetPin()
    {
        string? passwordPin;
        while (true)
        {
            Console.Write("Enter Pin: ");
            
            passwordPin = Console.ReadLine();
            if (passwordPin == null || passwordPin.Contains('-') || passwordPin.Contains(' ') || passwordPin.Length >= 8)
            {
                Console.WriteLine("Please enter a positive number with no spaces, less than 8 numbers long.");
                continue;
            }

            break;
        }

        int passwordRealPin = Convert.ToInt32(passwordPin);
        return passwordRealPin;
    }
    
    
}