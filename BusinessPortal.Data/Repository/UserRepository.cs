using BusinessPortal.Data.DbContext;
using BusinessPortal.Domain.Entities.Account;
using BusinessPortal.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        #region Ctor

        public BusinessPortalDbContext _context;

        public UserRepository(BusinessPortalDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Site Side

        public async Task<User?> GetUserByMobile(string Mobile)
        {
            return await _context.Users.FirstOrDefaultAsync(p => !p.IsDelete && p.Mobile == Mobile);
        }

        public async Task<bool> IsExistUserById(ulong userId)
        {
            return await _context.Users.AnyAsync(p => !p.IsDelete && p.Id == userId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Admin Side

        #endregion

        #region User Panel

        public async Task EditUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
