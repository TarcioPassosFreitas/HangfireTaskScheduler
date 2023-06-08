using Microsoft.AspNetCore.Mvc;

namespace HangfireTaskScheduler.API.Controllers;

public abstract class BaseController : ControllerBase
{
    protected ObjectResult InternalServerError(string message)
    {
        var response = new
        {
            Message = message
        };

        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    protected ObjectResult BadRequest(string message)
    {
        var response = new
        {
            Message = message
        };

        return StatusCode(StatusCodes.Status400BadRequest, response);
    }

    protected ObjectResult NotFound(string message)
    {
        var response = new
        {
            Message = message
        };

        return StatusCode(StatusCodes.Status404NotFound, response);
    }
}