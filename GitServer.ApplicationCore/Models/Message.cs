using System;

namespace GitServer.ApplicationCore.Models
{
    public class Message : BaseEntity
    {
        public long SendUserId { get; set; }
        public string SendUserName { get; set; }
        public long ReceiverUserId { get; set; }
        public string ReceiverUserName { get; set; }
        public string Content { get; set; }
        public DateTime SendDate { get; set; }
        public bool IsRead { get; set; }
    }
}