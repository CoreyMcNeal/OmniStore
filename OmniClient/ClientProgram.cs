using System;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Tls;

namespace OmniClient;

//Remember to add note in readme about downloading the MYSQL package for the appropriate classes

internal static class ClientProgram
{
    private static StoreCommunicator storeCommunicator;
    
    public static void Main(string[] args)
    {
        storeCommunicator = new StoreCommunicator();
        UIStart();
    }

    
    //UI
    private static void UIStart()
    {
                                                           
        Console.WriteLine("Welcome to the C# Technology Store!");
        Console.WriteLine("We'll need some database information first :)");

        
                                                                // Gets server info from user inputs, and tests the
                                                                // connection to the server. Breaks if connection is made
         
        string?[] serverInformation = storeCommunicator.SetupAndTestConnection();


                                                            //Gathers whether the user wants to make a new account or login
                                                            //Breaks when valid entry is made
        string? userChoice;
        while (true)
        {
            Console.WriteLine("1 - Login");
            Console.WriteLine("2 - Register / Signup");
            userChoice = Console.ReadLine();
            if (userChoice != "1" && userChoice != "2") continue;
            
            switch (userChoice)
            {
                case "1":
                    //Code here for Login, maybe condense into method
                    while (true)
                    {
                        if (storeCommunicator.LoginDatabase(serverInformation[0], serverInformation[1],
                                serverInformation[2], serverInformation[3]))
                        {
                            return;
                        }
                        
                        break;
                    }
                    
                    continue;
                
                case "2":
                    //Code here for Signup
                    break;
            }
            
        }

        // 1 - try to log into DB with the email and pin
        // 2 - signup with the new account information
        

        
        
        
        
        
        Console.WriteLine("Welcome to the C# Technology Store!");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("1 - Go Shopping");
        Console.WriteLine("2 - My Cart");
        Console.WriteLine("3 - Load Card");
        
        // 1 - Retrieve items, pricing, and stock from the sql server
        // 2 - Retrieve items, pricing and stock from the sql server for the cart (could be built similar to store)
        // 3 - Add money to the users account
        
        //Check the users card balance when trying to checkout from the My Cart Section
        
        

    }
    
    
    
}