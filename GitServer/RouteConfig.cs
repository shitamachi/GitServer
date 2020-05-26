using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;

namespace GitServer
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute(
                "Home",
                "{controller=Home}/{action=Home}/{id?}"
            );

            #region Routes for viewing the file tree

            routeBuilder.MapRoute(
                "RedirectGitLink",
                "{userName}/{repoName}.git",
                new {controller = "FileView", action = "RedirectGitLink"},
                new {method = new HttpMethodRouteConstraint("GET")}
            );

            routeBuilder.MapRoute(
                "GetRepositoryHomeView",
                "{userName}/{repoName}",
                new {controller = "FileView", action = "GetTreeView", id = "master", path = string.Empty},
                new {method = new HttpMethodRouteConstraint("GET")}
            );

            routeBuilder.MapRoute(
                "GetTreeView",
                "{userName}/{repoName}/tree/{id}/{*path}",
                new {controller = "FileView", action = "GetTreeView"},
                new {method = new HttpMethodRouteConstraint("GET")}
            );

            routeBuilder.MapRoute(
                "GetBlobView",
                "{userName}/{repoName}/blob/{id}/{*path}",
                new {controller = "FileView", action = "GetBlobView"},
                new {method = new HttpMethodRouteConstraint("GET")}
            );

            routeBuilder.MapRoute(
                "GetRawBlob",
                "{userName}/{repoName}/raw/{id}/{*path}",
                new {controller = "FileView", action = "GetRawBlob"},
                new {method = new HttpMethodRouteConstraint("GET")}
            );

            #endregion

            routeBuilder.MapRoute(
                "GetAllCommit",
                "{userName}/{repoName}/commit/{branch}",
                new {controller = "Commit", action = "GetAllCommit"},
                new {method = new HttpMethodRouteConstraint("GET")}
            );

            routeBuilder.MapRoute(
                "ShowSingleCommitDetail",
                "{userName}/{repoName}/commit/{branch}/{sha1}",
                new {controller = "Commit", action = "ShowSingleCommitDetail"},
                new {method = new HttpMethodRouteConstraint("GET")}
            );

            routeBuilder.MapRoute(
                "UserSetting",
                "{controller=User}/{action=Setting}"
            );

            routeBuilder.MapRoute(
                "Branch",
                "{userName}/{repoName}/branch/",
                new {controller = "Branch", action = "Index"},
                new {method = new HttpMethodRouteConstraint("GET")}
            );

            routeBuilder.MapRoute(
                "Issue",
                "{userName}/{repoName}/issue/",
                new {controller = "Issue", action = "Index"},
                new {method = new HttpMethodRouteConstraint("GET")}
            );

            routeBuilder.MapRoute(
                "IssueDetail",
                "{userName}/{repoName}/issue/{index}",
                new {controller = "Issue", action = "IssueDetail"},
                new {method = new HttpMethodRouteConstraint("GET")}
            );

            routeBuilder.MapRoute(
                "AddIssue",
                "{userName}/{repoName}/issue/new",
                new {controller = "Issue", action = "NewIssue"},
                new {method = new HttpMethodRouteConstraint("POST", "GET")}
            );

            routeBuilder.MapRoute(
                "AddComment",
                "{userName}/{repoName}/issue/{index}",
                new {controller = "Issue", action = "AddComment"},
                new {method = new HttpMethodRouteConstraint("POST")}
            );
        }
    }
}