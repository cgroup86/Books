using Books.BL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            try
            {
                User user = new User();
                return user.Read();
            }
            catch (Exception ex)
            {
                return new List<User>();
            }
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] User user)
        {
            try
            {
                user.Register();
                return Ok(new { message = "User registered successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred: " + ex.Message });
            }
        }


        [HttpPost("Login")]
        public IActionResult Login([FromBody] User user)
        {
            try
            {
                user.Login();
                return Ok(new { id = user.Id, message = "Logged in successfully", name = user.Name, isActive = user.IsActive, isAdmin = user.IsAdmin });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred: " + ex.Message });
            }
        }

        [HttpPut("UpdateUserValuesById/user Id/{userId}/Is Active/{isActive}")]
        public IActionResult UpdateCourseIsActiveAndTitle(int userId, bool isActive)
        {
            try
            {
                User user = new User();
                user.UpdateUserValues(userId, isActive);
                return Ok(new { message = "Course with id: " + userId + " isActive status and name has been updated successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // ----------------------------------------------------------------------------------------------

        //// GET: api/<UsersController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<UsersController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<UsersController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<UsersController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<UsersController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}