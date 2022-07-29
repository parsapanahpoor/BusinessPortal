﻿using BusinessPortal.Application.Services.Interfaces;
using BusinessPortal.Application.StaticTools;
using BusinessPortal.Data.DbContext;
using BusinessPortal.Domain.Entities.Account;
using BusinessPortal.Domain.ViewModels.Access;
using BusinessPortal.Domain.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Application.Services.Implementation
{
    public class PermissionService : IPermissionService
    {
        #region Ctor

        public IUserService _userService { get; set; }
        public BusinessPortalDbContext _context { get; set; }

        public PermissionService(BusinessPortalDbContext context, IUserService userServie)
        {
            _context = context;
            _userService = userServie;
        }

        #endregion

        #region Check Permission

        public async Task<bool> HasUserPermission(ulong userId, string permissionName)
        {
            // get user
            var user = await _userService.GetUserById(userId);

            // check user exists
            if (user == null) return false;

            // admin user access any where
            if (user.IsAdmin) return true;

            // check user access not limited
            if (!user.IsEmailConfirm || user.IsBan)
            {
                return false;
            }

            // get permission from permission list
            var permission = PermissionsList.Permissions.FirstOrDefault(s => s.PermissionUniqueName == permissionName);

            // check permission exists
            if (permission == null) return false;

            // get user roles
            var userRoles = await _context.UserRoles
                .Where(s => s.UserId == userId && !s.IsDelete)
                .ToListAsync();

            // check user has any roles
            if (!userRoles.Any()) return false;

            // get user role Ids list
            var userRoleIds = userRoles.Select(s => s.RoleId).ToList();

            // check user has permission
            var result = await _context.RolePermissions.AnyAsync(s =>
                s.PermissionId == permission.Id && userRoleIds.Contains(s.RoleId) && !s.IsDelete);

            return result;
        }


        #endregion

        #region Role

        public async Task<List<SelectListViewModel>> GetSelectRolesList()
        {
            return await _context.Roles.Where(s => !s.IsDelete).Select(s => new SelectListViewModel
            {
                Id = s.Id,
                Title = s.Title
            }).ToListAsync();
        }

        public async Task<bool> IsRoleNameValid(string name, ulong roleId)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(s => s.RoleUniqueName.Equals(name.Trim().ToLower()));

            if (role == null) return true;

            if (roleId != 0 && role.Id == roleId)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> CreateRole(CreateRoleViewModel create)
        {
            if (!await IsRoleNameValid(create.RoleUniqueName, 0))
            {
                return false;
            }

            // add role
            var role = new Role
            {
                RoleUniqueName = create.RoleUniqueName,
                Title = create.Title
            };

            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();

            // add permissions
            if (create.Permissions != null && create.Permissions.Any())
            {
                foreach (var permissionId in create.Permissions)
                {
                    var rolePermission = new RolePermission
                    {
                        PermissionId = permissionId,
                        RoleId = role.Id
                    };

                    await _context.AddAsync(rolePermission);
                }
                await _context.SaveChangesAsync();
            }

            return true;
        }


        public async Task<FilterRolesViewModel> FilterRoles(FilterRolesViewModel filter)
        {
            var query = _context.Roles.Where(s => !s.IsDelete).AsQueryable();

            #region Filter

            if (!string.IsNullOrEmpty(filter.RoleTitle))
            {
                query = query.Where(s => s.Title.Contains(filter.RoleTitle));
            }

            if (!string.IsNullOrEmpty(filter.RoleUniqueName))
            {
                query = query.Where(s => s.RoleUniqueName.Contains(filter.RoleUniqueName));
            }

            #endregion

            await filter.Paging(query);

            return filter;
        }

        public async Task<EditRoleViewModel> FillEditRoleViewModel(ulong roleId)
        {
            var role = await GetRoleById(roleId);

            if (role == null)
            {
                return null;
            }

            var permissionIds = await _context.RolePermissions.Where(s => !s.IsDelete && s.RoleId == roleId)
                .Select(s => s.PermissionId).ToListAsync();

            var result = new EditRoleViewModel
            {
                Id = roleId,
                RoleUniqueName = role.RoleUniqueName,
                Title = role.Title,
                Permissions = permissionIds
            };

            return result;
        }

        public async Task<Role?> GetRoleById(ulong roleId)
        {
            return await _context.Roles.FirstOrDefaultAsync(s => s.Id == roleId && !s.IsDelete);
        }

        public async Task<EditRoleResult> EditRole(EditRoleViewModel edit)
        {
            var role = await GetRoleById(edit.Id);

            if (role == null)
            {
                return EditRoleResult.RoleNotFound;
            }

            if (!await IsRoleNameValid(edit.RoleUniqueName, edit.Id))
            {
                return EditRoleResult.UniqueNameExists;
            }

            role.Title = edit.Title;
            role.RoleUniqueName = edit.RoleUniqueName;

            _context.Update(role);
            await _context.SaveChangesAsync();

            // remove all permissions
            var rolePermissions =
                await _context.RolePermissions.Where(s => !s.IsDelete && s.RoleId == edit.Id).ToListAsync();
            _context.RolePermissions.RemoveRange(rolePermissions);
            await _context.SaveChangesAsync();

            // add permissions
            if (edit.Permissions != null && edit.Permissions.Any())
            {
                foreach (var permissionId in edit.Permissions)
                {
                    var rolePermission = new RolePermission
                    {
                        PermissionId = permissionId,
                        RoleId = role.Id
                    };

                    await _context.AddAsync(rolePermission);
                }
                await _context.SaveChangesAsync();
            }

            return EditRoleResult.Success;
        }

        public async Task<bool> DeleteRole(ulong roleId)
        {
            var role = await GetRoleById(roleId);

            if (role == null)
            {
                return false;
            }

            role.IsDelete = true;

            _context.Update(role);
            await _context.SaveChangesAsync();

            // remove all permissions
            var rolePermissions =
                await _context.RolePermissions.Where(s => !s.IsDelete && s.RoleId == roleId).ToListAsync();
            _context.RolePermissions.RemoveRange(rolePermissions);
            await _context.SaveChangesAsync();

            return true;
        }


        #endregion
    }
}
