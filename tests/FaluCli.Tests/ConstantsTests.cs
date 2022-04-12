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
    [InlineData("wksp_60ffe3f79c1deb8060f91312")]
    [InlineData("wksp_27e868O6xW4NYrQb3WvxDb8iW6D")]
    public void WorkspaceIdFormat_IsCorrect(string input)
    {
        Assert.Matches(Constants.WorkspaceIdFormat, input);
    }

    [Theory]
    [InlineData("evt_602cd2747409e867a240d000")]
    [InlineData("evt_60ffe3f79c1deb8060f91312")]
    [InlineData("evt_27e868O6xW4NYrQb3WvxDb8iW6D")]
    public void EventIdFormat_IsCorrect(string input)
    {
        Assert.Matches(Constants.EventIdFormat, input);
    }

    [Theory]
    [InlineData("we_602cd2747409e867a240d000")]
    [InlineData("we_60ffe3f79c1deb8060f91312")]
    [InlineData("we_27e868O6xW4NYrQb3WvxDb8iW6D")]
    public void WebhookEndpointIdFormat_IsCorrect(string input)
    {
        Assert.Matches(Constants.WebhookEndpointIdFormat, input);
    }
}
