using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnSale.Web.Data;
using OnSale.Web.Helpers;
using OnSale.Web.Models;
using System;
using System.Threading.Tasks;

namespace OnSale.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public CategoriesController(DataContext dataContext, IBlobHelper blobHelper, IConverterHelper converterHelper)
        {
            _dataContext = dataContext;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Categories.ToListAsync());
        }

        public IActionResult Create()
        {
            CategoryViewModel model = new CategoryViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "categories");
                }

                try
                {
                    Onsale.Common.Entities.Category category = _converterHelper.ToCatogory(model, imageId, true);
                    _dataContext.Add(category);
                    await _dataContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {

                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same Name.");
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

            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _dataContext.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToCategoryViewModel(category);
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var imageId = Guid.Empty;

                if (model.ImageId != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "categories");

                }

                try
                {
                    var category = _converterHelper.ToCatogory(model, imageId, false);
                    _dataContext.Update(category);
                    await _dataContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same Name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.Message);
                    }
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _dataContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            try
            {
                _dataContext.Categories.Remove(category);
                await _dataContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
