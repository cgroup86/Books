using Books.BL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {


        // POST api/<ValuesController>
        [HttpPost]
        public int Post([FromBody] Category category)
        {
            try
            {
                return category.insertCategory();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        //// GET: api/<CategoriesController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<CategoriesController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<CategoriesController>
        //[HttpPost]
        //public int Post([FromBody] Category category)
        //{
        //    //return category.Insert();
        //    return 1;
        //}

        //// PUT api/<CategoriesController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<CategoriesController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
