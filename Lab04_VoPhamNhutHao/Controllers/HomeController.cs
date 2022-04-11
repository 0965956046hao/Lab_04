using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab04_VoPhamNhutHao.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace Lab04_VoPhamNhutHao.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            BigSchool context = new BigSchool();
            var upcommingCourse = context.Course.Where(p => p.DateTime > DateTime.Now).OrderBy(p => p.DateTime).ToList();
            // lay user login hien tai dua vao viewbag -> de truyen qua view
            // neu gia tri = null -> chua đang nhap 
            var loginUser = User.Identity.GetUserId();
            ViewBag.LoginUser = User.Identity.GetUserId();
            foreach (Course i in upcommingCourse)
            {
                ApplicationUser user =
                System.Web.HttpContext.Current.GetOwinContext().GetUserManager < ApplicationUserManager > ().FindById(i.LecturerId);
                i.Name = user.Name;
                Attendance find = context.Attendance.FirstOrDefault(p => p.CourseId == i.Id && p.Attendee == loginUser);
                if (find == null)
                    i.isShowGoing = true;
                Following findFl = context.Followings.FirstOrDefault(p => p.FollowerId == loginUser && p.FolloweeId == i.LecturerId);
                if (findFl == null)
                    i.isShowFollow = true;
            }
            return View(upcommingCourse);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
                
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}