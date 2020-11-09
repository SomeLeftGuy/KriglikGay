using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesMain.ViewModels
{
    public class HomeViewModel
    {
        public HomeView[] Companies { get; set; }
    }
    public class HomeView
    {
        public int Ref { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double Rating { get; set; }
        public double FullRating { get; set; }
        public DateTime EndDate { get; set; }
        public string Sum { get; set; }
        public string CollectedSum { get; set; }
    }
}
