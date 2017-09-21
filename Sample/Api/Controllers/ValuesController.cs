using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            var claims = from c in User.Claims
                         select new { c.Type, c.Value };

            return new JsonResult(claims);
        }
    }
}
