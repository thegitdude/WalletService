using System;

namespace WalletService.Model
{
    public class Account
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public DateTimeOffset DeletedDate { get; set; }
        public decimal Balance { get; set; }
        public bool Active { get; set; }
    }
}