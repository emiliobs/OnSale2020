using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnSale.Web.Data;
using OnSale.Web.Data.Entities;
using System.Threading.Tasks;

namespace OnSale.Web.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly DataContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public UserHelper(DataContext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this._context = context;
            this._roleManager = roleManager;
            this._userManager = userManager;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName,
                });
            }
        }

        public async Task<User> GetUserAsync(string email)
        {
            return await _context.Users.Include(u => u.City).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }
    }
}
