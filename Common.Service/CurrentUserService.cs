using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Common.Service
{
    public class CurrentUserService:ICurrentUserSerice
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
