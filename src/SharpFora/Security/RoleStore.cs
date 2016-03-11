using Microsoft.AspNet.Identity;
using SharpFora.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using SharpFora.DAL;
using Microsoft.Data.Entity;

namespace SharpFora.Security
{
    public class RoleStore : IRoleStore<Role>
    {
        private SharpForaContext DbContext { get; set; }

        public RoleStore(SharpForaContext dbContext)
        {
            DbContext = dbContext;
        }

        public Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            DbContext.Roles.Add(role);
            DbContext.SaveChanges();
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            DbContext.Roles.Remove(role);
            DbContext.SaveChanges();
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            int id;
            if (!int.TryParse(roleId, out id))
                return null;

            return DbContext.Roles.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken) =>
            DbContext.Roles.FirstOrDefaultAsync(x => x.Name == normalizedRoleName, cancellationToken);

        public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken) =>
            GetRoleNameAsync(role, cancellationToken);

        public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken) =>
            Task.FromResult(role.Id.ToString());

        public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken) =>
            Task.FromResult(role.Name);

        public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken) =>
            SetRoleNameAsync(role, normalizedName, cancellationToken);

        public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return UpdateAsync(role, cancellationToken);
        }

        public Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            DbContext.Update(role);
            DbContext.SaveChanges();
            return Task.FromResult(IdentityResult.Success);
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
