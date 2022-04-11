using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lab04_VoPhamNhutHao.Models;
using Microsoft.AspNet.Identity;

namespace Lab04_VoPhamNhutHao.Controllers
{
    public class FollowingsController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Follow(Following followingDTO)
        {
            var loginUser = User.Identity.GetUserId();
            followingDTO.FollowerId = loginUser;
            BigSchool context = new BigSchool();
            Following find = context.Followings.FirstOrDefault(p => p.FollowerId == followingDTO.FollowerId && p.FolloweeId == followingDTO.FolloweeId);
            if (find == null)
                context.Followings.Add(followingDTO);
            else
                context.Followings.Remove(find);
            context.SaveChanges();
            return Ok();
        }
    }
}
