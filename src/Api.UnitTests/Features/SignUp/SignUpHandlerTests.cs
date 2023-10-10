using Api.Features.SignUp;
using AutoFixture;
using FluentAssertions;
using NSubstitute;

namespace Api.UnitTests.Features.SignUp;

public class SignUpHandlerTests
{
    private readonly Fixture _fixture = new();
    private readonly IUserNameIsUniqueRepository _userNameIsUniqueRepository;
    private readonly IEmailIsUniqueRepository _emailIsUniqueRepository;
    private readonly ICreateUserRepository _createUserRepository;
    private readonly SignUpHandler _sut;

    public SignUpHandlerTests()
    {
        _userNameIsUniqueRepository = Substitute.For<IUserNameIsUniqueRepository>();
        _emailIsUniqueRepository = Substitute.For<IEmailIsUniqueRepository>();
        _createUserRepository = Substitute.For<ICreateUserRepository>();
        _sut = new SignUpHandler(_userNameIsUniqueRepository, _emailIsUniqueRepository, _createUserRepository);
    }

    [Fact]
    public async Task UserNameIsTaken_ReturnsError()
    {
        var request = _fixture.Create<SignUpRequest>();
        _userNameIsUniqueRepository
            .UserNameIsUniqueAsync(request.UserName, default)
            .Returns(false);

        var response = await _sut.Handle(request, default);

        response.IsFailed.Should().BeTrue();
        response.Errors.Should().Contain(x => x.Message == ErrorMessages.UserNameIsTaken);
    }

    [Fact]
    public async Task EmailIsTaken_ReturnsError()
    {
        var request = _fixture.Create<SignUpRequest>();
        _userNameIsUniqueRepository
            .UserNameIsUniqueAsync(request.UserName, default)
            .Returns(true);
        _emailIsUniqueRepository
            .EmailIsUniqueAsync(request.Email, default)
            .Returns(false);

        var response = await _sut.Handle(request, default);

        response.IsFailed.Should().BeTrue();
        response.Errors.Should().Contain(x => x.Message == ErrorMessages.EmailIsTaken);
    }

    [Fact]
    public async Task RequestIsValid_ReturnsUserResponse()
    {
        var request = _fixture.Create<SignUpRequest>();
        _userNameIsUniqueRepository
            .UserNameIsUniqueAsync(request.UserName, default)
            .Returns(true);
        _emailIsUniqueRepository
            .EmailIsUniqueAsync(request.Email, default)
            .Returns(true);
        var user = _fixture.Create<User>();
        _createUserRepository
            .CreateUserAsync(request.UserName, request.Email, request.Password, default)
            .Returns(user);

        var response = await _sut.Handle(request, default);

        response.IsSuccess.Should().BeTrue();
        response.Value.Id.Should().Be(user.Id);
        response.Value.UserName.Should().Be(user.UserName);
        response.Value.Email.Should().Be(user.Email);
    }
}
