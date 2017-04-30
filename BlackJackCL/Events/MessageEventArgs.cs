using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackCL.Events
{
    public class MessageEventArgs
    {
        public Message GameMessage { get; set; }

        public MessageEventArgs()
        {
            GameMessage = new Message(); 
        }

        public MessageEventArgs(Message gameMessage)
        {
            this.GameMessage = gameMessage;
        }
    }

    public delegate void MessageReceivedEventHandler(object sender, MessageEventArgs e);
}
