using System;
using System.Collections.Generic;
using System.Text;

namespace GitServer.ApplicationCore.Models
{
    public class Issue:BaseEntity
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public long RepositoryID { get; set; }
        public int Index { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsPull { get; set; }
        public bool IsClosed { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<Label> Labels { get; set; }
        //public long ParentIssueId { get; set; }
    }
}
