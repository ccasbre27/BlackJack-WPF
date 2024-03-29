﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlackJackCL;
using BlackJackCL.Events;

namespace BlackJackBL
{
    public class ServerBL
    {
        // ip del localhost
        IPAddress IP_ADDRESS = IPAddress.Parse("127.0.0.1");
    
        // puerto del servidor
        const int PORT = 10830;
       
        // indica si el servidor está corriendo
        bool IsRunning;    

        // se encarga de escuchar las conexiones
        TcpListener tcpListener;     

        // indica si los clientes están conectado
        int ConnectedClients;

        Player playerOne;
        Player playerTwo;
        
        DealerBL dealerBL;

        #region eventos

        // eventos jugador conectado/desconectado
        public event EventHandler PlayerOneConnected; 
        public event EventHandler PlayerTwoConnected; 
        public event EventHandler ClientDisconnected; 

        // se lanza cuando se intenta conectar un jugador más , solo pueden ser do
        public event EventHandler TooManyClients; 

        // lanzado al repartir una carta
        public event MessageReceivedEventHandler CardDealed;
 
        // eventos fin de juego
        public event MessageReceivedEventHandler PlayerOneWins;
        public event MessageReceivedEventHandler PlayerTwoWins;
        public event MessageReceivedEventHandler TiedGame; 
        #endregion


        #region constructor
        public ServerBL()
        {
            IsRunning = true;
            ConnectedClients = 0;
            dealerBL = new DealerBL();
            playerOne = new Player();
            playerTwo = new Player();
        }
        #endregion

        #region methods
        public void ServerSetUp()
        {
            // se configura el tcp con la ip y el puerto donde se va escuchar
            tcpListener = new TcpListener(IP_ADDRESS, PORT);
            tcpListener.Start();

            // se indica que el servidor ya está corriendo
            IsRunning = true;

            // se inicia el proceso de escuchar
            StartListen();
        }

        public void StartListen()
        {
            // se ejecuta mientras esté corriendo
            while (IsRunning)
            {
                // se verifica la cantidad de clientes conectados
                if (ConnectedClients == 2)
                {
                    // se lanza el evento que indica que ya están los dos jugadores conectados
                    OnTooManyClients();
                }
                else
                {
                    // en caso que aún no estén los jugadores y se intente conectar uno
                    // se acepta la conexión y la obtenemos
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    // se hace un lock de la cantidad de jugadores ya que pueden estar ingresando los dos jugadores
                    // al mismo tiempo
                    Interlocked.Increment(ref ConnectedClients);

                    // se verifica cuál jugador se está conectando
                    switch (ConnectedClients)
                    {
                        case 1:
                            // se guarda la conexión
                            playerOne.TcpConnection = tcpClient;
                            
                            // se dispara el evento
                            OnPlayerOneConnected();

                            break;

                        case 2:
                            // se guarda la conexión
                            playerTwo.TcpConnection = tcpClient;

                            // se dispara el evento
                            OnPlayerTwoConnected();
                            break;

                        default:
                            break;
                    }

                    // se inicia la configuración para el cliente
                    ClientSetup(ConnectedClients, tcpClient);
                }
            }
        }

        public void ClientSetup(int numberOfPlayer, TcpClient clientTCP)
        {

            try
            {
                // Obtenemos el canal de comunicación
                NetworkStream networkStream = clientTCP.GetStream();

                // permite ler datos del canal
                BinaryReader binaryReader = new BinaryReader(networkStream, Encoding.UTF8);

                // permite escribir datos en el canal
                BinaryWriter binaryWriter = new BinaryWriter(networkStream, Encoding.UTF8);

                // enviamos un aviso al cliente
                binaryWriter.Write((Byte) (Status.AreReady));
                binaryWriter.Write(numberOfPlayer);

                // limpiamos el buffer
                binaryWriter.Flush();

                // esperamos que el jugador nos indique que está listo
                // se lee lo que 
                Status clientMessage = (Status) Enum.Parse(typeof(Status), binaryReader.ReadByte().ToString());
                int gamePlayer = binaryReader.ReadInt32();

                // Se recibió un mensaje de "Ready". 
                if (numberOfPlayer == gamePlayer)
                {
                    // indicamos que se ha recibido el mensaje
                    binaryWriter.Write((Byte) Status.Ok);
                    binaryWriter.Flush();
                    
                    StartToPlay(ref clientTCP, ref binaryReader, ref binaryWriter);
                }

               
                   
               
            }
            catch (Exception ex)
            {
               
            }
        }

