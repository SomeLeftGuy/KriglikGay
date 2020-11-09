using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesMain.ViewModels
{
    public class CompanyViewModel
    {
        [Required]
        public int id { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string Theme { get; set; }
        public string[] Themes { get; set; }
        public string Tags { get; set; }
        public string CompanyInfo { get; set; }
        public string CompanyLinkVideo { get; set; }
        public string CompanyLinkImage { get; set; }
        [Required]
        public string Sum { get; set; }
        [Required]
        public string EndDate { get; set; }
    }
}
