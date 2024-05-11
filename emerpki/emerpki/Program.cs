namespace emerpki;

class Program
{
    
    static void Main(string[] args)
    {
        bool isDebug = false;
        bool emertestnet = false;

        if (args.Length > 0 && args[0] == "--debug")
        {
            isDebug = true;
        }

        if (args.Length > 1 && args[1] == "--testnet")
        {
            emertestnet = true;
        }
        Menu ProgramMenu = new Menu(isDebug, emertestnet);
        ProgramMenu.DisplayTestMenu();
    }

}