        private void StartToPlay(ref TcpClient tcpClient, ref BinaryReader binaryReader, ref BinaryWriter binaryWriter)
        {
            Message message = new Message();
            Card card = new Card();

            try
            {
                // se ejecuta mientras el cliente se encuentre conectado
                while (tcpClient.Client.Connected)  
                {
                    // se obtiene el tipo de mensaje
                    Status Status = (Status) Enum.Parse(typeof(Status), binaryReader.ReadByte().ToString());

                    int currentNumberOfPlayer = binaryReader.ReadInt32();

                    #region obtener carta
                    switch (Status)
                    {
                   
                        case Status.Deal:

                            lock (dealerBL)
                            {
                                // se obtiene la carta
                                card = dealerBL.GetCard();

                            }

                            // se verifica el número de jugador
                            if (currentNumberOfPlayer == 1)
                            {
                                // se agrega al deck del jugador
                                playerOne.AddCard(card);
                            }
                            else if (currentNumberOfPlayer == 2)
                            {
                                // se agrega al deck del jugador
                                playerTwo.AddCard(card);
                            }

                            // se establece el mensaje que se va enviar
                            message = new Message(currentNumberOfPlayer, card, Status.Deal);

                            // se envía el mensaje
                            SendMessage(message);

                            // se dispara el evento que indica que se entrégó una carta
                            OnCardDealed(new MessageEventArgs(message));

                            break;

                        case Status.Stay:

                            // se verifica el número de jugador
                            if (currentNumberOfPlayer == 1)
                            {
                                // se indica que ya no está jugando
                                playerOne.IsPlaying = false;

                                playerOne.Status = Status.Stay;
                                
                            }
                            else if (currentNumberOfPlayer == 2)
                            {
                                // se indica que ya no está jugando
                                playerTwo.IsPlaying = false;

                                playerTwo.Status = Status.Stay;
                                
                            }
                            
                            
                            break;
                        default:
                            break;
                    }
                    #endregion 

                    #region obtener estado actual del juego
                    // se obtiene el estado actual del juego
                    Status result = GetGameStatus();

                    Status StatusPlayerOne = Status.Undefined;
                    Status StatusPlayerTwo = Status.Undefined;

                    switch (result)
                    {
                        // jugador uno gana
                        case Status.PlayerOneWins:

                            StatusPlayerOne = Status.Wins;

                            StatusPlayerTwo = Status.GameOver;

                            // finish
                            break;

                        // jugador dos gana
                        case Status.PlayerTwoWins:

                            StatusPlayerTwo = Status.Wins;

                            StatusPlayerOne = Status.GameOver;
                            
                            //Finish();
                            break;

                        // empate
                        case Status.Tie:
                            StatusPlayerOne = Status.Tie;

                            StatusPlayerTwo = Status.Tie;
                            //Finish();
                            break;

                        case Status.Continue:
                            StatusPlayerOne = Status.ReadyToPlay;

                            StatusPlayerTwo = Status.ReadyToPlay;
                            break;

                        default:
                            break;
                    }

                    // send message to player one and player two
                    
                    SendMessage(new Message(1, StatusPlayerOne));
                    SendMessage(new Message(2, StatusPlayerTwo));

                    #endregion
                }
            }
            catch (Exception ex)
            {
                binaryWriter.Write((Byte) Status.Error);
                binaryWriter.Flush();
            }
        }

        public void SendMessage(Message clientMessage)
        {
            // canal para enviar datos al cliente
            NetworkStream networkStream = null;       
           
            BinaryWriter bianryWriter;   

            // según el cliente debemos de abrir cierto canal de comunicación
            if (clientMessage.IdPlayer == 1)
            {
                // se obtiene el canal de comunicación
                networkStream = playerOne.TcpConnection.GetStream(); 
            }
            else if (clientMessage.IdPlayer == 2)
            {
                // se obtiene el canal de comunicación
                networkStream = playerTwo.TcpConnection.GetStream(); 
            }

            bianryWriter = new BinaryWriter(networkStream, Encoding.UTF8);
            
            // se prepara el mensaje
            bianryWriter.Write((Byte) clientMessage.Status);
            bianryWriter.Write(Helper.ObjectToByteArray(clientMessage).Length);
            bianryWriter.Write(Helper.ObjectToByteArray(clientMessage));

            // se limpia el buffer
            bianryWriter.Flush();

        }

