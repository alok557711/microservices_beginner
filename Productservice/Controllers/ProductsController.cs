using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Productservice_2.Models;
using RestEase ;
using Steeltoe.Common.Discovery;

namespace Productservice_2.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/Products")]
    public class ProductsController : Controller
    {
        private readonly Productservice_2Context _context;

        public ProductsController(Productservice_2Context context,IDiscoveryClient discoveryclient,IConfiguration configuration)
        {
            _context = context;
            var handler = new DiscoveryHttpClientHandler(discoveryclient);
            var httpclient = new HttpClient(handler, false)
            {
                BaseAddress=new Uri(configuration.GetValue<string>("userurl"))
            };
            RestClient.For<HttpClient>(httpclient);
        }

        // GET: api/Products
        [HttpGet]
        public IEnumerable<Product> GetProduct()
        {
            return _context.Product;
        }

        [HttpGet("{customername}")]
        public async Task<IActionResult> GetProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Product.SingleOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product product)
        {

            try
            {
                Productservice_2.Controllers.RabbitMQClient c = new Productservice_2.Controllers.RabbitMQClient();
                c.sendDetail(product);
                c.close();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
            return Ok(product);

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //_context.Product.Add(product);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Product.SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}