using System.IO;
using GitServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace GitServer.Controllers
{
    public class BranchController : Controller
    {
        private readonly GitRepositoryService _repositoryService;

        // GET
        public BranchController(GitRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }
        
        public IActionResult Index(string userName, string repoName)
        {
            var path = Path.Combine(userName, repoName);
            var repo = _repositoryService.GetRepository(path);
            return View(repo.Branches);
        }

    }
}