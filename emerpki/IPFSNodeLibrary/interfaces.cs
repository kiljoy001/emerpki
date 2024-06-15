namespace IPFSNodeLibrary;

public interface interfaces
{
    public interface IIPFSCommands
    {
        Task<bool> PinFileAsync(string cid);
        Task<string> AddFileAsync(string cid);
        Task<bool> GetFileAsync(string cid);
    }

    public interface IIPFSFileMetadata
    {
        Task<string> GetCid();
        Task<string> GetFileName();
        Task<string> GetSize();
    }
    
    public interface IIPFSSettings
    {
        public string BaseUrl { get; set; }
    }
}