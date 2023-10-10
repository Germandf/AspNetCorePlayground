using FluentResults;
using MediatR;

namespace Api.Features.SignUp;

public record SignUpRequest : IRequest<Result<SignUpResponse>>
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}