using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Onsale.Common.Enums;
using OnSale.Web.Data;
using OnSale.Web.Data.Entities;
using OnSale.Web.Models;
using System;
using System.Threading.Tasks;

namespace OnSale.Web.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly DataContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserHelper(DataContext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager,
                          SignInManager<User> signInManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<User> AddUserAsync(AddUserViewModel addUserViewModel, Guid imageId, UserType userType)
        {
            var user = new User
            {
                Address = addUserViewModel.Address,
                City = await _context.Cities.FindAsync(addUserViewModel.CityId),
                Document = addUserViewModel.Document,
                Email = addUserViewModel.Username,
                FirstName = addUserViewModel.FirstName,
                LastName = addUserViewModel.LastName,
                ImageId = imageId,
                UserName = addUserViewModel.Username,
                UserType = userType,
                PhoneNumber = addUserViewModel.PhoneNumber,

            };

            var result = await _userManager.CreateAsync(user, addUserViewModel.Password);
            if (result != IdentityResult.Success)
            {
                return null;
            }

            var newUser = await GetUserAsync(addUserViewModel.Username);
            await AddUserToRoleAsync(newUser, user.UserType.ToString());

            return newUser;
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string olsPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, olsPassword, newPassword);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            bool roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName,
                });
            }
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<User> GetUserAsync(string email)
        {
            return await _context.Users.Include(u => u.City).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            return await _context.Users.Include(u => u.City).FirstOrDefaultAsync(u => u.Id == userId.ToString());
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel loginViewModel)
        {
            return await _signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password,
                                                             loginViewModel.RememberMe, false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<SignInResult> ValidatePasswordAsync(User user, string password)
        {
            return await _signInManager.CheckPasswordSignInAsync(user, password, false);
        }
    }
}
