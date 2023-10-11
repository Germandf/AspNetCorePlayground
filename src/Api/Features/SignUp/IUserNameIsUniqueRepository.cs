using Microsoft.AspNetCore.Identity;

namespace Api.Features.SignUp;

public interface IUserNameIsUniqueRepository
{
    Task<bool> UserNameIsUniqueAsync(string userName, CancellationToken cancellationToken);
}

public class UserNameIsUniqueRepository : IUserNameIsUniqueRepository
{
    private readonly UserManager<IdentityUser> _userManager;
    public UserNameIsUniqueRepository(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<bool> UserNameIsUniqueAsync(string userName, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(userName);
        return user is null;
    }
}
