using Microsoft.AspNetCore.Mvc;
using Books.BL;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<Author> Get(int page = 1, int pageSize = 10)
        {
            Author author = new Author();
            var totalCount = author.Read().Count();
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            var productsPerPage = author.Read().Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return productsPerPage;
            //return author.Read();
        }

        ////// GET api/<ValuesController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //POST api/<ValuesController>
        [HttpPost]
        public int Post([FromBody] Author author)
        {
            return author.Insert();
        }


        // PUT api/<ValuesController>/5
        [HttpPut("InsertImages/AuthorName/{name}/AuthorImage/{image}/Description/{description}")]

        public int PutImagesOfAuthors(string name, string image, string description)
        {
            Author author = new Author();
            return author.InsertImagesOfAuthor(name, image, description);
        }

        //// DELETE api/<ValuesController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}