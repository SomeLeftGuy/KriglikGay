using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesMain.Models
{
    public class TagsToCompany
    {
        public int id { get; set; }
        public int tagId { get; set; }
        public Tag tag { get; set; }
        public int companyId { get; set; }
        public Company company { get; set; }
    }
}
