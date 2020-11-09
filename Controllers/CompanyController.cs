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
    public class CompanyController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly UserManager<User> _userManager;
        public CompanyController(UserManager<User> userManager, ApplicationContext db)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IActionResult> AddCompany()
        {
            User user = await _userManager.GetUserAsync(User);
            _db.Companies.Add(new Company {
                user = user,
                name = "Simple name",
                createDate = DateTime.Now.ToString("MM/dd/yyyy"),
                colectedSum = "0",
                theme = _db.Themes.FirstOrDefault(),
                sum = "0" });
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Profile");
        }
        public async Task<IActionResult> DeleteCompany(ProfileViewModel model)
        {
            for (int i = 0; i < model.companies.Length; i++)
            {
                if (model.companies[i].selected)
                {
                    Company company = _db.Companies.FirstOrDefault(item => item.id == model.companies[i].id);
                    _db.Companies.Remove(company);
                    await _db.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index", "Profile");
        }
        [HttpGet]
        public IActionResult EditCompany(ProfileViewModel model)
        {
            for (int i = 0; i < model.companies.Length; i++)
            {
                if (model.companies[i].selected)
                {
                    Company company = _db.Companies.FirstOrDefault(item => item.id == model.companies[i].id);
                    string[] themes = _db.Themes.Select(item => item.name).ToArray();
                    string[] tags = _db.TagsToCompanies.Where(item => item.company == company).Select(item => item.tag.name).ToArray();
                    string tag="";
                    foreach (string buf in tags)
                    {
                        tag += buf+" ";
                    }
                    return View(new CompanyViewModel {
                        id = company.id,
                        CompanyInfo = company.description,
                        CompanyName = company.name,
                        EndDate = company.endDate,
                        Sum = company.sum,
                        Themes = themes,
                        Tags = tag,
                        CompanyLinkImage = company.image,
                        CompanyLinkVideo = company.video });
                }
            }
            return RedirectToAction("Index", "Profile");
        }
        [HttpPost]
        public async Task<IActionResult> EditCompany(CompanyViewModel model)
        {
            Company company = _db.Companies.FirstOrDefault(item => item.id == model.id);
            if (_db.TagsToCompanies.FirstOrDefault(item => item.company == company)!=null)
                _db.TagsToCompanies.RemoveRange(_db.TagsToCompanies.Where(item => item.company == company));
            await _db.SaveChangesAsync();
            company.name = model.CompanyName;
            company.description = model.CompanyInfo;
            company.sum = model.Sum;
            company.endDate = model.EndDate;
            company.theme = _db.Themes.FirstOrDefault(item => item.name == model.Theme);
            company.image = model.CompanyLinkImage;
            company.video = model.CompanyLinkVideo;
            if (model.Tags != null)
            {
                model.Tags = model.Tags.Replace("  ", " ");
                string[] tags = model.Tags.Split(" ");
                _db.Companies.Update(company);
                for (int i = 0; i < tags.Length; i++)
                {
                    if (tags[i] != "")
                    {
                        Tag tag = _db.Tag.FirstOrDefault(item => item.name == tags[i]);
                        if (tag == null)
                        {
                            _db.Tag.Add(new Tag { name = tags[i] });
                            await _db.SaveChangesAsync();
                            _db.TagsToCompanies.Add(new TagsToCompany { company = company, tag = _db.Tag.FirstOrDefault(item => item.name == tags[i]) });
                        }
                        else
                        {
                            _db.TagsToCompanies.Add(new TagsToCompany { company = company, tag = tag });
                        }
                    }
                }
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Profile");
        }
        [Route("Company/{id:int}")]
        public async Task<IActionResult> Index(int id)
        {
            Company company = _db.Companies.FirstOrDefault(item => item.id == id);
            if (company != null)
            {
                User user = await _userManager.GetUserAsync(User);
                Mark mark = _db.Marks.FirstOrDefault(item => item.company == company && item.user == user);
                int[] marksAll = _db.Marks.Where(item => item.company == company).Select(item => item.value).ToArray();
                double marksRes = 0;
                if (marksAll.Length > 0)
                {
                    for (int i = 0; i < marksAll.Length; i++)
                    {
                        marksRes += marksAll[i]+1;
                    }
                    marksRes /= marksAll.Length;
                }
                marksRes = Math.Round(marksRes, 2);
                bool[] marks = new bool[5];
                if (mark != null)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (i <= mark.value)
                            marks[i] = true;
                        else
                            marks[i] = false;
                    }
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        marks[i] = false;
                    }
                }
                string theme = _db.Themes.FirstOrDefault(item => item.id == company.themeId).name;
                string userName = _userManager.Users.FirstOrDefault(item => item.Id == company.userId).UserName;

                NewsView[] news = _db.News.Where(item => item.company == company)
                    .Select(item => new NewsView { Name = item.name, Description = item.description, Image = item.image, id = item.id }).ToArray();

                CommentsView[] comments = _db.Comments.Where(item => item.company == company)
                    .Select(item => new CommentsView { id = item.id, Author = item.user.UserName, Text = item.text, Rating = 0 }).ToArray();

                BonusView[] bonuses = _db.Bonus.Where(item => item.company == company)
                    .Select(item => new BonusView { Name = item.name, Description = item.description, Sum = item.sum, id = item.id }).ToArray();
                for (int i = 0; i < comments.Length; i++)
                {
                    foreach(bool rating in _db.Ratings.Where(item => item.commentId == comments[i].id).Select(item => item.value))
                    {
                        if (rating)
                            comments[i].Rating++;
                        else
                            comments[i].Rating--;
                    }
                }

                return View(new ShowCompanyViewModel {
                    Id = company.id,
                    Name = company.name,
                    Theme = theme,
                    Image = company.image,
                    Description = company.description,
                    Video = company.video,
                    mark = marks,
                    marks = marksRes,
                    User = userName,
                    news = news,
                    addNews = new NewsView(),
                    comments = comments,
                    bonuses = bonuses,
                    addBonus = new BonusView()
                });

            }
            else return View("NotFound");
        }
        [HttpPost]
        public async Task<IActionResult> SetMark(ShowCompanyViewModel model)
        {
            User user = await _userManager.GetUserAsync(User);
            Company company = _db.Companies.FirstOrDefault(item => item.id == model.Id);
            Mark mark = _db.Marks.FirstOrDefault(item => item.company == company && item.user == user);
            for (int i = 4; i >= 0; i--)
            {
                if (model.mark[i]==true)
                {
                    if (mark == null)
                    {
                        _db.Marks.Add(new Mark { company = company, user = user, value = i });
                        await _db.SaveChangesAsync();
                        return RedirectToAction(model.Id + "", "Company");
                    }else
                    {
                        mark.value = i;
                        _db.Marks.Update(mark);
                        await _db.SaveChangesAsync();
                        return RedirectToAction(model.Id + "", "Company");
                    }
                }
            }
            if (mark != null)
            {
                _db.Marks.Remove(mark);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(model.Id + "", "Company");
        }
    }
}
