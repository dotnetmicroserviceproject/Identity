using Microsoft.AspNetCore.Identity;

namespace User.Service.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Status { get; set; } = "Active";
        public decimal Gil { get; set; }
        public string PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenTime { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
        public ICollection<LastLogin> LastLogins { get; set; } = new List<LastLogin>();

    }
}
