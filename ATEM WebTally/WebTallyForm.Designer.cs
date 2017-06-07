namespace ATEM_WebTally
{
    partial class WebTallyForm
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
            this.ipInputLabel = new System.Windows.Forms.Label();
            this.ipInput = new System.Windows.Forms.TextBox();
            this.consoleBox = new System.Windows.Forms.ListBox();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.btnStopServer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ipInputLabel
            // 
            this.ipInputLabel.AutoSize = true;
            this.ipInputLabel.Location = new System.Drawing.Point(12, 16);
            this.ipInputLabel.Name = "ipInputLabel";
            this.ipInputLabel.Size = new System.Drawing.Size(91, 13);
            this.ipInputLabel.TabIndex = 0;
            this.ipInputLabel.Text = "IP-adres switcher:";
            // 
            // ipInput
            // 
            this.ipInput.Location = new System.Drawing.Point(109, 13);
            this.ipInput.Name = "ipInput";
            this.ipInput.Size = new System.Drawing.Size(129, 20);
            this.ipInput.TabIndex = 1;
            // 
            // consoleBox
            // 
            this.consoleBox.FormattingEnabled = true;
            this.consoleBox.Location = new System.Drawing.Point(259, 16);
            this.consoleBox.Name = "consoleBox";
            this.consoleBox.ScrollAlwaysVisible = true;
            this.consoleBox.Size = new System.Drawing.Size(201, 173);
            this.consoleBox.TabIndex = 2;
            // 
            // btnStartServer
            // 
            this.btnStartServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartServer.Location = new System.Drawing.Point(12, 95);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(99, 23);
            this.btnStartServer.TabIndex = 3;
            this.btnStartServer.Text = "Start server";
            this.btnStartServer.UseVisualStyleBackColor = true;
            // 
            // btnStopServer
            // 
            this.btnStopServer.Enabled = false;
            this.btnStopServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopServer.Location = new System.Drawing.Point(117, 95);
            this.btnStopServer.Name = "btnStopServer";
            this.btnStopServer.Size = new System.Drawing.Size(99, 23);
            this.btnStopServer.TabIndex = 4;
            this.btnStopServer.Text = "Stop server";
            this.btnStopServer.UseVisualStyleBackColor = true;
            // 
            // WebTallyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 200);
            this.Controls.Add(this.btnStopServer);
            this.Controls.Add(this.btnStartServer);
            this.Controls.Add(this.consoleBox);
            this.Controls.Add(this.ipInput);
            this.Controls.Add(this.ipInputLabel);
            this.Name = "WebTallyForm";
            this.Text = "ATEM WebTally";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ipInputLabel;
        private System.Windows.Forms.TextBox ipInput;
        private System.Windows.Forms.ListBox consoleBox;
        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.Button btnStopServer;
    }
}

