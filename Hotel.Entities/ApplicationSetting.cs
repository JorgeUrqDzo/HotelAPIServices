using System;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Entities
{
    public class ApplicationSetting : ChangeTrackingEntity
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(20), Required]
        public string Type { get; set; }

        [MaxLength(50), Required]
        public string Name { get; set; }

        [MaxLength(50), Required]
        public string Value { get; set; }
    }
}