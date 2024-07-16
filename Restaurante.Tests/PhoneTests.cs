using Xunit;
using Restaurante.Api.Functions;
namespace Restaurante.Tests;

public class TestPhone
{
    [Fact]
    public void ValidateAndFormatPhoneNumber_UnformattedPhoneNumber_ValidPhoneNumber_True()
    {

        string phoneNumber = "21999999999";
        string phoneExpected = "+5521999999999";

        bool result = Phone.VerifyPhone(phoneNumber, out string newPhone);

        Assert.Equal(phoneExpected, newPhone);
    }
}