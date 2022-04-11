using Lab04_VoPhamNhutHao.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Lab04_VoPhamNhutHao.Controllers
{
    public class AttendanceController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Attend( Course courseDTO)
        {
            var userID = User.Identity.GetUserId();
                if (userID == null)
                return BadRequest("please login first");
            BigSchool context = new BigSchool();
            var attendance = new Attendance()
            {
                CourseId = courseDTO.Id,
                Attendee = userID
            };
            context.Attendance.Add(attendance);
            context.SaveChanges();
            return Ok();

        }
    }
}
