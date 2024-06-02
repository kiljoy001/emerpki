using CustomGenerators;

namespace Testing;
using FsCheck.Xunit;

using static ByteArrayGenerator;

public class CustomGeneratorsTest
{
    [Property]
    public bool GeneratedByteArraysShouldHaveCorrectLength(byte[] bytes)
    {
        // Test that the length is within the expected range
        return bytes.Length >= 1 && bytes.Length <= 100;
    }

    [Property]
    public bool GeneratedByteArraysShouldContainValidByteValues(byte[] bytes)
    {
        // Test that all values in the byte array are valid byte values
        return bytes.All(b => b >= 0 && b <= 255);
    }
}