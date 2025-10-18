
namespace User.Service.Exceptions
{
    [Serializable]
    internal class UnKnownUserException : Exception
    {
        public Guid UserId { get; }
        public UnKnownUserException(Guid userId)
            : base($"Unkown user `{userId}`")
        {
            this.UserId = userId;
        }
    }
}