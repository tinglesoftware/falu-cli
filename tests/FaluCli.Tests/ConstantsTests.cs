using Xunit;

namespace Falu.Tests;

public class ConstantsTests
{
    [Fact]
    public void MaxMpesaStatementFileSizeString_IsCorrect()
    {
        Assert.Equal("128 KiB", Constants.MaxMpesaStatementFileSizeString);
    }
}
