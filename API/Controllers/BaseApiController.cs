using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ApiException), StatusCodes.Status500InternalServerError)]
public class BaseApiController : ControllerBase
{
}
