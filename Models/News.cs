using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesMain.Models
{
    public class News
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public int companyId { get; set; }
        public Company company { get; set; }
    }
}
