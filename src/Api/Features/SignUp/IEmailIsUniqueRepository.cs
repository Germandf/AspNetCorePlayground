using Microsoft.AspNetCore.Identity;

namespace Api.Features.SignUp;

public interface IEmailIsUniqueRepository
{
    Task<bool> EmailIsUniqueAsync(string email, CancellationToken cancellationToken);
}

public class EmailIsUniqueRepository : IEmailIsUniqueRepository
{
    private readonly UserManager<IdentityUser> _userManager;

    public EmailIsUniqueRepository(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> EmailIsUniqueAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user is null;
    }
}
