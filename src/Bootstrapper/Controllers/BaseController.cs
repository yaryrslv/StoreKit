using Microsoft.AspNetCore.Mvc;

namespace StoreKit.Bootstrapper.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BaseController : ControllerBase
    {
    }
}