using Api.Features.SignUp;
using Api.IntegrationTests.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;

namespace Api.IntegrationTests.Features.SignUp;

public class SignUpTests : ApiTests
{
    public SignUpTests(ApiWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task SignUp_WithValidCredentials_ReturnsSuccess()
    {
        var signUpRequest = new SignUpRequest
        {
            Email = "example@domain.com",
            UserName = "exampleUser",
            Password = "ExampleUser123!"
        };

        var response = await ApiClient.PostAsJsonAsync("Auth/SignUp", signUpRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var signUpResponse = await response.Content.ReadFromJsonAsync<SignUpResponse>();
        signUpResponse!.Id.Should().NotBeEmpty();
        signUpResponse.Email.Should().Be(signUpRequest.Email);
        signUpResponse.UserName.Should().Be(signUpRequest.UserName);
        var users = await DbContext.Users.ToListAsync();
        users.Should().HaveCount(1);
        var user = users.First();
        user.Id.Should().Be(signUpResponse.Id.ToString());
    }
}
