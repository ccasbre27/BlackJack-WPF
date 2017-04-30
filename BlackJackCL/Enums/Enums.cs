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
    public enum Status
    {
        Undefined,
        Error,
        AreReady,
        Ready,
        Deal,
        Playing,
        Stay,
        Lost,
        Wins,
        GameOver,
        Continue,
        FiveCards,
        TwentyOne,
        BlackJack,
        PlayerOneWins,
        PlayerTwoWins,
        Tied
    }




}
