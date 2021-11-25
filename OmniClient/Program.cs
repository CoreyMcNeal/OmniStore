
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
        //Login method here, check database for existing name. If it doesn't exist, let the user create a new one.
        //Log into the users account
        
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