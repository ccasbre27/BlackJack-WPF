using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackCL
{
    public class Message
    {
        public int IdPlayer { get; set; }

        public Card CurrentCard { get; set; }

        public Status Status { get; set; }

        public int DeckSum { get; set; }

        public Message()
        {
            IdPlayer = 0;
            CurrentCard = new Card(CardSuit.Undefined, CardValue.Undefined);
            Status = Status.Undefined;
            DeckSum = 0;
        }

        public Message(int idPlayer, Card currentCard, Status typeOfMessage, int deckSum)
        {
            this.IdPlayer = idPlayer;
            this.CurrentCard = currentCard;
            this.Status = typeOfMessage;
            this.DeckSum = deckSum;
        }

        public Message(int idPlayer, Card currentCard, Status typeOfMessage)
        {
            this.IdPlayer = idPlayer;
            this.CurrentCard = currentCard;
            this.Status = typeOfMessage;
            this.DeckSum = 0;
        }

        public Message(int idPlayer, Status typeOfMessage)
        {
            this.IdPlayer = idPlayer;
            this.CurrentCard = new Card();
            this.Status = typeOfMessage;
            this.DeckSum = 0;
        }

        public Message(Status typeOfMessage)
        {
            this.IdPlayer = 0;
            this.CurrentCard = new Card();
            this.Status = typeOfMessage;
            this.DeckSum = 0;
        }
        
    }
}
