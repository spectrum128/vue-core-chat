
using System;
using Microsoft.AspNetCore.Mvc;
using StChat.Entities;
using StChat.Services;

namespace StChat.Controllers
{
    
    public class BaseController : ControllerBase
    {

        protected IUserService _userService;
        public BaseController()
        {

        }


        public User GetCurrentUser()
        {
            var username = HttpContext.User.Identity.Name;

            int userId = Convert.ToInt32(username);

            var user = _userService.GetById(userId);

            return user;
        }
    }
}