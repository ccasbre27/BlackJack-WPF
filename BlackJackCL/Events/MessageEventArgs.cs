using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackCL.Events
{
    public class MessageEventArgs
    {
        public Message Message { get; set; }

        public MessageEventArgs()
        {
            Message = new Message(); 
        }

        public MessageEventArgs(Message gameMessage)
        {
            this.Message = gameMessage;
        }
    }

    public delegate void MessageReceivedEventHandler(object sender, MessageEventArgs e);
}
