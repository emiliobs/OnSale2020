using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onsale.Common.Entities;
using OnSale.Web.Data;
using OnSale.Web.Helpers;
using OnSale.Web.Models;

namespace OnSale.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DataContext _context;
        private readonly IBlobHelper _blobHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;

        public ProductsController(DataContext context, IBlobHelper blobHelper, ICombosHelper combosHelper, IConverterHelper converterHelper)
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

        public IActionResult Create()
        {
            var model = new ProductViewModel
            {
                Categories = _combosHelper.GetComboCategories(),
                IsActive = true,

            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var product = await _converterHelper.ToProductAsync(model, true);

                    if (model.ImageFile != null)
                    {
                        var imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "products");
                        product.ProductImages = new List<ProductImage>
                        {
                            new ProductImage{ ImageId = imageId }
                        };
                    }

                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            model.Categories = _combosHelper.GetComboCategories();

            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(p => p.Category)
                                                 .Include(p => p.ProductImages)
                                                 .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToProductViewModel(product);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var product = await _converterHelper.ToProductAsync(model, false);

                    if (model.ImageFile == null)
                    {
                        var imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "products");
                        if (product.ProductImages == null)
                        {
                            product.ProductImages = new List<ProductImage>();
                        }
                        product.ProductImages.Add(new ProductImage { ImageId = imageId });
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same Name.");
                    }
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }

            model.Categories = _combosHelper.GetComboCategories();
            return View(model);
        }
    }
}
