using System;
using BlackJackCL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class TestDeck
    {
        [TestMethod]
        public void Fill()
        {
            Deck deck = new Deck();

            Assert.AreEqual(deck.DeckCard.Count, 52);
        }
    }
}
