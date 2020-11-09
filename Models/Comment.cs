using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesMain.Models
{
    public class Comment
    {
        public int id { get; set; }
        public string text { get; set; }
        public string userId { get; set; }
        public User user { get; set; }
        public int companyId { get; set; }
        public Company company { get; set; }
    }
}
