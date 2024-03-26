using Common.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Common.Service
{
    public class CurrentUserService:ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
           
        }
        public string CurrentUserId =>_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        public string[] CurrentUserRoles =>_httpContextAccessor.HttpContext.User.Claims.Where(c=>c.Type==ClaimTypes.Role).Select(c=>c.Value).ToArray();
    }
}
