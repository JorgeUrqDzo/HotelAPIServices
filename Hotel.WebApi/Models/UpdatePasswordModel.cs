using System;

namespace Hotel.WebApi.Models
{
    public class UpdatePasswordModel
    {
        public Guid UserId { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public Guid? ActivationToken { get; set; }
    }
}