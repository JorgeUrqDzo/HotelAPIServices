using System;

namespace Hotel.Entities
{
    public class ChangeTrackingEntity
    {
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}