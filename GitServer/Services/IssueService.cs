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

        public IssueService(IRepository<Issue> issue)
        {
            _issue = issue;
        }

        public IEnumerable<Issue> GetAllIssues()
        {
            return _issue.List();
        }

        public Issue GetIssueById(long id)
        {
            return _issue.GetById((int) id);
        }
    }
}
