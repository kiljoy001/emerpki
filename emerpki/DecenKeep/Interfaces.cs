namespace DecenKeep;

public interface IDatabaseService
{
    Task<bool> CreateDatabaseAsync();
    Task<bool> InsertIntoDatabaseAsync();
}

public interface IFileService
{
    Task<bool> PinFileAsync();
}

public interface IWalletService
{
    Task<bool> CheckBalanceAsync();
}

public interface IBlockchainService
{
    Task<bool> PostToBlockchainAsync();
}