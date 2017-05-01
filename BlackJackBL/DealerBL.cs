using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJackCL;

namespace BlackJackBL
{
    public class DealerBL
    {
        private Deck DealerDeck;

        public DealerBL()
        {
            DealerDeck = new Deck();
        }

        /// <summary>
        /// Este método devuelve la primer carta
        /// </summary>
        /// <returns>carta en la cima</returns>
        public Card GetCard()
        {
            return DealerDeck.DeckCard.Pop();
        }
    }
}
