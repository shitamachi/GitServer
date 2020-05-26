#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using GitServer.ApplicationCore.Interfaces;
using GitServer.ApplicationCore.Models;
using GitServer.Services;
using GitServer.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace GitServer.Controllers
{
    public class IssueController : Controller
    {
        private readonly IssueService _issue;
        private readonly UserService _user;
        private readonly IRepository<Repository> _repository;
        private IRepository<Message> _message;

        public IssueController(IssueService issue, UserService user, IRepository<Repository> repository, IRepository<Message> message)
        {
            this._issue = issue;
            _user = user;
            _repository = repository;
            _message = message;
        }

        public IActionResult Index(string userName, string repoName)
        {
            var issues = _issue.GetAllIssues();
            // var results =
            //     issues.Select(ConvertIssueToViewModel).ToList();
            var s = issues.Select(ConvertIssueToViewModel).ToList();
            return View(s);
        }

        [HttpGet]
        public IActionResult NewIssue(string userName, string repoName)
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewIssue(string userName, string repoName, AddIssueViewModel newIssue)
        {
            var currentUser = HttpContext.User.Identity.Name;
            if (currentUser == null)
            {
                return RedirectToAction("Login", "User");
            }

            var userId = _user.GetUserByName(userName).ID;
            var repo = _repository.List(
                    repository => repository.UserName == userName && repository.Name == repoName)
                .FirstOrDefault();
            _issue.AddIssue(new Issue
            {
                UserID = userId,
                Title = newIssue.Title,
                Content = newIssue.Content,
                RepositoryID = repo.ID
            });
            return RedirectToAction("Index");
        }

        public IActionResult IssueDetail(string userName, string repoName, string index)
        {
            var issue = _issue.GetIssueByIndex(int.Parse(index));
            var comments = _issue.GetAllCommentsByIssueId(issue.ID);
            // return View(new Tuple<Issue, List<Comment>>()ConvertIssueToViewModel(issue));
            return View(new Tuple<Issue, List<Comment>>(issue, comments));
        }

        [HttpPost]
        public IActionResult AddComment(string userName, string repoName, string index, string content, string replyUser)
        {
            var currentUser = _user.GetUserByName(HttpContext.User.Identity.Name);

            _issue.AddComment(new Comment
            {
                Content = content,
                CreationDate = DateTime.Now,
                UserName = currentUser.Name,
                IssueIndex = long.Parse(index),
                UserID = currentUser.ID
            });
            if (!string.IsNullOrEmpty(replyUser.Trim()))
            {
                _message.Add(new Message
                {
                    Content = $"[DON'T REPLY THIS MESSAGE]This message come from website,you have new issue create by user {userName},content is {content}",
                    IsRead = false,
                    SendDate = DateTime.Now,
                    SendUserId = currentUser.ID,
                    SendUserName = currentUser.Name,
                    ReceiverUserId = 0,
                    ReceiverUserName = userName
                });
            }            
            return RedirectToRoute("IssueDetail", new {userName = userName, repoName = repoName, index = index});
        }

        public IssueViewModel ConvertIssueToViewModel(Issue issue)
        {
            return new IssueViewModel
            {
                IssueId = issue.ID,
                UserName = issue.UserName,
                Title = issue.Title,
                Content = issue.Content,
                CreationDate = issue.CreationDate,
                IsClosed = issue.IsClosed,
                IsPull = issue.IsPull,
                Labels = issue.Labels,
                Index = issue.Index
            };
        }
    }
}