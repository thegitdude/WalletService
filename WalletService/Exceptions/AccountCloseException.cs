using System;

namespace WalletService.Exceptions
{
    public class AccountCloseException : Exception
    {
        public AccountCloseException(string message): base(message) { }
    }
}