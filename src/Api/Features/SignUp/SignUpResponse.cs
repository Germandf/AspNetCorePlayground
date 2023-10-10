namespace Api.Features.SignUp;

public class SignUpResponse
{
    public required Guid Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
}
