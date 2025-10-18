using System.Diagnostics;

namespace User.Contracts
{
        public record DebitGil(Guid UserId,decimal Gil,Guid CorrelatonId);

        public record GilDebited(Guid CorrelatonId);

        public record ApplicationUserCreated(Guid UserId , string Email);

}
