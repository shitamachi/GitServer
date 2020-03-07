using System;
using System.IO;
using System.Linq;
using GitServer.Services;
using LibGit2Sharp;
using Microsoft.AspNetCore.Mvc;

namespace GitServer.Controllers
{
    public class CommitController: Controller
    {
        private readonly GitRepositoryService _repositoryService;

        public CommitController(GitRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }
        
        public IActionResult GetAllCommit(string userName, string repoName, string id)
        {
            var path = Path.Combine(userName, repoName);
            Repository repo = _repositoryService.GetRepository(path);
            var list = repo.Commits.QueryBy(new CommitFilter {IncludeReachableFrom = repo.Branches["master"]});
            var groupBy = list.ToList().GroupBy(commit => commit.Committer.When.Date);
            return View(list.ToList());
        }
    }
}