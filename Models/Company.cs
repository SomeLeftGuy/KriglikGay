using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesMain.Models
{
    public class Company
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string video { get; set; }
        public string image { get; set; }
        public string colectedSum { get; set; }
        public string sum { get; set; }
        public string endDate{ get; set; }
        public string createDate { get; set; }
        public string userId { get; set; }
        public int themeId { get; set; }
        public Theme theme { get; set; }
        public User user { get; set; }
    }
}
