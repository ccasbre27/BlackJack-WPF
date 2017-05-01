using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace BlackJackCL
{
    public class Player
    {
        public int IdPlayer { get; set; }

        public List<Card> Cards { get; set; }

        public int SumOfCards { get; set; }

        public TcpClient TcpConnection { get; set; }

        public Status Status { get; set; }

        public bool IsPlaying { get; set; }

        public Player()
        {
            IdPlayer = 0;
            Cards = new List<Card>();
            SumOfCards = 0;
            TcpConnection = new TcpClient();
            Status = Status.Undefined;
            IsPlaying = false;
        }

        public void AddCard(Card card)
        {
            Card tempCard = new Card();
            int index = 0;

            // se agrega la carta
            Cards.Add(card);

            switch (card.Value)
            {
                case CardValue.One:
                case CardValue.Two:
                case CardValue.Three:
                case CardValue.Four:
                case CardValue.Five:
                case CardValue.Six:
                case CardValue.Seven:
                case CardValue.Eight:
                case CardValue.Nine:
                case CardValue.Ten:
                    // cada posición está asociada al valor por lo que podemos hacer un casting
                    // para obtener el valor y sumarlo
                    // se suma 1 porque empieza en cero
                    SumOfCards += (int)card.Value + 1;
                    break;

                case CardValue.Ace:

                    // se verifica si hay un black jack
                    // para ello debe haber una J,Q o K en la primera carta ya que esta sería la segunda
                    if (Cards.Count == 2)
                    {
                        if (Cards[0].Value == CardValue.Jack ||
                            Cards[0].Value == CardValue.Queen ||
                            Cards[0].Value == CardValue.King)
                        {
                            Status = Status.BlackJack;
                        }
                    }

                    SumOfCards += 11;

                    break;

                // para el caso de la J, Q y K ya no basta con solo hacer el casting
                // ya que estos valen 10 y se encuentran en las posiciones 12,13 y 14
                case CardValue.Jack:
                case CardValue.Queen:
                case CardValue.King:

                    // se verifica si hay un black jack
                    // para ello debe haber un AS en la primera carta ya que esta sería la segunda
                    if (Cards.Count == 2)
                    {
                        if (Cards[0].Value == CardValue.Ace)
                        {
                            Status = Status.BlackJack;
                        }
                    }

                    SumOfCards += 10;
                    break;

                default:
                    break;

            } // swtich

            
            // se verifica si se pasó de la suma
            if (SumOfCards > 21)
            {
                // entonces se pasa el As a valer uno
             
                // se busca el As
                tempCard = Cards.FirstOrDefault(e => e.Value == CardValue.Ace);

                // se verifica si se encontró una carta
                if (tempCard != null)
                {
                    // se busca el índice de esta carta
                    //index = Cards.FindIndex(e => e.Suit == tempCard.Suit && e.Value == tempCard.Value);
                    index = Cards.FindIndex(e => e.Equals(tempCard));

                    // se establece el uno
                    Cards[index].Value = CardValue.One;

                    // se resta 11 del as y se suma 1 del uno
                    SumOfCards -= 10;
                }
                else
                {
                    // en caso que no se encuentre quiere decir que no se puede cambiar ningún AS de valor
                    // por lo que perdió
                    Status = Status.Lost;
                }
                
               
            }

            // se verifica si hay 5 cartas menores que 21, se hace aquí porque el AS pudo haber cambiado
            if (Cards.Count == 5 && SumOfCards < 21)
            {
                Status = Status.FiveCards;
            }

            // se verifica el total, si es 21 y que no sea Black Jack para que no lo sobreescriba
            if (SumOfCards == 21 && Status != Status.BlackJack)
            {
                // se indica el estado
                Status = Status.TwentyOne;
            }
        }
    }
}
