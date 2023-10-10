using FluentResults;
using MediatR;

namespace Api.Features.SignUp;

public class SignUpHandler : IRequestHandler<SignUpRequest, Result<SignUpResponse>>
{
    private readonly IUserNameIsUniqueRepository _userNameIsUniqueRepository;
    private readonly IEmailIsUniqueRepository _emailIsUniqueRepository;
    private readonly ICreateUserRepository _createUserRepository;

    public SignUpHandler(
        IUserNameIsUniqueRepository userNameIsUniqueRepository,
        IEmailIsUniqueRepository emailIsUniqueRepository,
        ICreateUserRepository createUserRepository)
    {
        _userNameIsUniqueRepository = userNameIsUniqueRepository;
        _emailIsUniqueRepository = emailIsUniqueRepository;
        _createUserRepository = createUserRepository;
    }

    public async Task<Result<SignUpResponse>> Handle(SignUpRequest request, CancellationToken cancellationToken)
    {
        var userNameIsUnique = await _userNameIsUniqueRepository.UserNameIsUniqueAsync(request.UserName, cancellationToken);
        if (userNameIsUnique is false)
            return Result.Fail(ErrorMessages.UserNameIsTaken);

        var emailIsUnique = await _emailIsUniqueRepository.EmailIsUniqueAsync(request.Email, cancellationToken);
        if (emailIsUnique is false)
            return Result.Fail(ErrorMessages.EmailIsTaken);

        var user = await _createUserRepository.CreateUserAsync(request.UserName, request.Email, request.Password, cancellationToken);
        var response = new SignUpResponse { Id = user.Id, UserName = user.UserName, Email = user.Email };
        return Result.Ok(response);
    }
}
