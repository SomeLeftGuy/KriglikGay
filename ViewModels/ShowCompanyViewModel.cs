using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesMain.ViewModels
{
    public class ShowCompanyViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Video { get; set; }
        public string Theme { get; set; }
        public bool[] mark { get; set; }
        public double marks { get; set; }
        public string User { get; set; }
        public NewsView[] news { get; set; }
        public NewsView addNews { get; set; }
        public BonusView[] bonuses { get; set; }
        public BonusView addBonus { get; set; }
        public int deleteNewsID { get; set; }
        public int deleteBonusID { get; set; }
        public CommentsView[] comments { get; set; }
        public int payID { get; set; }

    }
    public class NewsView
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
    public class CommentsView
    {
        public int id { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
    }
    public class BonusView
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Sum { get; set; }
    }
}
