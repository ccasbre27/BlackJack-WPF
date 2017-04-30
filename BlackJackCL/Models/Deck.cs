using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackCL
{
    public class Deck
    {
        public Stack<Card> DeckCard { get; set; }

        public Deck()
        {
            DeckCard = new Stack<Card>();
            Fill();
        }

        public void Fill()
        {
            List<Card> cards = new List<Card>();
            Card tempCard;
            Random random = new Random(Guid.NewGuid().GetHashCode());
            int index = 0;

            // se llena el deck

            foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
            {
                // se ignora el undefined
                if (suit != CardSuit.Undefined)
                {
                    foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
                    {
                        // se ignora el undefined y el 1 ya que es el mismo que el as
                        if (value != CardValue.One && value != CardValue.Undefined)
                        {
                            cards.Add(new Card(suit, value));
                        }
                    }
                }
            }

            // se mueven las posiciones de las cartas
            for (int i = 0, tam = cards.Count; i < tam; i++)
            {
                // guardamos la carta a mover
                tempCard = cards[i];

                // obtenemos un índice nuevo
                index = random.Next(0, i + 1);

                // establecemos en la carta actual la carta que está en el índice nuevo
                cards[i] = cards[index];

                // en el índice nuevo establecemos la carta que guardamos anteriormente
                cards[index] = tempCard;
            }
           

            DeckCard = new Stack<Card>(cards);
            
        }
    }
}
