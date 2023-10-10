namespace Api.Features.SignUp;

public interface IEmailIsUniqueRepository
{
    Task<bool> EmailIsUniqueAsync(string email, CancellationToken cancellationToken);
}