        private Status GetGameStatus()
        {
            lock (this)
            {
                // se verifica que ya ninguno esté jugando
                if (!playerOne.IsPlaying && !playerTwo.IsPlaying)
                {
                    // se evalua cada posible estado del juego
                    switch (playerOne.Status)
                    {
                        case Status.FiveCards:

                            // se evalúan las cartas del jugador dos
                            switch (playerTwo.Status)
	                        {

                                // si el jugador dos tiene igual 5 cartas menores que 21 hay empate
                                case Status.FiveCards:
                                    OnGameTied(new MessageEventArgs(new Message(Status.FiveCards)));
                                    return Status.Tie;

                                // en caso contrario el jugador uno gana
                                default:
                                    OnPlayerOneWins(new MessageEventArgs(new Message(Status.FiveCards)));
                                    return Status.PlayerOneWins;
                                    
                            }


                        case Status.BlackJack:

                            // se evalúan las cartas del jugador dos
                            switch (playerTwo.Status)
                            {

                                case Status.FiveCards:
                                    OnPlayerTwoWins(new MessageEventArgs(new Message(Status.FiveCards)));
                                    return Status.PlayerTwoWins;

                                case Status.BlackJack:
                                    OnGameTied(new MessageEventArgs(new Message(Status.BlackJack)));
                                    return Status.Tie;

                                // en caso contrario el jugador uno gana
                                default:
                                    OnPlayerOneWins(new MessageEventArgs(new Message(Status.BlackJack)));
                                    return Status.PlayerOneWins;
                            }

                        case Status.TwentyOne:
                        
                            // se evalúan las cartas del jugador dos
                            switch (playerTwo.Status)
                            {

                                case Status.FiveCards:
                                    OnPlayerTwoWins(new MessageEventArgs(new Message(Status.FiveCards)));
                                    return Status.PlayerTwoWins;

                                case Status.BlackJack:
                                    OnPlayerTwoWins(new MessageEventArgs(new Message(Status.BlackJack)));
                                    return Status.PlayerTwoWins;

                                case Status.TwentyOne:
                                    OnGameTied(new MessageEventArgs(new Message(Status.TwentyOne)));
                                    return Status.Tie;

                                // en caso contrario el jugador uno gana
                                default:
                                    OnPlayerOneWins(new MessageEventArgs(new Message(Status.TwentyOne)));
                                    return Status.PlayerOneWins;
                            }


                        case Status.Stay:

                            // se evalúan las cartas del jugador dos
                            switch (playerTwo.Status)
                            {

                                case Status.FiveCards:
                                    OnPlayerTwoWins(new MessageEventArgs(new Message(Status.FiveCards)));
                                    return Status.PlayerTwoWins;

                                case Status.BlackJack:
                                    OnPlayerTwoWins(new MessageEventArgs(new Message(Status.BlackJack)));
                                    return Status.PlayerTwoWins;

                                case Status.TwentyOne:
                                    OnPlayerTwoWins(new MessageEventArgs(new Message(Status.TwentyOne)));
                                    return Status.PlayerTwoWins;

                                case Status.Stay:
                                    if (playerOne.SumOfCards > playerTwo.SumOfCards)
                                    {

                                        OnPlayerOneWins(new MessageEventArgs(new Message(Status.CloserTo21)));
                                        return Status.PlayerOneWins;
                                    }
                                    else
                                    {
                                        if (playerOne.SumOfCards < playerTwo.SumOfCards)
                                        {
                                            OnPlayerTwoWins(new MessageEventArgs(new Message(Status.CloserTo21)));
                                            return Status.PlayerTwoWins;
                                            
                                        }
                                        else
                                        {

                                            OnGameTied(new MessageEventArgs(new Message(Status.Tie)));
                                            return Status.Tie;
                                        }
                                    }

                                case Status.Lost:
                                    OnPlayerOneWins(new MessageEventArgs(new Message(Status.CloserTo21)));
                                    return Status.PlayerOneWins;

                               
                                default:
                                    return Status.Continue;
                            }

                                                
                        case Status.Lost:

                            // se evalúan las cartas del jugador dos
                            switch (playerTwo.Status)
                            {

                                case Status.Lost:
                                    OnGameTied(new MessageEventArgs(new Message(Status.Tie)));
                                    return Status.Tie;

                                // en caso contrario el jugador dos gana
                                default:
                                    OnPlayerTwoWins(new MessageEventArgs(new Message(Status.CloserTo21)));
                                    return Status.PlayerTwoWins;      
                            }

                                         
                        default:
                            return Status.Continue;
                    }
                }
                else
                {
                    return Status.Continue;                  
                }
            }
        }
    

        #endregion

        #region validación y registro de eventos

        private void OnPlayerOneConnected()
        {
            if (PlayerOneConnected != null)
            {
                PlayerOneConnected(this, EventArgs.Empty);
            }
        }

        private void OnPlayerTwoConnected()
        {
            if (PlayerTwoConnected != null)
            {
                PlayerTwoConnected(this, EventArgs.Empty);
            }    
        }

        private void OnClientDisconnected()
        {
            if (ClientDisconnected != null)
            {
                ClientDisconnected(this, EventArgs.Empty);
            }                
        }

        private void OnPlayerOneWins(MessageEventArgs e)
        {
            if (PlayerOneWins != null)
            {
                PlayerOneWins(this, e);
            }    
        }

        private void OnPlayerTwoWins(MessageEventArgs e)
        {
            if (PlayerTwoWins != null)
            {
                PlayerTwoWins(this, e);
            }   
        }

        private void OnGameTied(MessageEventArgs e)
        {
            if (TiedGame != null)
            {
                TiedGame(this, e);
            }
        }

        private void OnTooManyClients()
        {
            if (TooManyClients != null)
            {
                TooManyClients(this, EventArgs.Empty);
            }    
        }

        private void OnCardDealed(MessageEventArgs e)
        {
            if (CardDealed != null)
            {
                CardDealed(this, e);
            }
        }
        #endregion
    }
}
