
using MySql.Data.MySqlClient;

namespace OmniClient;

public class StoreCommunicator
{
        //Remember to add SQLClient download through nuget in the readme on github
        //Check for user login info, validate that it exists with username and password matching
        
        //When item is chosen to buy, check if the store has stock, if it does. remove one from the stock amount. If
        //not, let the user know theres not stock remaining for that item.
    

                                                                //Tests whether the database information actually connects
                                                                //to a database, returns successful server information
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
                if (serverName == "x")
                {
                    serverName = "localhost";
                }
                if (!CheckEmpty(serverName)) continue;
                //Usually localhost
    
                Console.Write("Database Name: ");
                databaseName = Console.ReadLine();
                if (databaseName == "x")
                {
                    databaseName = "storeDB";
                }
                if (!CheckEmpty(databaseName)) continue;
                //take in database name
    
                Console.Write("Database UserName: ");
                databaseUsername = Console.ReadLine();
                if (databaseUsername == "x")
                {
                    databaseUsername = "root";
                }
                if (!CheckEmpty(databaseUsername)) continue;
                //Take in server name
    
                Console.Write("Database Password: ");
                databasePassword = Console.ReadLine();
                if (databasePassword == "x")
                {
                    databasePassword = "cm117670";
                }
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

    
                                                                //Attempts to login to database "userInfo". If the match exists,
                                                                //returns true in array with info. If not, return false in array.
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

    
    
    
    
                                                                    //Process to create user in database
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
    
    
                                                                    //Executes SQL statement to create user in Database                                               
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






    public static void ShowShelf( string?[] serverInformation,string[] credentials)
    {
        string myConnectionString = $"Server={serverInformation[0]};Database={serverInformation[1]};Uid={serverInformation[2]};Pwd={serverInformation[3]};";

        using MySqlConnection myConn = new MySqlConnection(myConnectionString);
            myConn.Open();

            try
            {
                MySqlCommand myCmd = new MySqlCommand();
                myCmd.Connection = myConn;
                myCmd.CommandText = $"SELECT * FROM storeInfo;";

                MySqlDataReader myReader = myCmd.ExecuteReader();
                if (!myReader.HasRows)
                {
                    Console.WriteLine("Shelf (Database) is empty");
                    return;
                }

                while (myReader.Read())
                {
                    Console.WriteLine($"ID: {myReader["sku"]}");
                    Console.WriteLine($"Product: {myReader["name"]}");
                    Console.WriteLine($"Stock: {myReader["stock"]}");
                    Console.WriteLine($"Price: ${myReader["price"]}");
                    Console.WriteLine();

                }
                myReader.Close();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error trying to execute SQL Command");
                throw;
            }
    }

    public static void AttemptPurchase(string?[] serverInformation, string[] credentials)
    {
        //Get users balance
        //Get price of desired item
        //See if users balance is enough to get the item
        //Take 1 away from the stock of item
        
    }

    //Checks if entry is empty
    private static bool CheckEmpty(string? entry)
    {
        if (!string.IsNullOrWhiteSpace(entry)) return true;
        
        Console.WriteLine("Empty entry detected, please make a valid entry");
        return false;

    }
    
                                                        //Process to get and return email from user
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
    
                                                        //Process to get and return pin from user
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
    
    

                                                        //Gets balance from user in databse
    public static int GetUserBalance(string?[] serverInformation,string[] credentials)
    {
        string myConnectionString = $"Server={serverInformation[0]};Database={serverInformation[1]};" +
                                    $"Uid={serverInformation[2]};Pwd={serverInformation[3]};";

        using MySqlConnection myConn = new MySqlConnection(myConnectionString);
        
            myConn.Open();
            using MySqlCommand myCmd = new MySqlCommand();
                myCmd.Connection = myConn;
                myCmd.CommandText = $"SELECT balance FROM userInfo WHERE email='{credentials[0]}' AND pin_number='{credentials[1]}';";
                                        
                MySqlDataReader myReader = myCmd.ExecuteReader();
                while (myReader.Read())
                {
                    return (int) myReader["balance"];
                }

                return -1;
    }

    public static void SetUserBalance(string?[] serverInformation, string[] credentials, int newBalance)
    {
        string myConnectionString = $"Server={serverInformation[0]};Database={serverInformation[1]};" +
                                    $"Uid={serverInformation[2]};Pwd={serverInformation[3]};";

        using MySqlConnection myConn = new MySqlConnection(myConnectionString);
            
            myConn.Open();
            using MySqlCommand myCmd = new MySqlCommand();
                myCmd.Connection = myConn;
                myCmd.CommandText = $"UPDATE userInfo SET balance={newBalance} WHERE email='{credentials[0]}' AND pin_number='{credentials[1]}' AND userNUMID > 0;";
                MySqlDataReader myReader = myCmd.ExecuteReader();
    }

