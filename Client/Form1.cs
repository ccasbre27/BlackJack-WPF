using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlackJackBL;
using BlackJackCL;

namespace Client
{
    public partial class Form1 : Form
    {
        ClientBL clientBL;
        Player player;

        public Form1()
        {
            InitializeComponent();

            clientBL = new ClientBL();
            player = new Player();

            lblIdPlayer.Visible = false;

            // suscripción a eventos
            clientBL.Connected += clientBL_Connected;
            clientBL.Disconnected += clientBL_Disconnected;


            clientBL.PlayerWin += clientBL_PlayerWin;
            clientBL.PlayerLoose += clientBL_PlayerLoose;
            clientBL.GameTied += clientBL_GameTied;

            clientBL.GameContinue += clientBL_GameContinue;
            
            clientBL.MessageReceived += clientBL_MessageReceived;

    
        }

    

        void clientBL_Connected(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
            {
                lblStatus.Text = "Connectado";
                
                // se habilitan los botones
                btnConnect.Enabled = false;
                btnGetCard.Enabled = true;
                btnStay.Enabled = true;
                lblIdPlayer.Visible = true;

            }));
        }

        void clientBL_Disconnected(object sender, EventArgs e)
        {
            clientBL.Disconnect();
            
        }

        void clientBL_PlayerWin(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
            {
                SetButtonsEnabled(false);
                lblStatus.Text = "Ha ganado!";
                MessageBox.Show("Ha ganado!","Jugador #" + player.IdPlayer);

            }));

        }

        void clientBL_PlayerLoose(object sender, EventArgs e)
        {
            player.Status = Status.GameOver;
            CheckGameContinue();
        }

        void clientBL_GameTied(object sender, EventArgs e)
        {
            SetButtonsEnabled(false);
            player.Status = Status.Tie;
            MessageBox.Show("Juego empatado", "Jugador #" + player.IdPlayer);
        }

        void clientBL_GameContinue(object sender, EventArgs e)
        {
            CheckGameContinue();
        }

        void clientBL_MessageReceived(object sender, BlackJackCL.Events.MessageEventArgs e)
        {
            switch (e.Message.Status)
            {
                // ya está listo para jugar
                case Status.Ok:
                    this.BeginInvoke(new MethodInvoker(delegate
                    {
                        lblIdPlayer.Text = "Jugador #" + e.Message.IdPlayer;
                        
                        // se guarda el número de jugador
                        player.IdPlayer = e.Message.IdPlayer;

                        btnGetCard.Enabled = true;
                        btnStay.Enabled = true;
                        lblSumOfCards.Visible = true;

                    }));
                    break;

                case Status.Deal:

                    this.BeginInvoke(new MethodInvoker(delegate
                    {

                        lblCards.Text += String.Format("Carta {0} {1} ", e.Message.CurrentCard.Suit, e.Message.CurrentCard.Value) + Environment.NewLine;
                        lblSumOfCards.Text = "Total cartas: " + e.Message.DeckSum;

                        // se establece la suma de cartas
                        player.SumOfCards = e.Message.DeckSum;

                    }));

                    
                    break;

            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                // se inicia en una nueva tarea el proceso de conectar al servidor
                new Task(() => clientBL.Connect()).Start();
                
                // indicamos que se está intentando conectar al servidor
                lblStatus.Text = "Conectando al servidor, espere un momento...";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error, intente de nuevo", "Jugador #" + player.IdPlayer);
            }
        }

        private void btnGetCard_Click(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
            {
                // deshabilitamos los botones hasta que se obtenga la nueva carta
                btnGetCard.Enabled = false;
                btnStay.Enabled = false;

                lblStatus.Text = "Procesando solicitud...";

            }));

            clientBL.SendMessage(Status.Deal);            
        }

        private void btnStay_Click(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
            {
                btnGetCard.Enabled = false;
                btnStay.Enabled = false;
                lblStatus.Text = "Espere mientras el otro jugador finaliza...";

                player.Status = Status.Stay;

            }));

            clientBL.SendMessage(Status.Stay);            
        }

        private void CheckGameContinue()
        {
            BeginInvoke(new MethodInvoker(delegate
            {

                if (player.Status != Status.Stay && player.Status != Status.GameOver)
                {
                    lblStatus.Text = "Presione el botón de obtener carta o queda";
                    btnGetCard.Enabled = true;
                    btnStay.Enabled = true;
                }

                // en caso que haya perdido pasandose de 21 o porque el otro jugador estuvo más cerca
                // se lo indicamos
                if (player.SumOfCards > 21 || player.Status == Status.GameOver)
                {

                    SetButtonsEnabled(false);
                    lblStatus.Text = "Ha perdido";
                    MessageBox.Show("Ha perdido!", "Jugador #" + player.IdPlayer);
                }

            }));
        }

        private void SetButtonsEnabled(bool status)
        {
            btnGetCard.Enabled = status;
            btnStay.Enabled = status;
        }
    }
}
