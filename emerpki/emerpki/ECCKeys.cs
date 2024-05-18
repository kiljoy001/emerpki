using System.Text;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;


namespace emerpki;

public class EccKeys
{
    private SecureRandom _seed;
    private AsymmetricCipherKeyPair _keypair;

    private void InitializeSeed(string cid, string? akKeyString = null, string? blockchainHash = null)
    {
        //Create a random number unsigned int to the cid
        var random = new SecureRandom();
        var rounds = (uint)(random.NextInt() & 0x7FFFFFFF);
        //Convert to bytes
        var roundsBytes = BitConverter.GetBytes(rounds);
        var cidBytes = Encoding.UTF8.GetBytes(cid);
        
        //Concatenation of byte arrays
        List<byte> combinedByteList = new List<byte>();
        combinedByteList.AddRange(cidBytes);
        combinedByteList.AddRange(roundsBytes);
        if (akKeyString != null)
        {
            var akTPMKey = Encoding.UTF8.GetBytes(akKeyString);
            combinedByteList.AddRange(akTPMKey);
        }

        if (blockchainHash != null)
        {
            var blockHash = Encoding.UTF8.GetBytes(blockchainHash);
            combinedByteList.AddRange(blockHash);
        }

        var combinedBytes = combinedByteList.ToArray();
       
        _seed = new SecureRandom();
        _seed.SetSeed(combinedBytes);
    }
    
    public EccKeys(string cid)
    {
        InitializeSeed(cid);
    }

    public EccKeys(string cid, string akKey)
    {
        InitializeSeed(cid, akKey);
    }

    public EccKeys(string cid, string akKey, string blockHash)
    {
        InitializeSeed(cid, akKey, blockHash);
    }

    public AsymmetricCipherKeyPair GenerateEccKeyPair()
    {
        //Curve Parameters
        X9ECParameters curveParameters = SecNamedCurves.GetByName("secp256r1");
        ECDomainParameters ecSpec = new ECDomainParameters(curveParameters.Curve, curveParameters.G, curveParameters.N,
            curveParameters.H, curveParameters.GetSeed());
        
        //Generate Key Pair
        ECKeyPairGenerator generator = new ECKeyPairGenerator();
        ECKeyGenerationParameters kgenParameters = new ECKeyGenerationParameters(ecSpec, _seed);
        generator.Init(kgenParameters);
        AsymmetricCipherKeyPair keyPair = generator.GenerateKeyPair();
        _keypair = keyPair;
        return keyPair;
    }

    public bool EncryptData(Stream data)
    {
        if (_keypair == null)
        {
            GenerateEccKeyPair();
        }
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