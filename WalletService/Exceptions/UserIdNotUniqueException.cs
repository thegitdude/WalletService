using System;

namespace WalletService.Exceptions
{
    public class UserIdNotUniqueException: Exception
    {
        public UserIdNotUniqueException(string message): base(message)
        {
        }
    }
}