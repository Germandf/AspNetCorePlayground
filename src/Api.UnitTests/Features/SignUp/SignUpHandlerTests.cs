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
    private readonly SignUpRequest _request;
    private readonly User _user;

    public SignUpHandlerTests()
    {
        _userNameIsUniqueRepository = Substitute.For<IUserNameIsUniqueRepository>();
        _emailIsUniqueRepository = Substitute.For<IEmailIsUniqueRepository>();
        _createUserRepository = Substitute.For<ICreateUserRepository>();
        _sut = new SignUpHandler(_userNameIsUniqueRepository, _emailIsUniqueRepository, _createUserRepository);
        _request = _fixture.Create<SignUpRequest>();
        _userNameIsUniqueRepository
            .UserNameIsUniqueAsync(_request.UserName, default)
            .Returns(true);
        _emailIsUniqueRepository
            .EmailIsUniqueAsync(_request.Email, default)
            .Returns(true);
        _user = _fixture.Create<User>();
        _createUserRepository
            .CreateUserAsync(_request.UserName, _request.Email, _request.Password, default)
            .Returns(_user);
    }

    [Fact]
    public async Task UserNameIsTaken_ReturnsError()
    {
        _userNameIsUniqueRepository
            .UserNameIsUniqueAsync(_request.UserName, default)
            .Returns(false);

        var response = await _sut.Handle(_request, default);

        response.IsFailed.Should().BeTrue();
        response.Errors.Should().Contain(x => x.Message == ErrorMessages.UserNameIsTaken);
    }

    [Fact]
    public async Task EmailIsTaken_ReturnsError()
    {
        _emailIsUniqueRepository
            .EmailIsUniqueAsync(_request.Email, default)
            .Returns(false);

        var response = await _sut.Handle(_request, default);

        response.IsFailed.Should().BeTrue();
        response.Errors.Should().Contain(x => x.Message == ErrorMessages.EmailIsTaken);
    }

    [Fact]
    public async Task RequestIsValid_ReturnsUserResponse()
    {
        var response = await _sut.Handle(_request, default);

        response.IsSuccess.Should().BeTrue();
        response.Value.Id.Should().Be(_user.Id);
        response.Value.UserName.Should().Be(_user.UserName);
        response.Value.Email.Should().Be(_user.Email);
    }
}
