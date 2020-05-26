using System;
using System.Linq;
using GitServer.ApplicationCore.Interfaces;
using GitServer.ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace GitServer.Controllers
{
    public class ExploreController : Controller
    {
        private IRepository<User> _user;
        private IRepository<Message> _message;

        public ExploreController(IRepository<User> user, IRepository<Message> message)
        {
            _user = user;
            _message = message;
        }

        public IActionResult Index(string userName, string repoName)
        {
            var currentUser = HttpContext.User.Identity.Name;
            var allUsers = _user.List().Where(user => !user.Name.Equals(currentUser));
            return View(allUsers);
        }

        public IActionResult FollowUser()
        {
            throw new System.NotImplementedException();
        }

        public IActionResult AllMessages()
        {
            var name = HttpContext.User.Identity.Name;
            var list = _message.List(message => message.SendUserName.Equals(name) || message.ReceiverUserName.Equals(name));
            return View(list);
        }
        
        [HttpGet]
        public IActionResult MessageUser(string chatUserName)
        {
            var currentUserName = HttpContext.User.Identity.Name;
            if (string.IsNullOrEmpty(currentUserName?.Trim()))
            {
                return RedirectToAction("Login", "User");
            }

            // var messages = _message.List();
            var messages = _message.List(message =>
                (message.SendUserName.Equals(currentUserName) && message.ReceiverUserName.Equals(chatUserName))
                || ((message.SendUserName.Equals(chatUserName) && message.ReceiverUserName.Equals(currentUserName)))
            ).ToList().OrderBy(message => message.SendDate);

            return View(messages);
        }

        [HttpPost]
        public IActionResult MessageUser(string chatUserName, string content)
        {
            var currentUser = _user.List(user => user.Name.Equals(HttpContext.User.Identity.Name)).FirstOrDefault();
            var chatUser = _user.List(user => user.Name.Equals(chatUserName)).FirstOrDefault();

            if (currentUser != null && chatUser != null)
            {
                _message.Add(new Message
                {
                    Content = content,
                    SendDate = DateTime.Now,
                    SendUserId = currentUser.ID,
                    SendUserName = currentUser.Name,
                    ReceiverUserId = chatUser.ID,
                    ReceiverUserName = chatUser.Name
                });
                return RedirectToAction(nameof(MessageUser), new {chatUserName = chatUserName});
            }
            else
            {
                return View("_Error");
            }
        }
    }
}