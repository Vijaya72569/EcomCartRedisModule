using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAPIJWT.Models;

namespace UserAPIJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    var products = new List<Models.Product>
        //    {
        //        //new Models.Product { Id = 1, Url = "https://localhost:7067/Images/image12.jpg", Price = 400 },
        //        //new Models.Product { Id = 2, Url = "https://localhost:7067/Images/image13.jpg", Price = 500 },
        //        //new Models.Product { Id = 3, Url = "https://localhost:7067/Images/image13.jpg", Price = 400 },
        //        //new Models.Product { Id = 4, Url = "https://localhost:7067/Images/image14.jpg", Price = 500 },
        //        //new Models.Product { Id = 5, Url = "https://localhost:7067/Images/image15.jpg", Price = 600 },
        //        //new Models.Product { Id = 6, Url = "https://localhost:7067/Images/image16.jpg", Price = 900 },
        //        //new Models.Product { Id = 7, Url = "https://localhost:7067/Images/image17.jpg", Price = 700 },
        //        //new Models.Product { Id = 8, Url = "https://localhost:7067/Images/image18.jpg", Price = 800 },
        //        //new Models.Product { Id = 9, Url = "https://localhost:7067/Images/image19.jpg", Price = 900 },
        //        //new Models.Product { Id = 10, Url = "https://localhost:7067/Images/image20.jpg", Price = 1000 }

            //        //-------------------modify url https -> http------------------------


            //         new Models.Product { Id = 1, Url = "http://localhost:5072/Images/image12.jpg", Price = 400 },
            //        new Models.Product { Id = 2, Url = "http://localhost:5072/Images/image13.jpg", Price = 500 },
            //        new Models.Product { Id = 3, Url = "http://localhost:5072/Images/image13.jpg", Price = 400 },
            //        new Models.Product { Id = 4, Url = "http://localhost:5072/Images/image14.jpg", Price = 500 },
            //        new Models.Product { Id = 5, Url = "http://localhost:5072/Images/image15.jpg", Price = 600 },
            //        new Models.Product { Id = 6, Url = "http://localhost:5072/Images/image16.jpg", Price = 900 },
            //        new Models.Product { Id = 7, Url = "http://localhost:5072/Images/image17.jpg", Price = 700 },
            //        new Models.Product { Id = 8, Url = "http://localhost:5072/Images/image18.jpg", Price = 800 },
            //        new Models.Product { Id = 9, Url = "http://localhost:5072/Images/image19.jpg", Price = 900 },
            //        new Models.Product { Id = 10, Url = "http://localhost:5072/Images/image20.jpg", Price = 1000 }
            //    };
            //    return Ok(products);
            //}


             private static List<Models.Product> products = new List<Models.Product>
        {
            new Models.Product { Id = 1, Url = "http://localhost:5072/Images/image12.jpg", Price = 400 },
            new Models.Product { Id = 2, Url = "http://localhost:5072/Images/image13.jpg", Price = 500 },
            new Models.Product { Id = 3, Url = "http://localhost:5072/Images/image20.jpg", Price = 400 },
            new Models.Product { Id = 4, Url = "http://localhost:5072/Images/image14.jpg", Price = 500 },
            new Models.Product { Id = 5, Url = "http://localhost:5072/Images/image15.jpg", Price = 600 },
            new Models.Product { Id = 6, Url = "http://localhost:5072/Images/image16.jpg", Price = 900 },
            new Models.Product { Id = 7, Url = "http://localhost:5072/Images/image17.jpg", Price = 700 },
            new Models.Product { Id = 8, Url = "http://localhost:5072/Images/image18.jpg", Price = 800 },
            new Models.Product { Id = 9, Url = "http://localhost:5072/Images/image19.jpg", Price = 900 },
            new Models.Product { Id = 10, Url = "http://localhost:5072/Images/image20.jpg", Price = 1000 }
        };

        // GET all products
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(products);
        }
        // GET product by ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound("Product not found");

            return Ok(product);
        }
    }
}
