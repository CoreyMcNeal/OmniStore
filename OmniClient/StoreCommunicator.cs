
using MySql.Data.MySqlClient;

namespace OmniClient;

public static class StoreCommunicator
{


                                                            //-Tests whether the database information actually connects,
                                                            //returns successful server information or empty if failed
    public static string?[] AttemptFirstConnection(string[] serverInformation)
    {
        string myConnectionString =
            $"Server={serverInformation[0]};Database={serverInformation[1]};Uid={serverInformation[2]};Pwd={serverInformation[3]};";
    
        try
        {
            using MySqlConnection myConn = new MySqlConnection(myConnectionString);
                myConn.Open();
                Console.WriteLine("Connected!");
                myConn.Close();
                return new[] {serverInformation[0], serverInformation[1], serverInformation[2], serverInformation[3],
                              myConnectionString};
        }
        catch (Exception e)
        {
            Console.WriteLine(e + "\n");
            Console.WriteLine("Failed to Connect!");
            return Array.Empty<string>();
        }
        
        

        
    }

    
                                                                //-Attempts to login to database "userInfo". If the match
                                                                //exists,
                                                                //-returns true in array with info. If not, return false
                                                                //in array.
    public static string[] LoginDatabase(string? myConnectionString)
    {
        
        Console.Write("Username: ");
        //Gather user entry
        string? username = Console.ReadLine();
    
        Console.Write("Password: ");
        //Gather user password
        string? password = Console.ReadLine();

        //Check with SQL Database if entry is valid. If not, send user back to login/register options
        using MySqlConnection myConn = new MySqlConnection(myConnectionString);
        
            myConn.Open();
            try
            {
                using MySqlCommand myCmd = new MySqlCommand();
                    myCmd.Connection = myConn;
                    myCmd.CommandText =
                        $"SELECT * FROM userInfo WHERE email='{username}' AND pin_number = '{password}';";

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
    public static void RegisterDatabase(string? myConnectionString)
    {
        while (true)
        {
            //Gather email
            string userEmail = GetEmail();

            //Gather PIN
            int passwordPin = GetPin();

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
    private static void CreateUserSqlStatement(string? sqlConnectionString, string? userEmail, int passwordPin)
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
    public static int GetUserBalance(string? myConnectionString, string[] credentials)
    {
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

    public static void SetUserBalance(string? myConnectionString, string[] credentials, int newBalance)
    {

        using MySqlConnection myConn = new MySqlConnection(myConnectionString);
            
            myConn.Open();
            using MySqlCommand myCmd = new MySqlCommand();
                myCmd.Connection = myConn;
                myCmd.CommandText =
                    $"UPDATE userInfo SET balance={newBalance} WHERE email='{credentials[0]}' AND pin_number='{credentials[1]}' AND userNUMID > 0;";
                myCmd.ExecuteReader();
    }

                                                        //Gets item price from store in database
    public static int GetPrice(string? myConnectionString, int productId)
    {
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

    public static string GetName(string? myConnectionString, int productId)
    {

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
    public static int GetStock(string? myConnectionString, int productId)
    {
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
        int currentStock = StoreCommunicator.GetStock(serverInformation[4], productId);
        int newStock = currentStock - 1;

        string myConnectionString = serverInformation[4];

            using MySqlConnection myConn = new MySqlConnection(myConnectionString);
                
                myConn.Open();
                using MySqlCommand myCmd = new MySqlCommand();
                    myCmd.Connection = myConn;
                    myCmd.CommandText = $"UPDATE storeInfo SET stock={newStock} WHERE sku='{productId}' AND sku > 0;";
                    myCmd.ExecuteReader();
    }

    public static string? GetUserProductId(string[] serverInformation)
    {
        string? id = Console.ReadLine();
        List<string> storeIds = GetAllIds(serverInformation);
        if (!storeIds.Contains(id!)) { return "";}

        return id;

    }
    
    private static List<string> GetAllIds(IReadOnlyList<string> serverInformation)
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

        int amountToAdd = (StoreCommunicator.GetUserBalance(serverInformation[4], credentials)) + amount;

        using MySqlConnection myConn = new MySqlConnection(serverInformation[4]);
            
            myConn.Open();
            using MySqlCommand myCmd = new MySqlCommand();
                myCmd.Connection = myConn;
                myCmd.CommandText =
                    $"UPDATE userInfo SET balance={amountToAdd} WHERE email='{credentials[0]}' AND pin_number='{credentials[1]}' AND userNUMID > 0;";
                myCmd.ExecuteReader();
    }
    
}