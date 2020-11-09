using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesMain.Models
{
    public class Bonus
    {
        public int id { get; set; }
        public string name { get; set; }
        public string sum { get; set; }
        public string description { get; set; }
        public int companyID { get; set; }
        public Company company { get; set; }
    }
}
