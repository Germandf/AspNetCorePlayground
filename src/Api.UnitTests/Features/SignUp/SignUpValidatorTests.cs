using Api.Features.SignUp;
using Api.UnitTests.Common;
using FluentAssertions;

namespace Api.UnitTests.Features.SignUp;

public class SignUpValidatorTests
{
    private SignUpRequest _validRequest = new()
    {
        Email = "some.address@domain.com",
        Password = "Password1!",
        UserName = "someUserName"
    };
    private readonly SignUpValidator _sut = new();

    [Theory]
    [InlineData(null, ErrorMessages.UserNameIsEmpty)]
    [InlineData("", ErrorMessages.UserNameIsEmpty)]
    [InlineData(" ", ErrorMessages.UserNameIsEmpty)]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", ErrorMessages.UserNameIsTooLong)]
    [InlineData("a!", ErrorMessages.UserNameContainsInvalidCharacters)]
    public void UserNameIsInvalid_ValidationError(string userName, string errorMessage)
    {
        var invalidRequest = _validRequest with { UserName = userName };

        var result = _sut.Validate(invalidRequest);

        result.ShouldContainErrorMessage(errorMessage);
    }

    [Theory]
    [InlineData(null, ErrorMessages.EmailIsEmpty)]
    [InlineData("", ErrorMessages.EmailIsEmpty)]
    [InlineData(" ", ErrorMessages.EmailIsEmpty)]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa@domain.com", ErrorMessages.EmailIsTooLong)]
    [InlineData("some.example", ErrorMessages.EmailIsInvalid)]
    public void EmailIsInvalid_ValidationError(string email, string errorMessage)
    {
        var invalidRequest = _validRequest with { Email = email };

        var result = _sut.Validate(invalidRequest);

        result.ShouldContainErrorMessage(errorMessage);
    }

    [Theory]
    [InlineData(null, ErrorMessages.PasswordIsEmpty)]
    [InlineData("", ErrorMessages.PasswordIsEmpty)]
    [InlineData(" ", ErrorMessages.PasswordIsEmpty)]
    [InlineData("A1!aaaa", ErrorMessages.PasswordIsTooShort)]
    [InlineData("A1!aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", ErrorMessages.PasswordIsTooLong)]
    [InlineData("withoutuppercase1!", ErrorMessages.PasswordMustBeValid)]
    [InlineData("WITHOUTLOWERCASE1!", ErrorMessages.PasswordMustBeValid)]
    [InlineData("WithoutNumber!", ErrorMessages.PasswordMustBeValid)]
    [InlineData("WithoutSpecialCharacter1", ErrorMessages.PasswordMustBeValid)]

    public void PasswordIsEmpty_ValidationError(string password, string errorMessage)
    {
        var invalidRequest = _validRequest with { Password = password };

        var result = _sut.Validate(invalidRequest);

        result.ShouldContainErrorMessage(errorMessage);
    }

    [Fact]
    public void RequestIsValid_ValidationSuccess()
    {
        var result = _sut.Validate(_validRequest);

        result.Errors.Should().BeEmpty();
    }
}
