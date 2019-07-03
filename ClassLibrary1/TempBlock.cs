using System;
using System.Collections.Generic;

namespace ClassLibrary1
{
    [Serializable]
    internal class TempBlock
    {
        public DateTime TimeStamp { get; set; }
        public IEnumerable<Transaction> Data { get; set; }
        public string PreviousHash { get; set; }
        public int Nonce { get; set; }
    }
}
