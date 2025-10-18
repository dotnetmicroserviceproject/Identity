namespace User.Service.Exceptions
{
    [Serializable]
    internal class InsufficientFundsException : Exception
    {
        public decimal GilToDebit { get; }
        public Guid UserId { get; }

        public InsufficientFundsException(decimal gil, Guid userId)
            : base($"Not enough gil to debit `{gil}` from user `{userId}`")
        {
            this.GilToDebit = gil;
            this.UserId = userId;
        }
    }
}