                                                        //Gets item price from store in database
    public static int GetPrice(string?[] serverInformation, int productId)
    {
        
        string myConnectionString = $"Server={serverInformation[0]};Database={serverInformation[1]};" +
                                    $"Uid={serverInformation[2]};Pwd={serverInformation[3]};";

        using MySqlConnection myConn = new MySqlConnection(myConnectionString);
        
            myConn.Open();
            using MySqlCommand myCmd = new MySqlCommand();
                myCmd.Connection = myConn;
                myCmd.CommandText = $"SELECT price FROM storeInfo WHERE sku='{productId}';";
                        
                MySqlDataReader myReader = myCmd.ExecuteReader();
                while (myReader.Read())
                {
                    return (int) myReader["price"];
                }

                return -1;
    }

    public static string GetName(string?[] serverInformation, int productId)
    {
        string myConnectionString = $"Server={serverInformation[0]};Database={serverInformation[1]};" +
                                    $"Uid={serverInformation[2]};Pwd={serverInformation[3]};";

        using MySqlConnection myConn = new MySqlConnection(myConnectionString);
        
            myConn.Open();
            using MySqlCommand myCmd = new MySqlCommand();
                myCmd.Connection = myConn;
                myCmd.CommandText = $"SELECT name FROM storeInfo WHERE sku='{productId}';";
                                
                MySqlDataReader myReader = myCmd.ExecuteReader();
                while (myReader.Read())
                {
                    return (string)myReader["name"];
                }

                return "";
    }
    
                                                        //Gets item stock from store in database
    public static int GetStock(string[] serverInformation, int productId)
    {
        string myConnectionString = $"Server={serverInformation[0]};Database={serverInformation[1]};" +
                                    $"Uid={serverInformation[2]};Pwd={serverInformation[3]};";

        using MySqlConnection myConn = new MySqlConnection(myConnectionString);
        
            myConn.Open();
            using MySqlCommand myCmd = new MySqlCommand();
                myCmd.Connection = myConn;
                myCmd.CommandText = $"SELECT stock FROM storeInfo WHERE sku='{productId}';";
                                
                MySqlDataReader myReader = myCmd.ExecuteReader();
                while (myReader.Read())
                {
                    return (int) myReader["stock"];
                }

                return -1;
    }

    public static void RemoveFromStock(string[] serverInformation, int productId)
    {
        int currentStock = StoreCommunicator.GetStock(serverInformation, productId);
        int newStock = currentStock - 1;
        
        string myConnectionString = $"Server={serverInformation[0]};Database={serverInformation[1]};" +
                                    $"Uid={serverInformation[2]};Pwd={serverInformation[3]};";

            using MySqlConnection myConn = new MySqlConnection(myConnectionString);
                
                myConn.Open();
                using MySqlCommand myCmd = new MySqlCommand();
                    myCmd.Connection = myConn;
                    myCmd.CommandText = $"UPDATE storeInfo SET stock={newStock} WHERE sku='{productId}' AND sku > 0;";
                    MySqlDataReader myReader = myCmd.ExecuteReader();
    }

    public static string GetUserProductId(string[] serverInformation)
    {
        string? id = Console.ReadLine();
        List<string> storeIds = GetAllIds(serverInformation);
        if (!storeIds.Contains(id)) { return "";}

        return id;

    }
    
    private static List<string> GetAllIds(string[] serverInformation)
    {
        string myConnectionString = $"Server={serverInformation[0]};Database={serverInformation[1]};" +
                                    $"Uid={serverInformation[2]};Pwd={serverInformation[3]};";

        using MySqlConnection myConn = new MySqlConnection(myConnectionString);
        
            myConn.Open();
            using MySqlCommand myCmd = new MySqlCommand();
                myCmd.Connection = myConn;
                myCmd.CommandText = $"SELECT sku FROM storeInfo;";

                List<string> skuArray = new List<string>();
                MySqlDataReader myReader = myCmd.ExecuteReader();
                
                while (myReader.Read()) { skuArray.Add($"{myReader["sku"]}");}
                
                return skuArray;
    }

    public static void AddToCard(string?[] serverInformation, string[] credentials, int amount)
    {
        string myConnectionString = $"Server={serverInformation[0]};Database={serverInformation[1]};" +
                                    $"Uid={serverInformation[2]};Pwd={serverInformation[3]};";

        int amountToAdd = (StoreCommunicator.GetUserBalance(serverInformation, credentials)) + amount;

        using MySqlConnection myConn = new MySqlConnection(myConnectionString);
            
            myConn.Open();
            using MySqlCommand myCmd = new MySqlCommand();
                myCmd.Connection = myConn;
                myCmd.CommandText = $"UPDATE userInfo SET balance={amountToAdd} WHERE email='{credentials[0]}' AND pin_number='{credentials[1]}' AND userNUMID > 0;";
                MySqlDataReader myReader = myCmd.ExecuteReader();
    }
    
}