
namespace OmniClient;

//Remember to add note in readme about downloading the MYSQL package for the appropriate classes

internal static class ClientProgram
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
         
        string?[] serverInformation = _storeCommunicator.SetupAndTestConnection();


                                                    //Gathers whether the user wants to make a new account or login
        string[] credentials = LoginOrSignup(serverInformation);

        //By this point, the user will have logged in and prog. grabbed the credentials, or made a new account and grabbed
        //the credentials
        


        // Console.WriteLine("Welcome to the C# Technology Store!");
        // Console.WriteLine("----------------------------------------");
        // Console.WriteLine("1 - Go Shopping");
        // Console.WriteLine("2 - Load Card");

        // 1 - Retrieve items, pricing, and stock from the sql server
        // 2 - Retrieve items, pricing and stock from the sql server for the cart (could be built similar to store)
        // 3 - Add money to the users account

        //Check the users card balance when trying to checkout from the My Cart Section



    }


    private static string[] LoginOrSignup(string?[] serverInformation)
    {
        string? userChoice;
        string[] loginResults;
        while (true)
        {
            Console.WriteLine("1 - Login");
            Console.WriteLine("2 - Register / Signup");
            userChoice = Console.ReadLine();
            if (userChoice != "1" && userChoice != "2") continue;


            if (userChoice == "1")
            {
                loginResults = _storeCommunicator.LoginDatabase(serverInformation[0],
                                                                serverInformation[1],
                                                                serverInformation[2],
                                                                serverInformation[3]);
                if (loginResults[2] == "true")
                {
                    return new[] { loginResults[0], loginResults[1]};
                }
                
            }
            else if (userChoice == "2")
            {
                //Code here for registering
                //Loop back into "1" to properly login and return login credentials
                
            }
            
            
            
        }
    }
}