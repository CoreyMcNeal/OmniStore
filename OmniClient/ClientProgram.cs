
namespace OmniClient;

//Remember to add note in readme about downloading the MYSQL package for the appropriate classes

public static class ClientProgram
{
    private static StoreCommunicator _storeCommunicator;
    
    public static void Main(string[] args)
    {
        _storeCommunicator = new StoreCommunicator();
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
        


        Console.WriteLine("Welcome to the C# Technology Store!");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("1 - Go Shopping");
        Console.WriteLine("2 - Load Card");
        string? menuChoice = MainMenuChoice();

        switch (menuChoice)
        {
            case "1":
                //Method here to go shopping
                break;
            
            case "2":
                //method here to go load card with money
                break;
            
        }

        // 1 - Retrieve items, pricing, and stock from the sql server
        // 2 - Retrieve items, pricing and stock from the sql server for the cart (could be built similar to store)
        // 3 - Add money to the users account

        //Check the users card balance when trying to checkout from the My Cart Section



    }


    
    
    private static string[] LoginOrSignup(string?[] serverInformation)
    {
        while (true)
        {
            Console.WriteLine("1 - Login");
            Console.WriteLine("2 - Register / Signup");
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