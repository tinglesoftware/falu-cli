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
    [InlineData("fskl_602cd2747409e867a240d000")]
    [InlineData("fpkt_60ffe3f79c1deb8060f91312")]
    [InlineData("fskt_27e868O6xW4NYrQb3WvxDb8iW6D")]
    public void ApiKeyFormat_IsCorrect(string input)
    {
        Assert.Matches(Constants.ApiKeyFormat, input);
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
    [InlineData("5C5F6FC9-DD6A-4C5D-A63A-DC96740CFE12")]
    [InlineData("dad11640-8f1c-4b91-8e61-051241204c8f")]
    [InlineData("pay:reload:202205091300")]
    [InlineData("pay_reload:202205091300")]
    public void IdempotencyKeyFormat_IsCorrect(string input)
    {
        Assert.Matches(Constants.IdempotencyKeyFormat, input);
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

    [Theory]
    [InlineData("mtpl_602cd2747409e867a240d000")]
    [InlineData("tmpl_60ffe3f79c1deb8060f91312")]
    [InlineData("mtpl_27e868O6xW4NYrQb3WvxDb8iW6D")]
    public void MessageTemplateIdFormat_IsCorrect(string input)
    {
        Assert.Matches(Constants.MessageTemplateIdFormat, input);
    }

    [Theory]
    [InlineData("promo-message")]
    [InlineData("promo_message")]
    [InlineData("Birthday_Wishes_2022-05-10")]
    public void MessageTemplateAliasFormat_IsCorrect(string input)
    {
        Assert.Matches(Constants.MessageTemplateAliasFormat, input);
    }

    [Theory]
    [InlineData("+254722000000")]
    [InlineData("+254700000000")]
    [InlineData("+14155552671")]
    public void E164PhoneNumberFormat_IsCorrect(string input)
    {
        Assert.Matches(Constants.E164PhoneNumberFormat, input);
    }
}
