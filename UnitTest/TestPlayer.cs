using System;
using BlackJackCL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class TestPlayer
    {
        [TestMethod]
        public void TestBlackJack()
        {
            Player player = new Player();

            player.AddCard(new Card(CardSuit.Clubs, CardValue.Jack));

            player.AddCard(new Card(CardSuit.Clubs, CardValue.Ace));

            Assert.AreEqual(player.Status, Status.BlackJack);
        }

        [TestMethod]
        public void Test21()
        {
            Player player = new Player();

            player.AddCard(new Card(CardSuit.Clubs, CardValue.Seven));
            player.AddCard(new Card(CardSuit.Diamonds, CardValue.Seven));
            player.AddCard(new Card(CardSuit.Spades, CardValue.Seven));

            Assert.AreEqual(player.Status, Status.TwentyOne);
        }

        [TestMethod]
        public void TestLost()
        {
            Player player = new Player();

            player.AddCard(new Card(CardSuit.Clubs, CardValue.Seven));
            player.AddCard(new Card(CardSuit.Diamonds, CardValue.Seven));
            player.AddCard(new Card(CardSuit.Spades, CardValue.Eight));

            Assert.AreEqual(player.Status, Status.Lost);
        }

        [TestMethod]
        public void TestFiveCards()
        {
            Player player = new Player();

            player.AddCard(new Card(CardSuit.Clubs, CardValue.Two));
            player.AddCard(new Card(CardSuit.Diamonds, CardValue.Two));
            player.AddCard(new Card(CardSuit.Spades, CardValue.Two));
            player.AddCard(new Card(CardSuit.Spades, CardValue.Three));
            player.AddCard(new Card(CardSuit.Hearts, CardValue.Two));

            Assert.AreEqual(player.Status, Status.FiveCards);
        }

        [TestMethod]
        public void TestASConverted()
        {
            Player player = new Player();

            player.AddCard(new Card(CardSuit.Clubs, CardValue.Ace));
            player.AddCard(new Card(CardSuit.Clubs, CardValue.Two));
            player.AddCard(new Card(CardSuit.Diamonds, CardValue.Two));
            player.AddCard(new Card(CardSuit.Spades, CardValue.Two));
            player.AddCard(new Card(CardSuit.Hearts, CardValue.Jack));

            Assert.AreEqual(player.Status, Status.FiveCards);
        }
    }
}
