namespace DecenKeep;
using System.Collections.Generic;

public interface ITrezorService
{
    public Task InitializeAsync();
    public Task<string> GetPublicKeyAsync();
    public Task<string> SignTransactionAsync();
    
}
public interface ISecretHash
{
    Task<string> HashUserSecret(string input);
    Task<bool> VerifyUserSecret(string input, string hashedSecret);
}
public interface IDatabaseService
{
    Task<bool> InsertIntoDatabaseAsync<T>(string tableName, T data);
    Task<bool> UpdateDatabaseRecordAsync<T>(string tableName, string recordId, T updatedData);
    Task<bool> DeleteFromDatabaseAsync(string tableName, string recordId);
    Task<T> GetRecordByIdAsync<T>(string tableName, string recordId);
    Task<IEnumerable<T>> GetAllRecordsAsync<T>(string tableName);
    Task<string> EncryptDataAsync(string data, string key);
    Task<string> DecryptDataAsync(string encryptedData, string key);
    Task<bool> CreateDatabaseAsync();
}

public interface IFileService
{
    Task<bool> PinFileAsync();
    Task<bool> CreateBackupFolder(string location);
    Task<bool> AddFileToBackupAsync(string filePath);
    Task<bool> RemoveFileFromBackupAsync(string filePath);
    Task<IEnumerable<string>> ListBackedUpFilesAsync();
    Task<bool> EncryptAndBackupCidAsync(string cid, string userKey);
    Task<string> RetrieveEncryptedCidAsync(string cid, string userKey);
    Task<bool> PostKeyToEmercoinAsync(string encryptedKey);
    Task<string> RestoreFromEmercoinAsync(string nvsEntry);
    Task<string> GenerateNewEncryptionKeyAsync();
    Task<bool> InitializeBackupDirectoryAsync(string location);
}

public interface IWalletService
{
    Task<bool> CheckBalanceAsync();
    Task<double> EstimateCosts();
}

public interface IBlockchainService
{
    Task<bool> PostToBlockchainAsync();
}

public interface ICostEstimator
{
    double EstimateCost(int rewardInCents, int leaseTimeInYears, int dataSizeInBytes);
}
public interface IBayesianCorrector
{
    void UpdatePosterior(List<IObservedData> observedData);
    (double Mean, double StdDev) GetPosteriorStatistics();
    double CorrectEstimate(double initialEstimate, int dataSize);
}
public interface IObservedData
{
    int Reward { get; set; }
    double LeaseTime { get; set; }
    double Cost { get; set; }
}
public interface IHybridEstimator
{
    void UpdateBayesianCorrector(List<IObservedData> observedData);
    double EstimateCost(int rewardInCents, int leaseTimeInYears, int dataSizeInBytes);
    (double Mean, double StdDev) GetPosteriorStatistics();
}
