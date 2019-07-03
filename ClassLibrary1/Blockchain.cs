using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ClassLibrary1
{
    public class Blockchain
    {
        public readonly List<Block> Chain = new List<Block>();
        private int _minigReward = 100;

        public Blockchain()
        {
            Difficulty = 1;
            PendingTransactions = new List<Transaction>();
            Chain.Add(GenerateGenisysBlock());
        }

        public int Difficulty { get; set; }
        public List<Transaction> PendingTransactions { get; set; }

        private Block GenerateGenisysBlock()
        {
            var genesysBlock = new Block(new DateTime(2017, 1, 1), new Transaction[1], "0");
            genesysBlock.MineBlock(Difficulty);
            return genesysBlock;
        }

        private Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }

        public bool IsChainValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                Block currentBlock = Chain[i];
                Block previousBlock = Chain[i - 1];
                if (currentBlock.Hash != currentBlock.CalculateHash()) return false;
                if (previousBlock.Hash != currentBlock.PreviousHash) return false;
            }
            return true;
        }

        public void MinePendingTransactions(string minigRewardAddress)
        {
            if (PendingTransactions.Count == 0) return;
            var block = new Block(DateTime.Now, PendingTransactions);
            block.MineBlock(Difficulty);
            Debug.WriteLine("bock mined");
            Chain.Add(block);
            PendingTransactions = new List<Transaction>
            {
                new Transaction(_minigReward, null, minigRewardAddress)
            };
        }

        public void CreateTransaction(Transaction transaction)
        {
            PendingTransactions.Add(transaction);
        }

        public decimal GetBalance(string address)
        {
            decimal result = 0;
            foreach (Transaction tran in Chain.SelectMany(block => block.Data))
            {
                if (tran == null) continue;
                if (tran.FromAddress == address)
                    result -= tran.Amount;
                if (tran.ToAddress == address)
                    result += tran.Amount;
            }
            return result;
        }

        public decimal GetPendingBalance(string address)
        {
            decimal result = GetBalance(address);
            foreach (Transaction tran in PendingTransactions)
            {
                if (tran == null) continue;
                if (tran.FromAddress == address)
                    result -= tran.Amount;
                if (tran.ToAddress == address)
                    result += tran.Amount;
            }
            return result;
        }

        public void AddBlock(Block block)
        {
            if (block == null) return;
            block.PreviousHash = GetLatestBlock().Hash;
            block.MineBlock(Difficulty);
            Chain.Add(block);
        }
    }
}