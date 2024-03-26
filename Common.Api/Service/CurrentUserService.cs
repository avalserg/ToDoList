using System.Security.Claims;
using Common.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Common.Api.Service
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
