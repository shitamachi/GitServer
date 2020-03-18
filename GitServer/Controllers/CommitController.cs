using System;
using System.IO;
using System.Linq;
using System.Text;
using GitServer.Services;
using LibGit2Sharp;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace GitServer.Controllers
{
    public class CommitController : Controller
    {
        private readonly GitRepositoryService _repositoryService;

        public CommitController(GitRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public IActionResult GetAllCommit(string userName, string repoName, string branch)
        {
            var path = Path.Combine(userName, repoName);
            Repository repo = _repositoryService.GetRepository(path);
            var list = repo.Commits.QueryBy(new CommitFilter {IncludeReachableFrom = repo.Branches[branch]});
            var groupBy = list.ToList().GroupBy(commit => commit.Committer.When.Date);
            return View(list.ToList());
        }

        public IActionResult ShowSingleCommitDetail(string userName, string repoName, string branch, string sha1)
        {
            ViewBag.Sha1 = sha1;
            var path = Path.Combine(userName, repoName);
            var repo = _repositoryService.GetRepository(path);
            var commit = repo.Commits.Single(c => c.Sha.Equals(sha1));

            var c = repo.Commits.Single(c => c.Sha == sha1);
            foreach (var t in c.Tree)
            {
                var blob = t.Target as Blob;
                if (blob == null) continue;
                using var content = new StreamReader(blob.GetContentStream(), Encoding.UTF8);
                var fileContent = content.ReadToEnd();
                Console.WriteLine(fileContent);
            }

            foreach (var parent in commit.Parents)
            {
                Console.WriteLine("{0} | {1}", commit.Sha, commit.MessageShort);
                foreach (TreeEntryChanges change in repo.Diff.Compare<TreeChanges>(parent.Tree,
                    commit.Tree))
                {
                    Console.WriteLine("{0} : {1}", change.Status, change.Path);
                }
            }
            return View(new Tuple<Repository,string>(repo, sha1));
        }
    }
}