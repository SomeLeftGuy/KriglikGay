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
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationContext _db;
        public HomeController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }
        public async Task<IActionResult> Index(string filter = null)
        {
            if (_roleManager.Roles.Count() == 0)
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Dark"));
            }
            HomeView[] refs;
            if (filter == null)
            {
                refs = _db.Companies.Where(item => Convert.ToDateTime(item.endDate) > DateTime.Now).Select(item => new HomeView
                {
                    Ref = item.id,
                    EndDate = Convert.ToDateTime(item.endDate),
                    CollectedSum = item.colectedSum,
                    Sum = item.sum,
                    Image = item.image,
                    Name = item.name
                }).ToArray();
            }else
            {
                filter = filter.Replace("  ", " ");
                string[] tagsString = filter.Split(" ");
                for (int i = 0; i < tagsString.Length-1; i++)
                {
                    for (int j = i+1; j < tagsString.Length; j++)
                    {
                        if (tagsString[i] == tagsString[j])
                        {
                            tagsString[j] = null;
                        }
                    }
                }
                Tag[] tags = new Tag[tagsString.Length];
                List<HomeView> companies = new List<HomeView>();
                for (int i = 0; i < tags.Length; i++)
                {
                    tags[i] = _db.Tag.FirstOrDefault(item => item.name == tagsString[i]);
                    if (tags[i]!=null)
                    {
                        companies.AddRange(_db.TagsToCompanies.Where(item => item.tag == tags[i] && Convert.ToDateTime(item.company.endDate) > DateTime.Now).Select(item => new HomeView
                        {
                            Ref = item.company.id,
                            EndDate = Convert.ToDateTime(item.company.endDate),
                            CollectedSum = item.company.colectedSum,
                            Sum = item.company.sum,
                            Image = item.company.image,
                            Name = item.company.name,
                            Rating = 0
                        }));
                    }
                }
                List<HomeView> result = new List<HomeView>();
                for (int i = 0; i < companies.Count; i++)
                {
                    bool duplicate = false;
                    for (int z = 0; z < i; z++)
                    {
                        if (companies[z].Ref == companies[i].Ref)
                        {
                            duplicate = true;
                            break;
                        }
                    }
                    if (!duplicate)
                    {
                        result.Add(companies[i]);
                    }
                }
                refs = result.ToArray();
            }
            for (int i = 0; i < refs.Length; i++)
            {
                int[] marks = _db.Marks.Where(item => item.companyId == refs[i].Ref).Select(item => item.value).ToArray();
                if (marks != null)
                {
                    double rating = 0;
                    double fullRaiting = marks.Length;
                    for (int j = 0; j < marks.Length; j++)
                    {
                        rating += marks[j] + 1;
                    }
                    rating /= marks.Length;
                    fullRaiting *= rating;
                    refs[i].Rating = Math.Round(rating, 2);
                    refs[i].FullRating = fullRaiting;
                }else
                {
                    refs[i].FullRating = 0;
                    refs[i].Rating = 0;
                }
            }
            for (int i = 0; i < refs.Length-1; i++)
            {
                for (int j = i + 1; j < refs.Length; j++)
                {
                    if (refs[i].FullRating < refs[j].FullRating)
                    {
                        HomeView buf = refs[i];
                        refs[i] = refs[j];
                        refs[j] = buf;
                    }
                }
            }
            return View(new HomeViewModel { Companies = refs });
        }
    }
}
