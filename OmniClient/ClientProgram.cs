
namespace OmniClient;

public static class ClientProgram
{

    public static void Main(string[] args)
    {
        UiStart();
    }

    
    //UI
    private static void UiStart()
    {
                                                           
        Console.WriteLine("Welcome to the C# Technology Store!");
        Console.WriteLine("We'll need some database information first :)");

        
                                                                // Gets server info from user inputs, and tests the
                                                                // connection to the server.
        string?[] serverInformation = SetupAndTestConnection();


                                                                //User logs in or makes a new account to log in with
        string[] credentials = LoginOrSignup(serverInformation);


        Console.WriteLine("\n");
        Console.WriteLine("Welcome to the C# Technology Store!");


        while (true)
        {
            int balance = StoreCommunicator.GetUserBalance(serverInformation[4], credentials);
            
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"BALANCE - ${balance}");
            Console.WriteLine("1 - Go Shopping");
            Console.WriteLine("2 - Load Card");
            Console.WriteLine("3 - Leave");
            Console.Write("::");
            string menuChoice = MainMenuChoice();
            Console.WriteLine("\n");

            switch (menuChoice)
            {
                case "1":
                    
                    GoShopping(serverInformation!, credentials);
                    continue;

                case "2":
                    
                    GoLoadCard(serverInformation, credentials);
                    continue;
            
                case "3":
                    break;
            }

            break;
        }

    }

    private static string?[] SetupAndTestConnection()
    {
        
        string?[] results = Array.Empty<string>();
        while (results.Length == 0)
        {
            Console.Write("Server Name: ");
            string? serverName = Console.ReadLine();
            if (serverName == "x")
            {
                serverName = "localhost";
            }
            if (!CheckEmpty(serverName)) continue;
            

            Console.Write("Database Name: ");
            string? databaseName = Console.ReadLine();
            if (databaseName == "x")
            {
                databaseName = "storeDB";
            }
            if (!CheckEmpty(databaseName)) continue;
            

            Console.Write("Database UserName: ");
            string? databaseUsername = Console.ReadLine();
            if (databaseUsername == "x")
            {
                databaseUsername = "root";
            }
            if (!CheckEmpty(databaseUsername)) continue;
            

            Console.Write("Database Password: ");
            string? databasePassword = Console.ReadLine();
            if (databasePassword == "x")
            {
                databasePassword = "cm117670";
            }
            if (!CheckEmpty(databasePassword)) continue;

            string[] serverInformation = new string[] {serverName!, databaseName!, databaseUsername!, databasePassword!};
            
            results = StoreCommunicator.AttemptFirstConnection(serverInformation);
            
        }

        return results;
    }
    
    private static string[] LoginOrSignup(string?[] serverInformation)
    {
        while (true)
        {
            Console.WriteLine("1 - Login");
            Console.WriteLine("2 - Register / Signup");
            Console.Write("::");
            string? userChoice = Console.ReadLine();
            if (userChoice != "1" && userChoice != "2") continue;


            switch (userChoice)
            {
                case "1":
                {
                    string[] loginResults = StoreCommunicator.LoginDatabase(serverInformation[4]);
                    
                    if (loginResults[2] == "true") { return new[] {loginResults[0], loginResults[1]};}
                    
                    break;
                    
                }
                case "2":
                    StoreCommunicator.RegisterDatabase(serverInformation[4]);
                    break;
            }
        }
    }
    
    private static string MainMenuChoice()
    {
        while (true)
        {
            string? menuChoice = Console.ReadLine();
            if (menuChoice != "1" && menuChoice != "2" && menuChoice != "3")
            {
                Console.WriteLine("Invalid entry. Please choose 1 to go shopping, or 2 to load your card.");
                Console.Write(":::");
                continue;
            }

            return menuChoice;


        }
        
    }

    private static void GoShopping(string[] serverInformation, string[] credentials)
    {

        string? productId = "";
        
        while (productId is "" or null)
        {
            StoreCommunicator.ShowShelf(serverInformation, credentials);
            Console.WriteLine("Enter product ID of the product you want to buy.");
            Console.Write("::");
            productId = StoreCommunicator.GetUserProductId(serverInformation);
        }

        int itemPrice = StoreCommunicator.GetPrice(serverInformation[4], int.Parse(productId));
        int balance = StoreCommunicator.GetUserBalance(serverInformation[4], credentials);
        int stock = StoreCommunicator.GetStock(serverInformation[4], int.Parse(productId));
        string itemName = StoreCommunicator.GetName(serverInformation[4], int.Parse(productId));

        if (balance >= itemPrice && stock > 0)
        {
            int newBalance = balance - itemPrice;
            StoreCommunicator.SetUserBalance(serverInformation[4], credentials, newBalance);
            StoreCommunicator.RemoveFromStock(serverInformation, int.Parse(productId));
            
            Console.WriteLine($"${itemPrice} was removed from store account.");
            Console.WriteLine($"{itemName} was purchased!");

        }
        else if (balance >= itemPrice && stock < 1)
        {
            
            Console.WriteLine($"{itemName} is out of stock.");
            
        }
        else if (balance < itemPrice && stock > 0)
        {
            
            Console.WriteLine($"Not enough money in account for purchase. Load your card with more money " +
                              $"at the main menu.");
            
        }
        else
        {
            Console.WriteLine($"Not enough money in account for purchase and item is out of stock.");
            
        }
    }

    private static void GoLoadCard(string?[] serverInformation, string[] credentials)
    {
        Console.WriteLine("How much would you like to add onto your store card?");
        Console.Write("::$");
        int amount = AddMoneyAmount();
        StoreCommunicator.AddToCard(serverInformation, credentials, amount);
        Console.WriteLine($"${amount} added to store card.");
    }

    private static int AddMoneyAmount()
    {
        while (true)
        {
            string? stringAmount = Console.ReadLine();
            try
            {
                int amount = Convert.ToInt32(stringAmount);
                
                if (amount >= 1) return amount;
                
                
                Console.WriteLine("Enter a positive whole number please");
                Console.Write("::$");

            }
            catch
            {
                Console.WriteLine("Enter a positive whole number please.");
                Console.Write("::$");
            }
        }
    }
    
    private static bool CheckEmpty(string? entry)
    {
        if (!string.IsNullOrWhiteSpace(entry)) return true;
        
        Console.WriteLine("Empty entry detected, please make a valid entry");
        return false;

    }
}
