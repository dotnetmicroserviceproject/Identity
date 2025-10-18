using MassTransit;
using Microsoft.AspNetCore.Identity;
using User.Contracts;
using User.Service.Entities;
using User.Service.Exceptions;


namespace User.Service.Consumers
{
    public class DebitGilConsumer : IConsumer<DebitGil>
    {
        private readonly UserManager<ApplicationUser> userManager;
        public DebitGilConsumer(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task Consume(ConsumeContext<DebitGil> context)
        {
            var message = context.Message;
            var user = await userManager.FindByIdAsync(message.UserId.ToString());

            if (user == null)
            {
                throw new UnKnownUserException(message.UserId);
            }

            user.Gil -= message.Gil;

            if(user.Gil < 0)
            {
                throw new InsufficientFundsException(message.Gil, message.UserId);
            }

            await userManager.UpdateAsync(user);

            await context.Publish(new GilDebited(message.CorrelatonId));
            
        }
    }
}
