using SpeakMate.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace SpeakMate.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
