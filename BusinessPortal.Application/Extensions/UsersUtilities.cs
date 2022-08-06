using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using BusinessPortal.Application.StaticTools;
using BusinessPortal.Domain.Entities.Account;

namespace BusinessPortal.Application.Extensions
{
    public static class UsersUtilities
    {
        public static string GetUserAvatar(this User user)
        {
            return !string.IsNullOrEmpty(user.Avatar) ? PathTools.UserAvatarPath + user.Avatar : PathTools.DefaultUserAvatar;
        }

        public static string GetFullName(this User user)
        {
            return user.Username;
        }
    }
}
