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
    public class AdminFilter : Attribute, IAsyncAuthorizationFilter
    {
        private readonly UserManager<User> _userManager;
        public AdminFilter(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            User user = await _userManager.GetUserAsync(context.HttpContext.User);
            bool check = await _userManager.IsInRoleAsync(user, "Admin");
            if (!check)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
        }
    }
}
