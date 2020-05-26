using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitServer.ApplicationCore.Interfaces;
using GitServer.ApplicationCore.Models;

namespace GitServer.Services
{
    public class IssueService
    {
        private readonly IRepository<Issue> _issue;
        private readonly IRepository<Comment> _comment;

        public IssueService(IRepository<Issue> issue, IRepository<Comment> comment)
        {
            _issue = issue;
            _comment = comment;
        }

        public IEnumerable<Issue> GetAllIssues()
        {
            return _issue.List();
        }

        public Issue GetIssueById(long id)
        {
            return _issue.GetById((int) id);
        }

        public Issue GetIssueByIndex(int index)
        {
            return _issue.List(issue => issue.Index == index).FirstOrDefault();
        }

        public void AddIssue(Issue issue)
        {
            _issue.Add(issue);
        }

        public List<Comment> GetAllCommentsByIssueId(long id)
        {
            return _comment.List(comment => comment.IssueIndex == id).ToList();
        }

        public void AddComment(Comment comment)
        {
            _comment.Add(comment);
        }
    }
}