namespace Client
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStay = new System.Windows.Forms.Button();
            this.btnGetCard = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblGameStarted = new System.Windows.Forms.Label();
            this.lblSumOfCards = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblCards = new System.Windows.Forms.Label();
            this.lblIdPlayer = new System.Windows.Forms.Label();
            this.btnEnd = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnEnd);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnStay);
            this.groupBox1.Controls.Add(this.btnGetCard);
            this.groupBox1.Controls.Add(this.btnConnect);
            this.groupBox1.Location = new System.Drawing.Point(12, 269);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(590, 169);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 24);
            this.label1.TabIndex = 6;
            this.label1.Text = "Seleccione una acción";
            // 
            // btnStay
            // 
            this.btnStay.Enabled = false;
            this.btnStay.Location = new System.Drawing.Point(303, 97);
            this.btnStay.Name = "btnStay";
            this.btnStay.Size = new System.Drawing.Size(162, 51);
            this.btnStay.TabIndex = 2;
            this.btnStay.Text = "Quedarse";
            this.btnStay.UseVisualStyleBackColor = true;
            this.btnStay.Click += new System.EventHandler(this.btnStay_Click);
            // 
            // btnGetCard
            // 
            this.btnGetCard.Enabled = false;
            this.btnGetCard.Location = new System.Drawing.Point(303, 31);
            this.btnGetCard.Name = "btnGetCard";
            this.btnGetCard.Size = new System.Drawing.Size(162, 51);
            this.btnGetCard.TabIndex = 1;
            this.btnGetCard.Text = "Solicitar Carta";
            this.btnGetCard.UseVisualStyleBackColor = true;
            this.btnGetCard.Click += new System.EventHandler(this.btnGetCard_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(112, 31);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(162, 51);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Conectar";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lblGameStarted
            // 
            this.lblGameStarted.AutoSize = true;
            this.lblGameStarted.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGameStarted.Location = new System.Drawing.Point(12, 22);
            this.lblGameStarted.Name = "lblGameStarted";
            this.lblGameStarted.Size = new System.Drawing.Size(156, 18);
            this.lblGameStarted.TabIndex = 1;
            this.lblGameStarted.Text = "El juego no ha iniciado";
            // 
            // lblSumOfCards
            // 
            this.lblSumOfCards.AutoSize = true;
            this.lblSumOfCards.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSumOfCards.Location = new System.Drawing.Point(405, 242);
            this.lblSumOfCards.Name = "lblSumOfCards";
            this.lblSumOfCards.Size = new System.Drawing.Size(105, 18);
            this.lblSumOfCards.TabIndex = 2;
            this.lblSumOfCards.Text = "Total Cartas: 0";
            this.lblSumOfCards.Visible = false;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(25, 242);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(160, 18);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Estado: Desconectado";
            // 
            // lblCards
            // 
            this.lblCards.AutoSize = true;
            this.lblCards.Location = new System.Drawing.Point(26, 62);
            this.lblCards.Name = "lblCards";
            this.lblCards.Size = new System.Drawing.Size(0, 13);
            this.lblCards.TabIndex = 4;
            // 
            // lblIdPlayer
            // 
            this.lblIdPlayer.AutoSize = true;
            this.lblIdPlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIdPlayer.Location = new System.Drawing.Point(498, 22);
            this.lblIdPlayer.Name = "lblIdPlayer";
            this.lblIdPlayer.Size = new System.Drawing.Size(74, 18);
            this.lblIdPlayer.TabIndex = 5;
            this.lblIdPlayer.Text = "Jugador #";
            // 
            // btnEnd
            // 
            this.btnEnd.Enabled = false;
            this.btnEnd.Location = new System.Drawing.Point(112, 97);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(162, 51);
            this.btnEnd.TabIndex = 7;
            this.btnEnd.Text = "Finalizar";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Click += new System.EventHandler(this.btnEnd_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 461);
            this.Controls.Add(this.lblIdPlayer);
            this.Controls.Add(this.lblCards);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblSumOfCards);
            this.Controls.Add(this.lblGameStarted);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Black Jack";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnStay;
        private System.Windows.Forms.Button btnGetCard;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label lblGameStarted;
        private System.Windows.Forms.Label lblSumOfCards;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblCards;
        private System.Windows.Forms.Label lblIdPlayer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEnd;
    }
}

