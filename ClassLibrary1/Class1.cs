using System;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using Should;

namespace ClassLibrary1
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void ShouldPassValidation()
        {
            var savjeeCoin = new Blockchain();
            savjeeCoin.AddBlock(new Block(new DateTime(2017, 10, 7), new[] { new Transaction(4, null, null) }));
            savjeeCoin.AddBlock(new Block(new DateTime(2017, 12, 7), new[] { new Transaction(10, null, null) }));
            savjeeCoin.IsChainValid().ShouldBeTrue();
            Debug.WriteLine(JsonConvert.SerializeObject(savjeeCoin, Formatting.Indented));
        }

        [Test]
        public void ShouldFailValidationOnAlteredAmount()
        {
            var savjeeCoin = new Blockchain();
            savjeeCoin.AddBlock(new Block(new DateTime(2017, 10, 7), new[] { new Transaction(4, null, null) }));
            savjeeCoin.AddBlock(new Block(new DateTime(2017, 12, 7), new[] { new Transaction(10, null, null) }));
            savjeeCoin.Chain[1].Data.ToList()[0].Amount = 100;
            savjeeCoin.IsChainValid().ShouldBeFalse();
            Debug.WriteLine(JsonConvert.SerializeObject(savjeeCoin, Formatting.Indented));
        }

        [Test]
        public void ShouldFailValidationOnAlteredAmountAndHash()
        {
            var savjeeCoin = new Blockchain();
            savjeeCoin.AddBlock(new Block(new DateTime(2017, 10, 7), new[] { new Transaction(4, null, null) }));
            savjeeCoin.AddBlock(new Block(new DateTime(2017, 12, 7), new[] { new Transaction(10, null, null) }));
            savjeeCoin.Chain[1].Data.ToList()[0].Amount = 100;
            savjeeCoin.Chain[1].Hash = savjeeCoin.Chain[1].CalculateHash();
            savjeeCoin.IsChainValid().ShouldBeFalse();
            Debug.WriteLine(JsonConvert.SerializeObject(savjeeCoin, Formatting.Indented));
        }

        [Test]
        public void ShouldCheckBalances()
        {
            var savjeeCoin = new Blockchain();
            savjeeCoin.CreateTransaction(new Transaction(100, "address1", "address2"));
            savjeeCoin.CreateTransaction(new Transaction(50, "address2", "address1"));
            savjeeCoin.MinePendingTransactions("miner1");
            savjeeCoin.GetBalance("address1").ShouldEqual(-50);
            savjeeCoin.GetBalance("address2").ShouldEqual(50);
            savjeeCoin.GetBalance("miner1").ShouldEqual(0);
            savjeeCoin.GetPendingBalance("miner1").ShouldEqual(100);
            savjeeCoin.MinePendingTransactions("miner1");
            savjeeCoin.GetBalance("miner1").ShouldEqual(100);
            savjeeCoin.GetPendingBalance("miner1").ShouldEqual(200);
        }
    }
}
