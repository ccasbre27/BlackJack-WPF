using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJackBL;
using BlackJackCL.Events;

namespace Server_Console
{
    class Program
    {
        
        private static void Main(string[] args)
        {
            ServerBL serverBL = new ServerBL();

            // registro de eventos
            serverBL.PlayerOneConnected +=serverBL_PlayerOneConnected;
            serverBL.PlayerTwoConnected += serverBL_PlayerTwoConnected;
            serverBL.TooManyClients += serverBL_TooManyClients;

            serverBL.CardDealed += serverBL_CardDealed;
            serverBL.PlayerOneWins += serverBL_PlayerOneWins;  
            serverBL.PlayerTwoWins += serverBL_PlayerTwoWins;
            serverBL.TiedGame += serverBL_TiedGame;

            serverBL.ClientDisconnected += serverBL_ClientDisconnected;

            Console.WriteLine("Iniciando servidor espere un momento...");

            serverBL.ServerSetUp();
        }

        
        static void serverBL_PlayerOneConnected(object sender, EventArgs e)
        {
            Console.WriteLine("Jugador 1 conectado al servidor");
        }

        static void serverBL_PlayerTwoConnected(object sender, EventArgs e)
        {
            Console.WriteLine("Jugador 2 conectado al servidor");
        }

        static void serverBL_TooManyClients(object sender, EventArgs e)
        {
            Console.WriteLine("Un tercer jugador se ha intentado conectar, la conexión ha sido rechazada");
        }

        static void serverBL_CardDealed(object sender, MessageEventArgs e)
        {
            Console.WriteLine(String.Format("Carta {0} {1} entregada al jugador {2}",e.Message.CurrentCard.Suit, e.Message.CurrentCard.Value, e.Message.IdPlayer));
            
            Console.WriteLine("Suma total de cartas = " + e.Message.DeckSum);
        }

        static void serverBL_PlayerOneWins(object sender, EventArgs e)
        {
            Console.WriteLine("Jugador 1 gana el juego");
        }

        static void serverBL_PlayerTwoWins(object sender, EventArgs e)
        {
            Console.WriteLine("Jugador 2 gana el juego");
        }

        static void serverBL_TiedGame(object sender, EventArgs e)
        {
            Console.WriteLine("Juego empatado");
        }

        static void serverBL_ClientDisconnected(object sender, EventArgs e)
        {

            Console.WriteLine("Cliente desconectado del servidor!");
        }
     
    }
}
