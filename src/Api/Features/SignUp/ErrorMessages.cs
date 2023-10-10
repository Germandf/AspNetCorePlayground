namespace Api.Features.SignUp;

public partial class ErrorMessages
{
    public const string UserNameIsEmpty = "User name is empty";
    public const string UserNameIsTooLong = "User name is too long";
    public const string UserNameContainsInvalidCharacters = "User name contains invalid characters";
    public const string EmailIsEmpty = "Email is empty";
    public const string EmailIsTooLong = "Email is too long";
    public const string EmailIsInvalid = "Email is invalid";
    public const string PasswordIsEmpty = "Password is empty";
    public const string PasswordIsTooShort = "Password is too short";
    public const string PasswordIsTooLong = "Password is too long";
    public const string PasswordMustBeValid = "Password must be valid";
    public const string UserNameIsTaken = "User name is taken";
    public const string EmailIsTaken = "User email is taken";
}
