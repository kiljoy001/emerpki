using System.Data.SQLite;
namespace emerpki;

public class Menu
{
    public Menu(bool isDebug, bool connectEmercoinTestNet)
    {
        IsDebug = isDebug;
        ConnectEmercoinTestNet = connectEmercoinTestNet;
        TestMenu = isDebug && ConnectEmercoinTestNet;
    }

    public bool IsDebug { get; set; }
    private bool ConnectEmercoinTestNet { get; set; }

    public bool TestMenu { get; }

    private void DisplayGreetings()
    {
        Console.WriteLine("Welcome to EmerPKI, a SQLite based, IPFS hosted file, using the Emercoin Blockchain network");
    }

    private void ChooseOptions()
    {
        //TO-DO Update text output and case to include adding hashed public keys data into the file. 
        Console.WriteLine("Please Choose an option:\n1)Create a SQLite Database\n2)Pin a SQLite Database on IPFS" +
                          "\n3)Check Emercoin wallet balance\n4)Post Merkle Root & File CID to blockchain\n5)Add public key hashes" +
                          "\n6)Exit program");
    }

    private bool CreateSqLite()
    {
        var connectionString = "Data Source=public-keys.db";
        using (var connection = new SQLiteConnection(connectionString))
        {
            try
            {
                
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }

    private bool CreateFilePin()
    {
        throw new NotImplementedException();
    }

    private bool CheckWalletBalance()
    {
        throw new NotImplementedException();
    }

    private bool PostToBlockchain()
    {
        throw new NotImplementedException();
    }

    private bool ExitProgram()
    {
        throw new NotImplementedException();
    }
    private void InsertIntoDatabase()
    {
        throw new NotImplementedException();
    }
    public void DisplayTestMenu()
    {
        DisplayGreetings();
        ChooseOptions();
        string? optionChoice = Console.ReadLine();
        switch (optionChoice)
        {
            case "1":
                CreateSqLite();
                break;
            case "2":
                CreateFilePin();
                break;
            case "3":
                CheckWalletBalance();
                break;
            case "4":
                PostToBlockchain();
                break;
            case "5":
                InsertIntoDatabase();
                break;
            case "6":
                ExitProgram();
                break;
            default:
                ChooseOptions();
                break;
            
        }
        
    }

   
}