using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnSale.Web.Data;
using OnSale.Web.Helpers;

namespace OnSale.Web.Controllers
{
    public class Products : Controller
    {
        private readonly DataContext _context;
        private readonly IBlobHelper _blobHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;

        public Products(DataContext context, IBlobHelper blobHelper, ICombosHelper combosHelper, IConverterHelper converterHelper)
        {
            _context = context;
            _blobHelper = blobHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.Include(p => p.Category).Include(p => p.ProductImages).ToListAsync());
        }
    }
}
