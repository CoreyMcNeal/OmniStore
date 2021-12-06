
namespace OmniClient;

//Remember to add note in readme about downloading the MYSQL package for the appropriate classes

public static class ClientProgram
{
    private static StoreCommunicator _storeCommunicator;
    
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
                                                                // connection to the server. Breaks if connection is made
        string?[] serverInformation = StoreCommunicator.SetupAndTestConnection();


                                                                //User logs in or makes a new account to log in with
        string[] credentials = LoginOrSignup(serverInformation);
        
        
        
        //By this point, the user will have logged in and prog. grabbed the credentials, or made user made a new account
        //and prog grabbed the credentials
        

        Console.WriteLine("\n");
        Console.WriteLine("Welcome to the C# Technology Store!");


        while (true)
        {
            int balance = StoreCommunicator.GetBalance(serverInformation, credentials);
            
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"BALANCE - ${balance}");
            Console.WriteLine("1 - Go Shopping");
            Console.WriteLine("2 - Load Card");
            Console.Write("::");
            string? menuChoice = MainMenuChoice();
            Console.WriteLine("\n");

            switch (menuChoice)
            {
                case "1":
                    //Method here to go shopping
                    GoShopping(serverInformation!, credentials);
                    break;
            
                case "2":
                    //method here to go load card with money
                    GoLoadCard(serverInformation, credentials);
                    break;
            
            }
        }

    }

    private static void GoShopping(string[] serverInformation, string[] credentials)
    {

        string productId = "";
        while (productId == "")
        {
            StoreCommunicator.ShowShelf(serverInformation, credentials);
            Console.WriteLine("Enter product ID of the product you want to buy.");
            Console.Write("::");
            productId = StoreCommunicator.GetUserProductId(serverInformation);
        }
        
        //Add a check for the items price and a check for the users balance. Push the transaction if the balance is
        //more than the item price

        int itemPrice = StoreCommunicator.GetPrice(serverInformation, int.Parse(productId));
        int balance = StoreCommunicator.GetBalance(serverInformation, credentials);

        if (balance >= itemPrice)
        {
            //-purchase product
            int newBalance = balance - itemPrice;
            
            //-subtract from balance



        }
        else 
        {
            //Message that the user doesn't have enough, return to menu to load more onto card.
        }
    }

    private static void GoLoadCard(string?[] serverInformation, string[] credentials)
    {
        Console.WriteLine("How much would you like to add onto your store card?");
        Console.Write("::$");
        int amount = getAmount();
        StoreCommunicator.AddToCard(serverInformation, credentials, amount);
        Console.WriteLine($"${amount} added to store card.");
    }

    private static int getAmount()
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
                continue;

                return amount;
            }
            catch (Exception e)
            {
                Console.WriteLine("Enter a positive whole number please.");
                Console.Write("::$");
            }
        }
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
                    string[] loginResults = StoreCommunicator.LoginDatabase(serverInformation[0],
                                                                    serverInformation[1],
                                                                    serverInformation[2],
                                                                    serverInformation[3]);
                    
                    if (loginResults[2] == "true") { return new[] {loginResults[0], loginResults[1]};}
                    
                    break;
                    
                }
                case "2":
                    //Code here for registering
                    //Go back into "1" to properly login and return login credentials
                    StoreCommunicator.RegisterDatabase(serverInformation[0],
                        serverInformation[1],
                        serverInformation[2],
                        serverInformation[3]);
                    break;
            }
            
            
            
            
            
        }
    }

    private static string MainMenuChoice()
    {
        while (true)
        {
            string? menuChoice = Console.ReadLine();
            if (menuChoice != "1" && menuChoice != "2")
            {
                Console.WriteLine("Invalid entry. Please choose 1 to go shopping, or 2 to load your card.");
                Console.Write(":::");
                continue;
            }

            return menuChoice;


        }
        
    }

    
}
