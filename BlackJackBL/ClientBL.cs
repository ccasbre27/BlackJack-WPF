using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BlackJackCL;
using BlackJackCL.Events;

namespace BlackJackBL
{
    public class ClientBL
    {
        // datos para configurar el servidor
        const string SERVER_ADDRESS = "localhost";
        const int PORT = 10830;

        // comunicación con el servidor
        TcpClient tcpClient;

        // Canal de comunicación
        NetworkStream networkStream;

        // permite ler datos del canal
        BinaryReader binaryReader;

        // permite escribir datos en el canal
        BinaryWriter binaryWriter;

        // información del jugador que está jugando
        Player player;

        // indica si hay una conexión activa
        bool IsConnected;

        #region eventos
        // eventos relacionados con el servidor
        public event EventHandler Connected;
        public event EventHandler Disconnected; 
        public event MessageReceivedEventHandler MessageReceived; 
        public event EventHandler ServerError;

       // eventos relacionados con el jugador
        public event EventHandler PlayerWin;
        public event EventHandler PlayerLoose;
        public event EventHandler GameTied; 
        public event EventHandler GameContinue;
        #endregion 


        public ClientBL()
        {
            player = new Player();
            IsConnected = false;
            tcpClient = null;
        }

        public void Connect()
        {
            try
            {
                // se verifica que no haya una conexión activa
                if (!IsConnected)
                {
                    // indicamos que hay una conexión activa
                    IsConnected = true;

                    // configuramos para conectarnos al servidor
                    tcpClient = new TcpClient(SERVER_ADDRESS, PORT);

                    // obtenemso el canal
                    networkStream = tcpClient.GetStream();

                    binaryReader = new BinaryReader(networkStream, Encoding.UTF8);
                    binaryWriter = new BinaryWriter(networkStream, Encoding.UTF8);

                    // esperamos la indicación del servidor
                    // nos envía un AreReady
                    Status status = (Status)Enum.Parse(typeof(Status), binaryReader.ReadByte().ToString());

                    if (status == Status.AreReady)
                    {
                        // el servidor nos envía el número de jugador
                        // por lo que lo guardamos
                        player.IdPlayer = binaryReader.ReadInt32();

                        // el servidor nos preguntó si estabamos listos, indicamos que si
                        binaryWriter.Write((Byte) Status.ReadyToPlay);
                        binaryWriter.Write(player.IdPlayer);
                        binaryWriter.Flush();

                        //Esperamos el ACK del servidor
                        status = (Status) Enum.Parse(typeof(Status), binaryReader.ReadByte().ToString());
                        
                        if (status == Status.Ok)
                        {
                            // Cuando el servidor nos responde el Ok podemos iniciar a jugar

                            // indicamos que nos hemos conectado
                            OnConnected(); 

                            OnMessageReceived(new MessageEventArgs(new Message( player.IdPlayer, Status.Ok)));
                            
                            // iniciamos la escucha de mensajes y los procesamos
                            StartToListen();
                        }
                        else
                        {
                            Disconnect();
                        }
                    }

                }
            }
            catch (Exception)
            {
                OnServerError();
            }
        }

        /// <summary>
        /// Este método procesa los mensajes
        /// </summary>
        private void StartToListen()
        {
            try
            {
                int amountOfBytesToRead = 0;
                Message message = new Message();

                // se ejecuta mientras se encuentre conectado
                while (IsConnected)
                {
                    Status status = (Status) Enum.Parse(typeof(Status), binaryReader.ReadByte().ToString());

                    switch (status)
                    {
                   
                        case Status.ReadyToPlay:
                            amountOfBytesToRead = binaryReader.ReadInt32();
                            // se obtiene el mensaje que el servidor envía
                            message = (Message) Helper.ByteArrayToObject(binaryReader.ReadBytes(amountOfBytesToRead));
                            OnGameContinue();
                            break;

                        case Status.Deal:
                            amountOfBytesToRead = binaryReader.ReadInt32();
                            message = (Message) Helper.ByteArrayToObject(binaryReader.ReadBytes(amountOfBytesToRead));
                            OnMessageReceived( new MessageEventArgs(message));
                            break;
                     
                        case Status.Wins:
                            amountOfBytesToRead = binaryReader.ReadInt32();
                            message = (Message) Helper.ByteArrayToObject(binaryReader.ReadBytes(amountOfBytesToRead));
                            OnPlayerWins(); 
                            break;

                        case Status.GameOver:
                            amountOfBytesToRead = binaryReader.ReadInt32();
                            message = (Message) Helper.ByteArrayToObject(binaryReader.ReadBytes(amountOfBytesToRead));
                            OnPlayerLoose(); 
                            break;

                        case Status.Tie:
                            amountOfBytesToRead = binaryReader.ReadInt32();
                            message = (Message) Helper.ByteArrayToObject(binaryReader.ReadBytes(amountOfBytesToRead));
                            OnGameTied();
                            break;

                        case Status.Error:
                            OnServerError();
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception e)
            {
               
            }
        }

        /// <summary>
        /// Envía un mensaje al servidor
        /// </summary>
        /// <param name="status">Mensaje a enviar</param>
        public void SendMessage(Status status)
        {
            // se verifica si está conectado
            if (IsConnected)
            {
                try
                {
                    binaryWriter.Write((Byte) status);
                    binaryWriter.Write(player.IdPlayer);
                    binaryWriter.Flush();


                }
                catch (Exception e)
                {
                    OnServerError();
                }
            }
            else
            {
                OnServerError();
            }
        }

        public void Disconnect()
        {
            // se verifica si el jugador está conectado
            if (IsConnected)
            {
                IsConnected = false;

                tcpClient.Close();
                networkStream.Close();
                binaryReader.Close();
                binaryWriter.Close();
                
                // lanzamos el evento que indica que se ha desconectado
                OnDisconnected();
            }
        }


        #region validación y registro de eventos
        private void OnConnected()
        {
            if (Connected != null)
            {
                Connected(this, EventArgs.Empty);
            }
        }

        private void OnDisconnected()
        {
            if (Disconnected != null)
            {
                Disconnected(this, EventArgs.Empty);
            }
        }

        private void OnMessageReceived(MessageEventArgs e)
        {
            if (MessageReceived != null)
            {
                MessageReceived(this, e);
            }
                
        }

        private void OnServerError()
        {
            if (ServerError != null)
            {
                ServerError(this, EventArgs.Empty);
            }
        }

        private void OnPlayerWins()
        {
            if (PlayerWin != null)
            {
                PlayerWin(this, EventArgs.Empty);
            }
        }

        private void OnPlayerLoose()
        {
            if (PlayerLoose != null)
            {
                PlayerLoose(this, EventArgs.Empty);
            }
        }

        private void OnGameTied()
        {
            if (GameTied != null)
            {
                GameTied(this, EventArgs.Empty);
            }
        }


        private void OnGameContinue()
        {
            if (GameContinue != null)
            {
                GameContinue(this, EventArgs.Empty);
            }   
        }

 

        #endregion
    }
}
