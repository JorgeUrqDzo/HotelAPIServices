using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Entities
{
    public class Role
    {
        [Key] public Guid Id { get; set; }

        [Required] [MaxLength(50)] public string Name { get; set; }

        public string Description { get; set; }

        [Required] [MaxLength(25)] public string Identifier { get; set; }

        public int Order { get; set; }

        public ICollection<User> Users { get; set; }
    }
}