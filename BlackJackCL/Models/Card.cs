using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackCL
{
    [Serializable]
    public class Card
    {
        public CardSuit Suit { get; set; }

        public CardValue Value { get; set; }

        public string PictureURL { get; set; }

        public Card()
        {
            Suit = CardSuit.Undefined;
            Value = CardValue.Undefined;
        }

        public Card(CardSuit suit, CardValue cardValue)
        {
            this.Suit = suit;
            this.Value = cardValue;

        }

        public Card(CardSuit suit, CardValue cardValue, string pictureURL)
        {
            this.Suit = suit;
            this.Value = cardValue;
            this.PictureURL = pictureURL;
        }

        public override bool Equals(object obj)
        {
            Card card = (Card) obj;

            return Suit == card.Suit && Value == card.Value;
        }
    }
}
