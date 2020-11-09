using CoursesMain.Filters;
using CoursesMain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesMain.Controllers
{
    [TypeFilter(typeof(UserFilter))]
    public class CommentsHub : Hub
    {
        private ApplicationContext _db;
        public CommentsHub(ApplicationContext context)
        {
            _db = context;
        }
        public async Task Send(string message, string userName, string company)
        {
            User user = _db.Users.FirstOrDefault(item => item.UserName == userName);
            Comment comment = _db.Comments.FirstOrDefault(item => item.companyId + "" == company && item.user == user);
            if (comment == null)
            {
                if (message != "")
                {
                    _db.Comments.Add(new Comment { user = user, companyId = Convert.ToInt32(company), text = message });
                    _db.SaveChanges();
                }
            }
            else
            {
                if (message == "")
                {
                    _db.Comments.Remove(comment);
                    _db.SaveChanges();
                }
                else
                {
                    comment.text = message;
                    _db.Comments.Update(comment);
                    _db.SaveChanges();
                }
            }
            comment = _db.Comments.FirstOrDefault(item => item.companyId + "" == company && item.user == user);
            if (comment != null)
                await Clients.All.SendAsync("Send", message, userName, comment.id);
            else
                await Clients.All.SendAsync("Send", message, userName, 0);
        }
        public async Task Rating(string userName, bool value, string commentId)
        {
            int changeValue = 0;
            Comment comment = _db.Comments.FirstOrDefault(item => item.id + "" == commentId);
            User user = _db.Users.FirstOrDefault(item => item.UserName == userName);
            Rating rating = _db.Ratings.FirstOrDefault(item => item.comment == comment && item.user == user);
            if (rating == null)
            {
                _db.Ratings.Add(new Rating { user = user, comment = comment, value = value });
                _db.SaveChanges();
                if (value) changeValue = 1;
                else changeValue = -1;
            }
            else
            {
                if (rating.value == value)
                {
                    _db.Ratings.Remove(rating);
                    _db.SaveChanges();
                    if (value) changeValue = -1;
                    else changeValue = 1;
                }
                else
                {
                    rating.value = value;
                    _db.Ratings.Update(rating);
                    _db.SaveChanges();
                    if (value) changeValue = 2;
                    else changeValue = -2;
                }
            }
            await Clients.All.SendAsync("Rating", commentId, changeValue);
        }
    }
}
