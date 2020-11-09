using CoursesMain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesMain.Filters
{
    public class UserFilter : Attribute, IAsyncAuthorizationFilter
    {
        private readonly UserManager<User> _userManager;
        public UserFilter(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)  {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
            }else
            {
                bool check = await _userManager.GetLockoutEnabledAsync(await _userManager.GetUserAsync(context.HttpContext.User));
                if (check)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "LogOff" }));
                }
            }
        }
    }
}
