using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MoviesAPI.Services
{
    public class ServiceBase
    {
        protected readonly IHttpContextAccessor _httpContext;

        public ServiceBase(IHttpContextAccessor httpContext)
        {
            ResetNow();
            _httpContext = httpContext;
        }

        public Func<DateTime> Now { get; private set; } = () => DateTime.Now;

        public void SetNow(DateTime now) => Now = () => now;

        public void ResetNow() => Now = () => DateTime.Now;

        public string GetUsername() => _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Name);

        public IEnumerable<string> GetUserRoles()
        {
            return _httpContext.HttpContext.User.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray();
        }
    }
}