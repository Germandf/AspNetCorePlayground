namespace Api.Features.SignUp;

public interface ICreateUserRepository
{
    Task<User> CreateUserAsync(string userName, string email, string password, CancellationToken cancellationToken);
}