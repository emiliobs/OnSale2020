using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onsale.Common.Entities;
using Onsale.Common.Enums;
using OnSale.Web.Data;
using OnSale.Web.Helpers;
using OnSale.Web.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnSale.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly DataContext _context;

        public AccountController(IUserHelper userHelper, DataContext context, ICombosHelper combosHelper, IBlobHelper blobHelper)
        {
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;
            _context = context;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(loginViewModel);

                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return RedirectToAction(Request.Query["RetuenUrl"].First());
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Email or Password incorrect.");
            }

            return View(loginViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            AddUserViewModel model = new AddUserViewModel
            {
                //Countries = _combosHelper.GetComboCountries(),
                //Departments = _combosHelper.GetComboDepartments(0),
                //Cities = _combosHelper.GetComboCities(0),

                Countries = _combosHelper.GetComboCountries(),
                Departments = _combosHelper.GetComboDepartments(),
                Cities = _combosHelper.GetComboCities(),

            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(AddUserViewModel addUserViewModel)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (addUserViewModel.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(addUserViewModel.ImageFile, "users");
                }

                Data.Entities.User user = await _userHelper.AddUserAsync(addUserViewModel, imageId, UserType.User);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "This email is already used.");
                    addUserViewModel.Countries = _combosHelper.GetComboCountries();
                    addUserViewModel.Departments = _combosHelper.GetComboDepartments(addUserViewModel.CountryId);
                    addUserViewModel.Cities = _combosHelper.GetComboCities(addUserViewModel.CountryId);
                    return View(addUserViewModel);
                }

                //Here login to page with the new datas of user:
                LoginViewModel loginViewModel = new LoginViewModel
                {
                    Password = addUserViewModel.Password,
                    RememberMe = false,
                    Username = addUserViewModel.Username,
                };

                Microsoft.AspNetCore.Identity.SignInResult resulLogin = await _userHelper.LoginAsync(loginViewModel);
                if (resulLogin.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

            }

            addUserViewModel.Countries = _combosHelper.GetComboCountries();
            addUserViewModel.Departments = _combosHelper.GetComboDepartments(addUserViewModel.CountryId);
            addUserViewModel.Cities = _combosHelper.GetComboCities(addUserViewModel.CountryId);
            return View(addUserViewModel);
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }

        // two methods JsonResult for call ajax from the views:
        public JsonResult GetDepartments(int countryId)
        {
            Country country = _context.Countries
                .Include(c => c.Departments)
                .FirstOrDefault(c => c.Id == countryId);
            if (country == null)
            {
                return null;
            }

            return Json(country.Departments.OrderBy(d => d.Name));

        }

        public JsonResult GetCities(int departmentId)
        {
            Department department = _context.Departments
                .Include(d => d.Cities)
                .FirstOrDefault(d => d.Id == departmentId);
            if (department == null)
            {
                return null;
            }
            return Json(department.Cities.OrderBy(d => d.Name));
        }

    }
}
