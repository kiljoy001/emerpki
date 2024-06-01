using System;
using Xunit;
using FsCheck;
using FsCheck.Xunit;
using Org.BouncyCastle.Pqc.Crypto.Crystals.Kyber;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using System.Linq;

namespace DecenKeep.Tests
{
    public static class CustomGenerators
    {
        public static Arbitrary<byte[]> ByteArray()
        {
            var byteArrayGen = 
                from size in Gen.Choose(1, 100) // Array size between 1 and 100
                from bytes in Gen.ArrayOf(size, Gen.Choose(0, 255).Select(i => (byte)i))
                select bytes;
            return Arb.From(byteArrayGen);
        }
    }
    public class KyberCryptoServicesTests
    {
        [Property]
        public Property KyberEncryptionDecryption()
        {
            // Arrange
            var kyberService = new KyberCryptoServices();
            var keyPair = kyberService.GenerateKyberKeys();
            var publicKey = (KyberPublicKeyParameters)keyPair.Public;
            var privateKey = (KyberPrivateKeyParameters)keyPair.Private;

            // Define the property
            Func<byte[], bool> property = (data) =>
            {
                // Act
                var (cipherText, secret) = kyberService.EncryptData(publicKey);
                var decryptedSecret = kyberService.DecryptData(cipherText, privateKey);

                // Assert
                return secret.SequenceEqual(decryptedSecret);
            };

            // Create arbitrary data for testing
            return Prop.ForAll(Arb.Default.ByteArray().Filter(bytes => bytes.Length > 0), property);
        }

        [Fact]
        public void GenerateKyberKeys_ShouldGenerateValidKeyPair()
        {
            // Arrange
            var kyberService = new KyberCryptoServices();

            // Act
            var keyPair = kyberService.GenerateKyberKeys();

            // Assert
            Assert.NotNull(keyPair);
            Assert.NotNull(keyPair.Public);
            Assert.NotNull(keyPair.Private);
            Assert.IsType<KyberPublicKeyParameters>(keyPair.Public);
            Assert.IsType<KyberPrivateKeyParameters>(keyPair.Private);
        }
    }
}