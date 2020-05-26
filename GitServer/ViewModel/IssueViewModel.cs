using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitServer.ApplicationCore.Models;

namespace GitServer.ViewModel
{
    public class IssueViewModel
    {
        public long IssueId { get; set; }
        public string UserName { get; set; }
        public long RepositoryName { get; set; }
        public int Index { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsPull { get; set; }
        public bool IsClosed { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<Label> Labels { get; set; }
        public long? ParentIssueId { get; set; }
        public bool IsParent
        {
            get => ParentIssueId != null;
            set { }
        }
    }
}
