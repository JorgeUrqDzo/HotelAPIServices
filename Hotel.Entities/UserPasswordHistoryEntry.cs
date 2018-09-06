using System;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Entities
{
    public class UserPasswordHistoryEntry
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        [Required, MaxLength(256)]
        public string Password { get; set; }

        public DateTime UpdatedDate { get; set; }

        public User User { get; set; }
    }
}