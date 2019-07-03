using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace ClassLibrary1
{
    public class Block
    {
        public Block(DateTime timeStamp, IEnumerable<Transaction> data, string previousHash = null)
        {
            
            TimeStamp = timeStamp;
            Data = data;
            PreviousHash = previousHash;
            Hash = CalculateHash();
        }
        public int Index { get; set; }
        public IEnumerable<Transaction> Data { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public int Nonce;

        public string Hash { get; set; }

        public void MineBlock(int difficulty)
        {
            while (Hash.Substring(0, difficulty) != new string('0', difficulty))
            {
                Nonce++;
                Hash = CalculateHash();
            }
        }

        public string CalculateHash()
        {
            var mySha256 = SHA256.Create();
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, new TempBlock { TimeStamp = TimeStamp, Data = Data, PreviousHash = PreviousHash, Nonce = Nonce });
                return Convert.ToBase64String(mySha256.ComputeHash(ms.ToArray()));
            }
        }
    }
}
