using System.Text;
using FsCheck;
using FsCheck.Xunit;
using DecenKeep;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;


namespace Testing;


public class AESTests
{
    public class AesCryptoServiceTests
    {
        private readonly byte[] _iv = new byte[12];
        private readonly SecureRandom _random = new SecureRandom();

        public AesCryptoServiceTests()
        {
            _random.NextBytes(_iv);
        }

        [Property]
        public Property EncryptDecryptShouldReturnOriginalData()
        {
            return Prop.ForAll<string, string>((plainText, context) =>
            {
                // Arrange
                if (string.IsNullOrEmpty(plainText) || string.IsNullOrEmpty(context))
                {
                    return true; // Skip the test for empty inputs.
                }

                var plainBytes = Encoding.UTF8.GetBytes(plainText);
                var aesCryptoService = new AesCryptoService(_iv);

                // Act
                var encryptedBytes = aesCryptoService.Encrypt(plainBytes, context);
                var decryptedBytes = aesCryptoService.Decrypt(encryptedBytes, aesCryptoService.Key.GetKey());

                // Assert
                var decryptedText = Encoding.UTF8.GetString(decryptedBytes);
                return plainText == decryptedText;
            });
        }

        [Property]
        public Property EncryptDecryptFileShouldReturnOriginalData()
        {
            return Prop.ForAll<string, string>((fileContent, context) =>
            {
                // Arrange
                if (string.IsNullOrEmpty(fileContent) || string.IsNullOrEmpty(context))
                {
                    return true; // Skip the test for empty inputs.
                }

                var plainBytes = Encoding.UTF8.GetBytes(fileContent);
                var aesCryptoService = new AesCryptoService(_iv);
                var inputFile = "input.txt";
                var encryptedFile = "encrypted.txt";
                var decryptedFile = "decrypted.txt";

                try
                {
                    // Write plain text to input file
                    File.WriteAllBytes(inputFile, plainBytes);

                    // Act
                    var encryptResult = aesCryptoService.EncryptFile(inputFile, encryptedFile, context);
                    var decryptResult = aesCryptoService.DecryptFile(encryptedFile, decryptedFile, aesCryptoService.Key.GetKey());

                    // Read decrypted text from decrypted file
                    var decryptedBytes = File.ReadAllBytes(decryptedFile);
                    var decryptedText = Encoding.UTF8.GetString(decryptedBytes);

                    // Assert
                    return encryptResult && decryptResult && fileContent == decryptedText;
                }
                finally
                {
                    // Clean up temporary files
                    if (File.Exists(inputFile)) File.Delete(inputFile);
                    if (File.Exists(encryptedFile)) File.Delete(encryptedFile);
                    if (File.Exists(decryptedFile)) File.Delete(decryptedFile);
                }
            });
        }
    }
}
