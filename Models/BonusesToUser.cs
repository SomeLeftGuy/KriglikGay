using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesMain.Models
{
    public class BonusesToUser
    {
        public int id { get; set; }
        public int userId { get; set; }
        public User user { get; set; }
        public int bonusId { get; set; }
        public Bonus bonus { get; set; }
    }
}
