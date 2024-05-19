using FsCheck;
using FsCheck.Xunit;
using emerpki;
using Xunit;

namespace Testing;


public class EccKeyTests
{
    [Property]
    public void GeneratedKeyPairsAreNotNull(NonNull<string> cid)
    {
        var ecckeys = new AESCryptoService(cid.Item);
        var keypair = ecckeys.GenerateEccKeyPair();
        Assert.NotNull(keypair.Public);
        Assert.NotNull(keypair.Private);
        
    }
    /*[Property]
    public void GeneratedKeyPairEncryptDecrypt(NonNull<string> cid, NonNull<string> testMessage)
    {
        var ecckeys = new EccKeys(cid.Item);
        var keypair = ecckeys.GenerateEccKeyPair();
        var encryptedMessage = 
    } */
    
}