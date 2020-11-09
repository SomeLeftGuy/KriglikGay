using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesMain.Models
{
    public class Rating
    {
        public int id { get; set; }
        public bool value { get; set; }
        public int commentId { get; set; }
        public Comment comment { get; set; }
        public int userId { get; set; }
        public User user { get; set; }
    }
}
