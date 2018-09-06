using System;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Entities
{
    public class ApplicationEvent
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(20)]
        public string EventType { get; set; }

        public DateTime EventDate { get; set; }
        public Guid? InstanceId { get; set; }
        public Guid? UserId { get; set; }

        [Required, MaxLength(20)]
        public string ServerName { get; set; }

        [Required, MaxLength(20)]
        public string UserIPAddress { get; set; }

        public string StackTrace { get; set; }

        [Required]
        public string Message { get; set; }

        [MaxLength(255)]
        public string Controller { get; set; }

        public User User { get; set; }
    }
}