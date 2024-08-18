using Books.BL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class questionsController : ControllerBase
    {

        [HttpGet("GetQuestion1")]
        public IActionResult GetQuestion1()
        {
            return Ok(Question.getQuestion1());
        }

        [HttpGet("GetQuestion2")]
        public IActionResult GetQuestion2()
        {
            return Ok(Question.getQuestion2());
        }



    }
}