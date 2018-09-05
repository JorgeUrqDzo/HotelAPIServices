using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Entities
{
    public class Menu
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? ParentId { get; set; } // ParentId = null is menu, ParentId != null is submenu

        [Required]
        public string Type { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string State { get; set; }

        public string Icon { get; set; }

        [Required]
        public int Index { get; set; }

        public string Tooltip { get; set; }

        public bool Disabled { get; set; }

        [Required]
        public string EndPoint { get; set; }

        public bool Optional { get; set; }

        public Menu Parent { get; set; }

        public ICollection<MenuRole> Permissions { get; set; }
    }
}