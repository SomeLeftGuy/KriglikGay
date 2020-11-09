using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesMain.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Bonus> Bonus { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<TagsToCompany> TagsToCompanies { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<BonusesToUser> BonusesToUsers { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
