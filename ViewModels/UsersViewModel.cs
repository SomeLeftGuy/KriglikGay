using CoursesMain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesMain.ViewModels
{
    public class UsersViewModel
    {
        public string AdminName { get; set; }
        public UserView[] UsersList { get; set; }
    }
    public class UserView
    {
        public string id { get; set; }
        public bool Selected { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public string Type { get; set; }
    }
}
