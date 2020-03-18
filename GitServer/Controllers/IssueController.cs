using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public IssueController(IssueService issue, UserService user)
        {
            this._issue = issue;
            _user = user;
        }
        public IActionResult Index(string userName, string repoName)
        {
            var issues = _issue.GetAllIssues();
            var results = 
                issues.Select(ConvertIssueToViewModel).ToList();
            return View(results);
        }

        public IssueViewModel ConvertIssueToViewModel(Issue issue)
        {
            var user = _user.GetUserById(issue.UserID);
            return new IssueViewModel
            {
                IssueId = issue.ID,
                UserName = user.Name,
                Title = issue.Title,
                Content = issue.Comment,
                CreationDate = issue.CreationDate,
                IsClosed = issue.IsClosed,
                IsPull = issue.IsPull,
                Label = issue.Label,
                ParentIssueId = issue.ParentIssueId
            };
        }
    }
}