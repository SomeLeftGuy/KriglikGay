using CoursesMain.Filters;
using CoursesMain.Models;
using CoursesMain.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesMain.Controllers
{
    [TypeFilter(typeof(UserFilter))]
    public class NewsController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly UserManager<User> _userManager;
        public NewsController(UserManager<User> userManager, ApplicationContext db)
        {
            _db = db;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> AddNews(ShowCompanyViewModel model)
        {
            Company company = _db.Companies.FirstOrDefault(item => item.id == model.Id);
            if (company != null)
            {
                _db.News.Add(new News
                {
                    company = company,
                    name = model.addNews.Name,
                    description = model.addNews.Description,
                    image = model.addNews.Image
                });
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(model.Id+"", "Company");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteNews(ShowCompanyViewModel model)
        {
            _db.News.Remove(_db.News.FirstOrDefault(item => item.id == model.deleteNewsID));
            await _db.SaveChangesAsync();
            return RedirectToAction(model.Id + "", "Company");
        }
    }
}
