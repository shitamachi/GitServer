namespace GitServer.ApplicationCore.Models
{
    public class Label: BaseEntity
    {
        //public long ID { get; set; }
        public long RepositoryID { get; set; }
        public string content { get; set; }
    }
}