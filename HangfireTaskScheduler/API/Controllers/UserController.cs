using Ardalis.Result;
using AutoMapper;
using HangfireTaskScheduler.API.Models.User;
using HangfireTaskScheduler.Core.Aggregate.UserAggregate;
using HangfireTaskScheduler.Core.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace HangfireTaskScheduler.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : BaseController
{
    [HttpPost("create")]
    [ProducesResponseType(typeof(UserResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UserResponseModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UserResponseModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
         [FromServices] IMapper mapper,
         [FromServices] IUserService userService,
         [FromBody] UserRequestModel request,
         CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Model state is not valid.");
        }

        var user = mapper.Map<User>(request);

        var result = await userService.AddAsync(user, cancellationToken);

        if (result.Status == ResultStatus.Invalid) return BadRequest(result.ValidationErrors);

        if (result.Status == ResultStatus.Error) return InternalServerError("An unexpected error occurred while creating the user.");

        var response = mapper.Map<User>(result.Value);

        return Ok(response);
    }
}