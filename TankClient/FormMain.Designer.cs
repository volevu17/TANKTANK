namespace TankTank
{
    partial class FormMain
    {

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.GameStage = new System.Windows.Forms.PictureBox();
            this.pnlConnect = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblState = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.GameStage)).BeginInit();
            this.pnlConnect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // GameStage
            // 
            this.GameStage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("GameStage.BackgroundImage")));
            this.GameStage.Location = new System.Drawing.Point(0, 0);
            this.GameStage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GameStage.Name = "GameStage";
            this.GameStage.Size = new System.Drawing.Size(666, 557);
            this.GameStage.TabIndex = 0;
            this.GameStage.TabStop = false;
            this.GameStage.Click += new System.EventHandler(this.GameStage_Click);
            this.GameStage.Paint += new System.Windows.Forms.PaintEventHandler(this.GameStage_Paint);
            // 
            // pnlConnect
            // 
            this.pnlConnect.BackColor = System.Drawing.Color.White;
            this.pnlConnect.Controls.Add(this.pictureBox1);
            this.pnlConnect.Controls.Add(this.label4);
            this.pnlConnect.Controls.Add(this.txtName);
            this.pnlConnect.Controls.Add(this.lblState);
            this.pnlConnect.Controls.Add(this.btnConnect);
            this.pnlConnect.Controls.Add(this.txtPort);
            this.pnlConnect.Controls.Add(this.label2);
            this.pnlConnect.Controls.Add(this.txtIP);
            this.pnlConnect.Controls.Add(this.label1);
            this.pnlConnect.Location = new System.Drawing.Point(195, 83);
            this.pnlConnect.Name = "pnlConnect";
            this.pnlConnect.Size = new System.Drawing.Size(281, 354);
            this.pnlConnect.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(57, 272);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 16);
            this.label4.TabIndex = 17;
            this.label4.Text = "NAME:";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(111, 270);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 22);
            this.txtName.TabIndex = 16;
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.ForeColor = System.Drawing.Color.Red;
            this.lblState.Location = new System.Drawing.Point(74, 316);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(0, 16);
            this.lblState.TabIndex = 14;
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.Black;
            this.btnConnect.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.btnConnect.Location = new System.Drawing.Point(32, 307);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(182, 25);
            this.btnConnect.TabIndex = 12;
            this.btnConnect.Text = "CONNECT";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(111, 233);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(85, 22);
            this.txtPort.TabIndex = 11;
            this.txtPort.Text = "7776";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(60, 237);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 16);
            this.label2.TabIndex = 10;
            this.label2.Text = "PORT:";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(112, 201);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(102, 22);
            this.txtIP.TabIndex = 9;
            this.txtIP.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 205);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "SERVER IP:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.Location = new System.Drawing.Point(86, 56);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.TabIndex = 18;
            this.pictureBox1.TabStop = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(667, 556);
            this.Controls.Add(this.pnlConnect);
            this.Controls.Add(this.GameStage);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormMain";
            this.Text = "tanktank";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormMain_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.GameStage)).EndInit();
            this.pnlConnect.ResumeLayout(false);
            this.pnlConnect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.PictureBox GameStage;
        private System.Windows.Forms.Panel pnlConnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

