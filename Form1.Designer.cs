namespace NamaProyekAnda
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox textBoxkey;
        private System.Windows.Forms.PictureBox pictureBoxori;
        private System.Windows.Forms.Button buttonPilih;
        private System.Windows.Forms.Button buttonEnkripsi;
        private System.Windows.Forms.Button buttonDekripsi;

        private void InitializeComponent()
        {
            this.textBoxkey = new System.Windows.Forms.TextBox();
            this.pictureBoxori = new System.Windows.Forms.PictureBox();
            this.buttonPilih = new System.Windows.Forms.Button();
            this.buttonEnkripsi = new System.Windows.Forms.Button();
            this.buttonDekripsi = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxori)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxkey
            // 
            this.textBoxkey.Location = new System.Drawing.Point(12, 12);
            this.textBoxkey.Name = "textBoxkey";
            this.textBoxkey.Size = new System.Drawing.Size(260, 20);
            this.textBoxkey.TabIndex = 0;
            // 
            // pictureBoxori
            // 
            this.pictureBoxori.Location = new System.Drawing.Point(12, 38);
            this.pictureBoxori.Name = "pictureBoxori";
            this.pictureBoxori.Size = new System.Drawing.Size(260, 210);
            this.pictureBoxori.TabIndex = 1;
            this.pictureBoxori.TabStop = false;
            // 
            // buttonPilih
            // 
            this.buttonPilih.Location = new System.Drawing.Point(12, 255);
            this.buttonPilih.Name = "buttonPilih";
            this.buttonPilih.Size = new System.Drawing.Size(75, 23);
            this.buttonPilih.TabIndex = 2;
            this.buttonPilih.Text = "Pilih Gambar";
            this.buttonPilih.UseVisualStyleBackColor = true;
            this.buttonPilih.Click += new System.EventHandler(this.buttonPilih_Click);
            // 
            // buttonEnkripsi
            // 
            this.buttonEnkripsi.Location = new System.Drawing.Point(93, 255);
            this.buttonEnkripsi.Name = "buttonEnkripsi";
            this.buttonEnkripsi.Size = new System.Drawing.Size(75, 23);
            this.buttonEnkripsi.TabIndex = 3;
            this.buttonEnkripsi.Text = "Enkripsi";
            this.buttonEnkripsi.UseVisualStyleBackColor = true;
            this.buttonEnkripsi.Click += new System.EventHandler(this.buttonEnkripsi_Click);
            // 
            // buttonDekripsi
            // 
            this.buttonDekripsi.Location = new System.Drawing.Point(174, 255);
            this.buttonDekripsi.Name = "buttonDekripsi";
            this.buttonDekripsi.Size = new System.Drawing.Size(75, 23);
            this.buttonDekripsi.TabIndex = 4;
            this.buttonDekripsi.Text = "Dekripsi";
            this.buttonDekripsi.UseVisualStyleBackColor = true;
            this.buttonDekripsi.Click += new System.EventHandler(this.buttonDekripsi_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 291);
            this.Controls.Add(this.buttonDekripsi);
            this.Controls.Add(this.buttonEnkripsi);
            this.Controls.Add(this.buttonPilih);
            this.Controls.Add(this.pictureBoxori);
            this.Controls.Add(this.textBoxkey);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxori)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
