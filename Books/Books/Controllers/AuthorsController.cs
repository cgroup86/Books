using Microsoft.AspNetCore.Mvc;
using Books.BL;
using Microsoft.AspNetCore.Mvc.Formatters;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        //// GET: api/<ValuesController>
        //[HttpGet("get10AuthorsPerPage/{page}/{pageSize}")]
        //public IEnumerable<Author> Get(int page, int pageSize)
        //{
        //    Author author = new Author();
        //    var totalCount = author.Read().Count();
        //    var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
        //    var productsPerPage = author.Read().Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //    return productsPerPage;
        //    //return author.Read();
        //}

        [HttpGet("get10AuthorsPerPage/{page}/{pageSize}")]
        public IEnumerable<Author> Get(int page, int pageSize)
        {
            try
            {
                Author author = new Author();
                return author.ReadAuthorsByPage(page, pageSize);
            }
            catch (Exception ex)
            {
                return new List<Author>();
            }
        }


        ////// GET api/<ValuesController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //POST api/<ValuesController>
        [HttpPost]
        public IActionResult Post([FromBody] Author author)
        {
            try
            {
                var insert = author.Insert();
                return Ok(insert);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred: " + ex.Message });
            }
        }


        // PUT api/<ValuesController>/5
        [HttpPut("InsertImages/AuthorName/{name}/AuthorImage/{image}/Description/{description}")]

        public IActionResult PutImagesOfAuthors(string name, string image, string description)
        {
            try
            {
                Author author = new Author();
                return Ok(author.InsertImagesOfAuthor(name, image, description));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred: " + ex.Message });
            }
        }

        //// GET api/<ValuesController>/5
        [HttpGet("getBooksByAuthorsName/{authorName}")]
        public IEnumerable<Book> Get(string authorName)
        {
            try
            {
                Author author = new Author();
                return author.ReadBooksByAuthorName(authorName);
            }
            catch (Exception ex)
            {
                return new List<Book>();
            }
        }

        // GET: api/<BooksController>
        [HttpGet("getNumAuthorsInLibraries/{authorName}")]
        public IActionResult GetNumberOfAuthorName(string authorName)
        {
            try
            {
                Author author = new Author();
                var authors = author.getNumOfAuthorsInLibraries(authorName);

                return Ok(authors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                //return StatusCode(500, "An unexpected error occurred.");
            }
        }

        //// DELETE api/<ValuesController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}