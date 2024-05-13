namespace emerpki;

class Program
{
    
    static void Main(string[] args)
    {
        var isDebug = false;
        var connectEmercoinTestNet = false;

        if (args.Length > 0 && args[0] == "--debug")
        {
            isDebug = true;
        }

        if (args.Length > 1 && args[1] == "--testnet")
        {
            connectEmercoinTestNet = true;
        }
        var programMenu = new Menu(isDebug, connectEmercoinTestNet);
        programMenu.DisplayTestMenu();
    }

}


