using Books.BL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersScoresController : ControllerBase
    {

        // POST api/<UsersScoresController>
        [HttpPut]
        public IActionResult Put([FromBody] UserScore userScore)
        {
            try
            {
                //var status = userScore.SubmitScore();
                return Ok(userScore.SubmitScore());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred: " + ex.Message });
            }
        }

        [HttpGet("GetUserAndTopScores/userId/{id}")]
        public IActionResult GetUserAndTopScores(int id)
        {
            try
            {
                return Ok(UserScore.GetUserAndTopScores(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred: " + ex.Message });
            }
        }

        //// GET: api/<UsersScoresController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<UsersScoresController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<UsersScoresController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<UsersScoresController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<UsersScoresController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
