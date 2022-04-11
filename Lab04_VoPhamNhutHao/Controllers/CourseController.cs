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
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Create()
        {
            BigSchool data = new BigSchool();
            Course objCourse = new Course();
            objCourse.ListCategory = data.Category.ToList();
            return View(objCourse);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course objCourse)
        {
            BigSchool data = new BigSchool();

            // Không xét valid LectureId vì bằng user đăng nhập
            ModelState.Remove("LecturerId");
            if (!ModelState.IsValid)
            {
                objCourse.ListCategory = data.Category.ToList();
                return View("Create", objCourse);
            }


            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LecturerId = user.Id;

            data.Course.Add(objCourse);
            data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Attending()
        {
            BigSchool context = new BigSchool();
            var userID = User.Identity.GetUserId();//get login user
            //lay danh sach khoa học mà userlogin do tham du ( chi moi lay dc ID)
            var listAttendance = context.Attendance.Where(p => p.Attendee == userID).ToList();
            var courses = new List<Course>();
            // tim chi tiet khoa hoc tu listattendance( ma khoa hoc, ten giao vien phai truy cap tu aspnetuser.name)
            foreach(Attendance temp in listAttendance)
            {
                Course objCourse = temp.Course;
                objCourse.Name = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(objCourse.LecturerId).Name;
                courses.Add(objCourse);
            }
            return View(courses);
        }
        [Authorize]
        public ActionResult Mine()
        {
           String  loginUser = User.Identity.GetUserId();
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(loginUser);
            BigSchool context = new BigSchool();
            var courses = context.Course.Where(p => p.LecturerId == loginUser && p.IsCanceled != true).ToList();
            foreach (Course i in courses)
            {
                i.Name = user.Name;// lấy thông tin của name
            }
            return View(courses);
        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            BigSchool context = new BigSchool();
            var loginUser = User.Identity.GetUserId();
            Course course = context.Course.FirstOrDefault(p => p.LecturerId == loginUser && p.Id == id);
            if (course == null)
            {
                return HttpNotFound("Không timg thấy khóa học");

            }
            course.ListCategory = context.Category.ToList();
            return View("Create", course);
        }
        public ActionResult Delete(int id)
        {
            var loginUser = User.Identity.GetUserId();
           // ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(loginUser);
            BigSchool context = new BigSchool();
            Course course = context.Course.FirstOrDefault(p => p.LecturerId == loginUser && p.Id == id);
            course.IsCanceled = true;
            context.SaveChanges();
            return RedirectToAction("Mine");
        }
        public ActionResult Following()
        {
            BigSchool context = new BigSchool();
            ApplicationUser loginUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listFollowing = context.Followings.Where(p => p.FollowerId == loginUser.Id).ToList();
            var courses = new List<Course>();
            foreach (Following temp in listFollowing)
            {
                var listCourse = context.Course.Where(p => p.LecturerId == temp.FollowerId).ToList();
                if (listCourse.Count > 0)
                {
                    string Name = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(listCourse[0].LecturerId).Name;
                    foreach (Course i in listCourse)

                        i.Name = Name;
                    courses.AddRange(listCourse);

                }
            }
            return View(courses);
        }
    }
}