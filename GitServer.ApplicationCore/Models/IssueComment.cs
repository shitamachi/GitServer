namespace GitServer.ApplicationCore.Models
{
    public class IssueComment
    {
        public long IssueID { get; set; }
        public long CommentID { get; set; }
        public virtual Issue Issue { get; set; }
        public virtual Comment Comment { get; set; }
    }
}