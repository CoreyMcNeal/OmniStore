
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
    
    
    

    public static bool testConnection(string myConnectionString)
    {
        using MySqlConnection myConn = new MySqlConnection(myConnectionString);
            try
            {
                myConn.Open();
                Console.WriteLine("Connected!");
                myConn.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Failed to Connect!");
                return false;
            }
    }
    
    public static string getConnectionString()
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

        return myConnectionString;
    }
    
    private static bool checkEmpty(string? entry)
    {
        if (!string.IsNullOrWhiteSpace(entry)) return true;
        
        Console.WriteLine("Empty entry detected, please make a valid entry");
        return false;

    }
    
    
}