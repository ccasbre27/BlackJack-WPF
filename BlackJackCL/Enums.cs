using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackCL
{
    [Serializable]
    public enum CardSuit
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades,
        Undefined
    }

    [Serializable]
    public enum CardValue : byte
    {
        One, 
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Ace,
        Jack,
        Queen,
        King,
        Undefined
    }

    [Serializable]
    public enum PlayerStatus
    {
        Playing,
        Stay,
        FiveCards,
        TwentyOne,
        BlackJack,
        Lost,
        Undefined
    }
}
