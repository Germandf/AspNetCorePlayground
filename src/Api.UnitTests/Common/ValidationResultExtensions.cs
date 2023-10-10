using FluentAssertions;
using FluentResults;
using FluentValidation.Results;

namespace Api.UnitTests.Common;

public static class ValidationResultExtensions
{
    public static void ShouldContainErrorMessage(this ValidationResult validationResult, string errorMessage)
    {
        validationResult.Errors.Should().Contain(x => x.ErrorMessage == errorMessage);
    }
}
