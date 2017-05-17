using System.Threading.Tasks;
using System.Web.Http;

namespace WalletService.Controllers
{
    public class UserController : ApiController
    {
        [Route("api/user/create")]
        [HttpGet]
        public async Task<IHttpActionResult> CreateUser()
        {
            return Ok("ok");
        }
    }
}