namespace Api.Features.SignUp;

public interface IUserNameIsUniqueRepository
{
    Task<bool> UserNameIsUniqueAsync(string userName, CancellationToken cancellationToken);
}
