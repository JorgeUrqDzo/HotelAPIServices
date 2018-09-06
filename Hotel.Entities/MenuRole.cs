using System;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Entities
{
    public class MenuRole
    {
        [Required] public Guid MenuId { get; set; }

        [Required] public Guid RoleId { get; set; }

        public Menu Menu { get; set; }

        public Role Role { get; set; }
    }
}