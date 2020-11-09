using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesMain.ViewModels
{
    public class ProfileViewModel
    {
        public bool accessType { get; set; }
        public CompanyView[] companies { get; set; }
        public BonusView[] bonuses { get; set; }
        public int removeBonusID { get; set; }
    }
    public class CompanyView
    {
        public int id { get; set; }
        public string name { get; set; }
        public string createDate { get; set; }
        public string endDate { get; set; }
        public string sum { get; set; }
        public string colectedSum { get; set; }
        public bool selected { get; set; }
    }
}
