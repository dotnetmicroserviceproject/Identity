using common.Entities;

namespace User.Service.Entities
{
    public class LastLogin : EntityBase
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime LoginTime { get; set; } = DateTime.UtcNow;
        public DateTime LogoutTime { get; set; }
        public string IPAddress { get; set; }
    }
}
