using System.Data.SQLite;

namespace DecenKeep;

public class Menu
{
    private readonly IDatabaseService _databaseService;
    private readonly IFileService _fileService;
    private readonly IWalletService _walletService;
    private readonly IBlockchainService _blockchainService;

    public Menu(bool isDebug, IDatabaseService databaseService, IFileService fileService,
        IWalletService walletService, IBlockchainService blockchainService)
    {
        IsDebug = isDebug;
        TestMenu = isDebug && ConnectEmercoinTestNet;
        _databaseService = databaseService;
        _fileService = fileService;
        _walletService = walletService;
        _blockchainService = blockchainService;
    }

    public bool IsDebug { get; set; }
    private bool ConnectEmercoinTestNet { get; set; }

    public bool TestMenu { get; }

    private void DisplayGreetings()
    {
        Console.WriteLine(
            "Welcome to DecenKeep, a Aergolite based, IPFS hosted file, using the Emercoin Blockchain network");
    }

    private void ChooseOptions()
    {
        //TO-DO Update text output and case to include adding hashed public keys data into the file. 
        Console.WriteLine("Please Choose an option:\n1)Create a SQLite Database\n2)Pin a SQLite Database on IPFS" +
                          "\n3)Check Emercoin wallet balance\n4)Post Merkle Root & File CID to blockchain\n5)Add public key hashes" +
                          "\n6)Exit program");
    }

    private async Task CreateSqLiteAsync()
    {
        try
        {
            var result = await _databaseService.CreateDatabaseAsync();
            Console.WriteLine(result ? "Database created successfully!" : "Failed to create the database.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error creating database: {e.Message}");
        }
    }

    private async Task CreateFilePinAsync()
    {
        try
        {
            var result = await _fileService.PinFileAsync();
            Console.WriteLine(result ? "File pinned successfully." : "Failed to pin file.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error pinning file: {e.Message}");
        }
    }

    private async Task CheckWalletBalanceAsync()
    {
        try
        {
            var balance = await _walletService.CheckBalanceAsync();
            Console.WriteLine($"Wallet balance: {balance}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error checking wallet balance: {e.Message}");
        }
    }

    private async Task PostToBlockchainAsync()
    {
        try
        {
            var result = await _blockchainService.PostToBlockchainAsync();
            Console.WriteLine(result ? "Posted to blockchain successfully." : "Failed to post to blockchain.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error posting to blockchain: {e.Message}");
        }
    }

    private async Task InsertIntoDatabaseAsync()
    {
        try
        {
            var result = await _databaseService.InsertIntoDatabaseAsync();
            Console.WriteLine(result ? "Inserted into database successfully." : "Failed to insert into database.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error inserting into database: {e.Message}");
        }
    }

    private void ExitProgram()
    {
        Console.WriteLine("Exiting program...");
    }

    public async Task DisplayTestMenuAsync()
    {
        DisplayGreetings();
        while (true)
        {
            ChooseOptions();
            string? optionChoice = Console.ReadLine();
            switch (optionChoice)
            {
                case "1":
                    await CreateSqLiteAsync();
                    break;
                case "2":
                    await CreateFilePinAsync();
                    break;
                case "3":
                    await CheckWalletBalanceAsync();
                    break;
                case "4":
                    await PostToBlockchainAsync();
                    break;
                case "5":
                    await InsertIntoDatabaseAsync();
                    break;
                case "6":
                    ExitProgram();
                    break;
                default:
                    Console.WriteLine("Invalid Option, please try another option");
                    break;
            }
        }
    }
}