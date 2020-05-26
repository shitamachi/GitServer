using System;

namespace GitServer.ApplicationCore.Models
{
    public class Comment : BaseEntity
    {
        public long IssueIndex { get; set; }
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
    }
}