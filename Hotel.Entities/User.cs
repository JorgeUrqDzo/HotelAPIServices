using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Hotel.Entities
{
    public class User : ChangeTrackingEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? InstanceId { get; set; }
        [MaxLength(200), Required]
        public string UserName { get; set; }
        [MaxLength(50), Required]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string MiddleName { get; set; }
        [MaxLength(50), Required]
        public string LastName { get; set; }
        [MaxLength(200), Required, EmailAddress]
        public string Email { get; set; }
        [MinLength(8), MaxLength(256)]
        public string Password { get; set; }
        public DateTime? PasswordExpires { get; set; }
        public DateTime? LastLogin { get; set; }
        [MaxLength(20)]
        public string LastLoginIp { get; set; }
        public bool Locked { get; set; }
        public int LoginAttempts { get; set; }
        [Required]
        public Guid RoleId { get; set; }
        public bool IsActive { get; set; }
        public bool IsRemoved { get; set; }
        public Guid? ActivationToken { get; set; }
        
        public Role Role { get; set; }

    }
}