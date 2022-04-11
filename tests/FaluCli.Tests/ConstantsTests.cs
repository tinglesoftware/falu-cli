using Xunit;

namespace Falu.Tests;

public class ConstantsTests
{
    [Fact]
    public void MaxMpesaStatementFileSizeString_IsCorrect()
    {
        Assert.Equal("128 KiB", Constants.MaxMpesaStatementFileSizeString);
    }

    [Theory]
    [InlineData("wksp_602cd2747409e867a240d000")]
    [InlineData("tr_60ffe3f79c1deb8060f91312")]
    [InlineData("mtmpl_27e868O6xW4NYrQb3WvxDb8iW6D")]
    public void ObjectIdFormat_IsCorrect(string input)
    {
        Assert.Matches(Constants.ObjectIdFormat, input);
    }
}
