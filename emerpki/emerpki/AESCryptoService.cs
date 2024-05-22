using System.Globalization;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;


namespace emerpki;


public class AESCryptoService
{
    public KeyParameter Key;
    private readonly byte[] _iv;

    public AESCryptoService(byte[] iv)
    {
        _iv = iv;
    }
    
    private byte[] GenerateNonce(int size)
    {
        var nonce = new byte[size];
        var randomNumber = new SecureRandom();
        randomNumber.NextBytes(nonce);
        return nonce;
    }

    private byte[] GenerateAAD(string context)
    {
        List<byte> combinedBytes = new List<byte>();
        var convertStringBytes = Encoding.UTF8.GetBytes(context);
        var convertDateTimeBytes = Encoding.UTF8.GetBytes(DateTime.UtcNow.ToString(
            CultureInfo.InvariantCulture));
        combinedBytes.AddRange(convertStringBytes);
        combinedBytes.AddRange(convertDateTimeBytes);
        return combinedBytes.ToArray();
    }

    private byte[] appendAADDataBytes(byte[] encrypted, byte[] nonce, byte[] aadData)
    {
        var dataLength = nonce.Length + aadData.Length + encrypted.Length;
        var combinedBytes = new List<byte>();
        combinedBytes.AddRange(nonce);
        combinedBytes.AddRange(aadData);
        combinedBytes.AddRange(encrypted);
        return combinedBytes.ToArray();
    }
    public byte[] Encrypt(byte[] plainBytes, int nonceSizeInBytes, string contextFileName, 
        bool writeToFile = false, string? outputPath = null, string? inputFile = null)
    {
        //Setup
        const int bitsPerByte = 8;
        const int keySize = 256;
        IBlockCipher cipher = new AesEngine();
        var macSize = bitsPerByte * cipher.GetBlockSize();
        var nonce = GenerateNonce(nonceSizeInBytes);
        var aadData = GenerateAAD(contextFileName);
        var keyGen = new CipherKeyGenerator();
        keyGen.Init(new KeyGenerationParameters(new SecureRandom(), keySize));
        var keyParam = keyGen.GenerateKeyParameter();
        Key = keyParam;
        var keyParamAead = new AeadParameters(keyParam, macSize, nonce, aadData);
        var cipherMode = new GcmBlockCipher(cipher);
        cipherMode.Init(true, keyParamAead);
        //Encryption
        var encryptedBytes = new byte[cipherMode.GetOutputSize(plainBytes.Length)];
        if (!writeToFile)
        {
            var processData = cipherMode.ProcessBytes(plainBytes, 0, plainBytes.Length,
                encryptedBytes, 0);
            return appendAADDataBytes(encryptedBytes, nonce, aadData);
        }

        if (inputFile == null || outputPath == null) return [0];
        using var fileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
        using var outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
        const int byteBuffer = 1048576;
        var buffer = new byte[byteBuffer];
        int bytesRead;
        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
        {
            var outputBuffer = new byte[cipherMode.GetOutputSize(bytesRead)];
            cipherMode.ProcessBytes(buffer, 0, bytesRead, outputBuffer, 0);
            cipherMode.DoFinal(outputBuffer, bytesRead);
            outputStream.Write(outputBuffer, 0, outputBuffer.Length);
        }
        return [1];
    }
}
