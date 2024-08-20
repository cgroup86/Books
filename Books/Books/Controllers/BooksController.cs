using Books.BL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Books.Controllers
{
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        // POST api/<ValuesController>
        [HttpPost]
        public IActionResult Post([FromBody] Book book)
        {
            try
            {
                return Ok(book.InsertBook());
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/<BooksController>
        [HttpGet]
        public IActionResult Get(bool isEbook, int pageNumber, int pageSize, bool fetchTotalCount = false)
        {
            try
            {
                Book book = new Book();
                // Get the paged books and total record count
                int totalRecords = 0;
                List<Book> books = book.Read(isEbook, pageNumber, pageSize, out totalRecords, fetchTotalCount);

                // Create a response with pagination metadata
                var response = new
                {
                    IsEbook = isEbook,
                    TotalRecords = totalRecords,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Books = books
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                //return BadRequest(ex.Message);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        // GET: api/<BooksController>
        [HttpGet("get10BooksPerPage/{pageNumber}/{pageSize}")]
        public IActionResult Get(int pageNumber, int pageSize)
        {
            try
            {
                Book book = new Book();
                // Get the paged books and total record count
                var books = book.ReadAllBooks(pageNumber, pageSize);

                return Ok(books);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                //return StatusCode(500, "An unexpected error occurred.");
            }
        }




        [HttpPut("UpdateBookValuesById/book Id/{bookId}/Is Active/{isActive}/price/{newPrice}/NumberOfPrints/{numberOfPrints}")]
        public IActionResult UpdateCourseIsActiveAndTitle(int bookId, bool isActive, int newPrice, int numberOfPrints)
        {
            try
            {
                Book book = new Book();
                book.UpdateBooksValues(bookId, isActive, newPrice, numberOfPrints);
                return Ok(new { message = "Course with id: " + bookId + " isActive status and name has been updated successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        //// GET: api/<BooksController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<BooksController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<BooksController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<BooksController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<BooksController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
