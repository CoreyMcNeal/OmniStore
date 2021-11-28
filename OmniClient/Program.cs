using System;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Tls;

namespace OmniClient;

internal static class Program
{
    public static void Main(string[] args)
    {
        UIStart();
    }

    
    //UI
    private static void UIStart()
    {
                                                           
        Console.WriteLine("Welcome to the C# Technology Store!");
        Console.WriteLine("We'll need some database information first :)");
        
        

                                                                // Gets server info from user inputs, and tests the
                                                                // connection to the server
        while (true)
        {
            string myConnectionString = StoreCommunicator.getConnectionString();
            if (StoreCommunicator.testConnection(myConnectionString)) break;
        }



        while (true)
        {
            Console.WriteLine("1 - Login");
            Console.WriteLine("2 - Register / Signup");
            string? userChoice = Console.ReadLine();
            if (userChoice == "1" || userChoice == "2") break;
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