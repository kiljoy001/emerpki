namespace DecenKeep;

class Program
{
    private static bool debugMode = false;
    private static bool useTestnet = false;
    
    static void Main(string[] args)
    {
        foreach (var arg in args)
        {
            if (arg.Equals("--debug", StringComparison.OrdinalIgnoreCase))
            {
                debugMode = true;
            }
            else if (arg.Equals("--testnet", StringComparison.OrdinalIgnoreCase))
            {
                useTestnet = true;
            }
        }

        if (debugMode)
        {
            Console.WriteLine("Debug mode enabled.");
        }
        
        if (useTestnet)
        {
            Console.WriteLine("Using Emercoin testnet.");
        }
        //TO-DO implement application features
        // var program = new Menu();
        // await program.DisplayTestMenuAsync();
    }

}


