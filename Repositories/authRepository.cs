using System.Runtime.InteropServices.ComTypes;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Linq.Expressions;
using Api.Data;
using Api.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class AuthRepository
    {

        private readonly ApiContext _api;

        public AuthRepository(ApiContext api)
        {
            _api = api;
        }

        public async Task<User> Login(User user)
        {
            try
            {
                var auth = await _api.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName && u.Password == user.Password);
                return auth;
            }
            catch (System.Exception)
            {
                return null;

            }
        }

    }
}