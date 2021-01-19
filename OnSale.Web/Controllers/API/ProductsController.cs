using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnSale.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnSale.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            return Ok(_context.Products.Include(p => p.Category).Include(p => p.ProductImages).OrderBy(p => p.Name).ToList());
        }
    }
}
