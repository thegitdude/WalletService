using System;

namespace WalletService.Exceptions
{
    public class NotEnoughFundsException : Exception
    {
        public NotEnoughFundsException(string message) : base(message) { }
    }
}