using Microsoft.AspNetCore.Identity;

namespace Api.Features.SignUp;

public interface ICreateUserRepository
{
    Task<User> CreateUserAsync(string userName, string email, string password, CancellationToken cancellationToken);
}

public class CreateUserRepository : ICreateUserRepository
{
    private readonly UserManager<IdentityUser> _userManager;

    public CreateUserRepository(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<User> CreateUserAsync(string userName, string email, string password, CancellationToken cancellationToken)
    {
        var user = new IdentityUser { UserName = userName, Email = email };
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            throw new Exception($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
        return new User { Id = user.Id, UserName = user.UserName, Email = user.Email };
    }
}