
using System;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace OmniClient;

public class StoreCommunicator
{

    public StoreCommunicator()
    {
        //Remember to add SQLClient download through nuget in the readme on github
        //Check for user login info, validate that it exists with username and password matching
        
        //When item is chosen to buy, check if the store has stock, if it does. remove one from the stock amount. If
        //not, let the user know theres not stock remaining for that item.
        
       
        
        
    }

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
                        Console.WriteLine($"email: {myReader["email"].ToString()}");
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
    public bool LoginDatabase(string? serverAddress, string? nameDatabase, string? usernameDatabase, string? passwordDatabase)
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
                    return false;
                }

                while (myReader.Read())
                {
                    Console.WriteLine($"Login successful.");
                    return true;
                }
                myReader.Close();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
                







        return true;
    }

    // public bool attemptLogin()
    // {
    //     
    // }

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
                return new string?[] {serverName, databaseName, databaseUsername, databasePassword};
            }
            catch (Exception e)
            {
                // Console.WriteLine(e);
                Console.WriteLine("Failed to Connect!");
                
            }
        }
        
    }

    private bool checkEmpty(string? entry)
    {
        if (!string.IsNullOrWhiteSpace(entry)) return true;
        
        Console.WriteLine("Empty entry detected, please make a valid entry");
        return false;

    }
    
    
}