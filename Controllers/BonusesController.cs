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
    public class BonusesController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly UserManager<User> _userManager;
        public BonusesController(UserManager<User> userManager, ApplicationContext db)
        {
            _db = db;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> AddBonus(ShowCompanyViewModel model)
        {
            Company company = _db.Companies.FirstOrDefault(item => item.id == model.Id);
            if (company != null)
            {
                _db.Bonus.Add(new Bonus
                {
                    company = company,
                    name = model.addBonus.Name,
                    description = model.addBonus.Description,
                    sum = model.addBonus.Sum
                });
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(model.Id + "", "Company");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteBonus(ShowCompanyViewModel model)
        {
            _db.Bonus.Remove(_db.Bonus.FirstOrDefault(item => item.id == model.deleteBonusID));
            await _db.SaveChangesAsync();
            return RedirectToAction(model.Id + "", "Company");
        }
        [HttpPost]
        public async Task<IActionResult> Pay(ShowCompanyViewModel model)
        {
            Bonus bonus = _db.Bonus.FirstOrDefault(item => item.id == model.payID);
            if (bonus != null)
            {
                Company company = _db.Companies.FirstOrDefault(item => item.id == bonus.companyID);
                if (company != null)
                {
                    User user = await _userManager.GetUserAsync(User);
                    company.colectedSum = (int.Parse(company.colectedSum) + int.Parse(bonus.sum)) + "";
                    _db.Companies.Update(company);
                    _db.BonusesToUsers.Add(new BonusesToUser { bonus = bonus, user = user });
                    await _db.SaveChangesAsync();
                }
            }
            return RedirectToAction(model.Id + "", "Company");
        }
    }
}
