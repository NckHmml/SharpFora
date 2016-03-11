using Microsoft.AspNet.Identity;
using SharpFora.DAL;
using SharpFora.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Data.Entity;
using System.Security.Cryptography;

namespace SharpFora.Security
{
    public class UserStore : 
        IUserStore<User>, 
        IUserTwoFactorStore<User>,
        IUserSecurityStampStore<User>,
        IUserRoleStore<User>
    {
        /// <summary>
        /// Random generator used for creating user secrets
        /// </summary>
        private static RandomNumberGenerator Random { get; } = RandomNumberGenerator.Create();

        private SharpForaContext DbContext { get; set; }

        public UserStore(SharpForaContext dbContext)
        {
            DbContext = dbContext;
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken) =>
            Task.FromResult(user.Id.ToString());

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken) =>
            Task.FromResult(user.Name);

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            user.Name = userName;
            return UpdateAsync(user, cancellationToken);
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken) =>
            GetUserNameAsync(user, cancellationToken);

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken) =>
            SetUserNameAsync(user, normalizedName, cancellationToken);

        public Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            DbContext.Users.Add(user);
            DbContext.SaveChanges();
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            DbContext.Update(user);
            DbContext.SaveChanges();
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            DbContext.Users.Remove(user);
            DbContext.SaveChanges();
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            int id;
            if (!int.TryParse(userId, out id))
                return null;

            return DbContext.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken) =>
            DbContext.Users.FirstOrDefaultAsync(x => x.Name == normalizedUserName, cancellationToken);
        
        public Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        {
            if (enabled)
            {
                var secret = new byte[10];
                Random.GetBytes(secret);
                user.TokenSecret = secret;
            }
            else
            {
                user.TokenSecret = new byte[0];
            }
            return UpdateAsync(user, cancellationToken);
        }

        public Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancellationToken) =>
            Task.FromResult(user.TokenSecret.Length > 0);

        public Task SetSecurityStampAsync(User user, string stamp, CancellationToken cancellationToken)
        {
            user.SecurityStamp = stamp;
            return UpdateAsync(user, cancellationToken);
        }

        public Task<string> GetSecurityStampAsync(User user, CancellationToken cancellationToken) =>
            Task.FromResult(user.SecurityStamp);

        public Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            Role role = DbContext.Roles.FirstOrDefault(x => x.Name == roleName);
            // Check if role exists
            if (role == null)
                return Task.FromResult(0);
            // Check if user already in role
            if(user.Roles.Any(x => x.Name == roleName))
                return Task.FromResult(0);

            DbContext.UserRoles.Add(new UserRole
            {
                RoleId = role.Id,
                UserId = user.Id
            });
            DbContext.SaveChanges();

            return Task.FromResult(0);
        }

        public Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            Role role = DbContext.Roles.FirstOrDefault(x => x.Name == roleName);
            // Check if role exists
            if (role == null)
                return Task.FromResult(0);
            // Check if user in role
            if (!user.Roles.Any(x => x.Name == roleName))
                return Task.FromResult(0);

            DbContext.UserRoles.Remove(new UserRole
            {
                RoleId = role.Id,
                UserId = user.Id
            });
            DbContext.SaveChanges();

            return Task.FromResult(0);
        }

        public Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken) =>
            Task.FromResult<IList<string>>(user.Roles.Select(x => x.Name).ToList());

        public Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken) =>
            Task.FromResult(user.Roles.Any(x => x.Name == roleName));

        public Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken) =>
            Task.FromResult<IList<User>>(
                DbContext.UserRoles
                .Where(x => x.Role.Name == roleName)
                .Select(x => x.User)
                .ToList()
            );
    }
}
