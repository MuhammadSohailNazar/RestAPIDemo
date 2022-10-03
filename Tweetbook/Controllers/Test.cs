using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweetbook.Controllers
{
    public class Test : Controller
    {
        [HttpGet("api/user")]
        public IActionResult Index()
        {
            return Ok(new { name = "sohail" });
        }
    }
}
