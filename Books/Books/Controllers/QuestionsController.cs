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
            try
            {
                return Ok(Question.getQuestion1());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred: " + ex.Message });
            }
        }

        [HttpGet("GetQuestion2")]
        public IActionResult GetQuestion2()
        {
            try
            {
                return Ok(Question.getQuestion2());

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred: " + ex.Message });
            }
        }

        [HttpGet("GetQuestion3")]
        public IActionResult GetQuestion3()
        {
            try
            {
                return Ok(Question.getQuestion3());

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred: " + ex.Message });
            }
        }

        [HttpGet("GetQuestion4")]
        public IActionResult GetQuestion4()
        {
            try
            {
                return Ok(Question.getQuestion4());

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred: " + ex.Message });
            }
        }

        [HttpGet("GetQuestion5")]
        public IActionResult GetQuestion5()
        {
            try
            {
                return Ok(Question.getQuestion5());

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred: " + ex.Message });
            }
        }

    }
}