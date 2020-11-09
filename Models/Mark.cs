using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesMain.Models
{
    public class Mark
    {
        public int id { get; set; }
        public int value { get; set; }
        public int userId { get; set; }
        public User user { get; set; }
        public int companyId { get; set; }
        public Company company { get; set; }
    }
}
