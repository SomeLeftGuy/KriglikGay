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
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationContext _db;
        public ProfileController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }
        [TypeFilter(typeof(AdminFilter))]
        public async Task<IActionResult> UsersTable()
        {
            UsersViewModel model = new UsersViewModel();
            Dictionary<string, string> roles = new Dictionary<string, string>();
            foreach (User user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                    roles.Add(user.Id, "Admin");
                else
                    roles.Add(user.Id, "User");
            }
            model.UsersList = _userManager.Users
                .Select(item => new UserView { id = item.Id, Email = item.Email, Name = item.UserName, Type = roles[item.Id], Status = item.LockoutEnabled }).ToArray();            
            return View(model);
        }
        [HttpPost]
        [TypeFilter(typeof(AdminFilter))]
        public async Task<ActionResult> Delete(UsersViewModel model)
        {
            for (int i = 0; i < model.UsersList.Length; i++)
            {
                if (model.UsersList[i].Selected)
                {
                    User user = await _userManager.FindByIdAsync(model.UsersList[i].id);
                    await _userManager.DeleteAsync(user);
                }
            }
            return RedirectToAction("UsersTable", "Profile");
        }
        [HttpPost]
        [TypeFilter(typeof(AdminFilter))]
        public async Task<IActionResult> Block(UsersViewModel model)
        {
            for (int i = 0; i < model.UsersList.Length; i++)
            {
                if (model.UsersList[i].Selected)
                {
                    User user = await _userManager.FindByIdAsync(model.UsersList[i].id);
                    await _userManager.SetLockoutEnabledAsync(user, true);
                }
            }
            return RedirectToAction("UsersTable", "Profile");
        }
        [HttpPost]
        [TypeFilter(typeof(AdminFilter))]
        public async Task<IActionResult> UnBlock(UsersViewModel model)
        {
            for (int i = 0; i < model.UsersList.Length; i++)
            {
                if (model.UsersList[i].Selected)
                {
                    User user = await _userManager.FindByIdAsync(model.UsersList[i].id);
                    await _userManager.SetLockoutEnabledAsync(user, false);
                }
            }
            return RedirectToAction("UsersTable", "Profile");
        }
        [HttpPost]
        [TypeFilter(typeof(AdminFilter))]
        public async Task<IActionResult> Access(UsersViewModel model)
        {
            for (int i = 0; i < model.UsersList.Length; i++)
            {
                if (model.UsersList[i].Selected)
                {
                    User user = await _userManager.FindByIdAsync(model.UsersList[i].id);
                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        await _userManager.RemoveFromRoleAsync(user, "Admin");
                    }else
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                }
            }
            return RedirectToAction("UsersTable", "Profile");
        }
        [HttpPost]
        [TypeFilter(typeof(AdminFilter))]
        public async Task<IActionResult> Login(UsersViewModel model)
        {
            for (int i = 0; i < model.UsersList.Length; i++)
            {
                if (model.UsersList[i].Selected)
                {
                    User user = await _userManager.FindByIdAsync(model.UsersList[i].id);
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("UsersTable", "Profile");
        }
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.GetUserAsync(User);
            ProfileViewModel model = new ProfileViewModel();
            model.accessType = await _userManager.IsInRoleAsync(user, "Admin");
            model.companies = _db.Companies.Where(item => item.userId == user.Id)
                .Select(item => new CompanyView {
                    name = item.name,
                    endDate = item.endDate,
                    sum = item.sum,
                    id = item.id,
                    colectedSum = item.colectedSum,
                    createDate = item.createDate }).ToArray();
            BonusView[] bonusView = _db.BonusesToUsers.Where(item => item.user == user)
                .Select(item => new BonusView {
                    id = item.bonus.id,
                    Name = item.bonus.name,
                    Description = item.bonus.description }).ToArray();
            model.bonuses = bonusView;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditName(string userName)
        {
            if (userName != null)
            {
                userName = userName.Replace(" ", "");
                if (userName != string.Empty)
                {
                    if (_db.Users.FirstOrDefault(item => item.UserName == userName) == null)
                    {
                        User user = await _userManager.GetUserAsync(User);
                        await _userManager.SetUserNameAsync(user, userName);
                        await _signInManager.SignOutAsync();
                        await _signInManager.SignInAsync(user, false);
                    }
                }
            }
            return RedirectToAction("Index", "Profile");
        }
        public async Task<IActionResult> SwitchColor()
        {
            User user = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(user, "Dark"))
                await _userManager.RemoveFromRoleAsync(user, "Dark");
            else
                await _userManager.AddToRoleAsync(user, "Dark");
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Profile");
        }
        [HttpPost]
        public async Task<IActionResult> RemoveBonus(ProfileViewModel model)
        {
            Bonus bonus = _db.Bonus.FirstOrDefault(item => item.id == model.removeBonusID);
            if (bonus != null)
            {
                User user = await _userManager.GetUserAsync(User);
                _db.BonusesToUsers.Remove(_db.BonusesToUsers.FirstOrDefault(item => item.user == user && item.bonus == bonus));
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Profile");
        }
    }
}
