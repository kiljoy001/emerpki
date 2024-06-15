using System.Net.Http.Headers;
using static IPFSNodeLibrary.interfaces;

namespace IPFSNodeLibrary;

public class IPFSService: IIPFSCommands
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private IIPFSCommands _iipfsCommandsImplementation;

    public IPFSService(HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient;
        _baseUrl = baseUrl.TrimEnd('/');
    }
    public async Task<bool> PinFileAsync(string cid)
    {
        var url = $"{_baseUrl}/{IpfsEndpoint.PinFile.Url}?arg={cid}"; 
        var response = await _httpClient.PostAsync(url, null); 
        return response.IsSuccessStatusCode;
    }

    public async Task<string> AddFileAsync(string filePath)
    {
        var url = string.Format($"{_baseUrl}/{IpfsEndpoint.AddFile.Url}", filePath);
        using var content = new MultipartContent();
        await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        using var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Add(fileContent);
        
        var response = await _httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        return responseString;
    }

    public Task<bool> GetFileAsync(string cid)
    {
        throw new NotImplementedException();
    }
}