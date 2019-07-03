using System;

namespace ClassLibrary1
{
    [Serializable]
    public class Transaction
    {
        public Transaction(decimal amount, string fromAddress, string toAddress)
        {
            Amount = amount;
            FromAddress = fromAddress;
            ToAddress = toAddress;
        }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public Decimal Amount { get; set; }
    }
}
