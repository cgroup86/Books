using Books.BL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalLibrariesController : ControllerBase
    {

        // POST api/<PersonalLibrariesController>
        [HttpPost("AddToLibrary/{isEbook}")]
        public IActionResult AddToLibrary([FromBody] PersonalLibrary personalLibrary, bool isEbook)
        {
            if (personalLibrary.AddToLibrary())
            {
                return Ok(new { success = true, book = personalLibrary, isEbook = isEbook });
            }
            else
            {
                return Ok(new { success = false });
            };
        }


        [HttpPut("UpdateBookStatus/UserId/{userId}/BookId/{bookId}/NewStatus/{newStatus}")]
        public IActionResult UpdateBookStatus(int userId, int bookId, bool newStatus)
        {
            try
            {
                PersonalLibrary.UpdateBookStatus(userId, bookId, newStatus);
                return Ok(new { message = "Book with id: " + bookId + " status has been updated successfully" });
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

        [HttpGet("BooksToRead/UserId/{userId}")]
        public IActionResult GetBooksToRead(int userId)
        {
            try
            {
                var books = PersonalLibrary.GetBooksToRead(userId);
                return Ok(books);
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


        [HttpGet("BooksRead/UserId/{userId}")]
        public IActionResult GetBooksRead(int userId)
        {
            try
            {
                var books = PersonalLibrary.GetBooksRead(userId);
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("BooksPurchased/UserId/{userId}")]
        public IActionResult GetBooksPurchased(int userId)
        {
            try
            {
                var books = PersonalLibrary.GetBooksPurchased(userId);
                return Ok(books);
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

        [HttpPut("AddRemoveRequestToBuy/sellerId/{sellerId}/buyerId/{buyerId}/bookId/{bookId}")]
        public IActionResult AddRemoveRequestToBuy(int sellerId, int buyerId, int bookId)
        {
            try
            {
                PersonalLibrary.AddRemoveRequestToBuy(sellerId, buyerId, bookId);
                return Ok(1);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred: " + ex.Message });
            }
        }


        [HttpPut("AcceptRequestToBuy/sellerId/{sellerId}/buyerId/{buyerId}/bookId/{bookId}")]
        public IActionResult AcceptRequestToBuy(int sellerId, int buyerId, int bookId)
        {
            try
            {
                PersonalLibrary.AcceptRequestToBuy(sellerId, buyerId, bookId);
                return Ok(1);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred: " + ex.Message });
            }
        }

        [HttpPut("GetPurchasedBooksWithStatus1/userId/{userId}")]
        public IActionResult GetPurchasedBooksWithStatus1(int userId)
        {
            try
            {
                var obj = PersonalLibrary.GetPurchasedBooksWithStatus1(userId);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred: " + ex.Message });
            }
        }

        [HttpPut("GetRequestedBooksByBuyer/userId/{userId}")]
        public IActionResult GetRequestedBooksByBuyer(int userId)
        {
            try
            {
                var obj = PersonalLibrary.GetRequestedBooksByBuyer(userId);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred: " + ex.Message });
            }
        }
        //// GET: api/<PersonalLibrariesController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}-------

        //// GET api/<PersonalLibrariesController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<PersonalLibrariesController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<PersonalLibrariesController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<PersonalLibrariesController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
