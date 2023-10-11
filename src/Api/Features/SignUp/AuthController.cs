using Api.Features.SignUp;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public partial class AuthController
{
    [HttpPost("SignUp", Name = nameof(SignUp))]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response.Value);
    }
}
