﻿using Microsoft.AspNetCore.Identity;
using Onsale.Common.Enums;
using OnSale.Web.Data.Entities;
using OnSale.Web.Models;
using System;
using System.Threading.Tasks;

namespace OnSale.Web.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(User user, string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);

        Task<SignInResult> LoginAsync(LoginViewModel loginViewModel);

        Task LogoutAsync();

        Task<SignInResult> ValidatePasswordAsync(User user, string password);

        Task<User> AddUserAsync(AddUserViewModel addUserViewModel, Guid imageId, UserType userType);
    }
}
