using CleanArchitecture.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CleanArchitecture.Infrastructure.Authorization
{
    public sealed class RoleAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string role;
        private readonly IUserRoleRepository userRoleRepository;

        public RoleAttribute(string role, IUserRoleRepository userRoleRepository)
        {
            this.role = role;
            this.userRoleRepository = userRoleRepository;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var userHasRole = userRoleRepository.Where(u => u.UserId == userIdClaim.Value)
                .Include(p => p.Role)
                .Any(p => p.Role.Name == role); 

            if (!userHasRole)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
