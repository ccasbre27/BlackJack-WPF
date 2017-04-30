using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackCL
{
    public class Card
    {
        public CardSuit Suit { get; set; }


        private CardValue _Value;

        public CardValue Value
        {
            get { return _Value; }
            set 
            {

                if (Suit != CardSuit.Undefined && value != CardValue.Undefined)
                {
                    // se establece la url de la imagen
                    PictureURL = Suit.ToString().ToLower() + "_of_" + value.ToString().ToLower() + ".png";
                }

                _Value = value; 
            }
        }
        

        public string PictureURL { get; set; }

        public Card()
        {
            Suit = CardSuit.Undefined;
            Value = CardValue.Undefined;
        }

        public Card(CardSuit pSuit, CardValue pCardValue)
        {
            Suit = pSuit;
            Value = pCardValue;

        }

        public Card(CardSuit pSuit, CardValue pCardValue, string pPictureURL)
        {
            Suit = pSuit;
            Value = pCardValue;
            PictureURL = pPictureURL;
        }

        public override bool Equals(object obj)
        {
            Card card = (Card) obj;

            return Suit == card.Suit && Value == card.Value;
        }
    }
}
