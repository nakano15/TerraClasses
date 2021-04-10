namespace IconTextureToByteArrayConverter
{
    partial class Main
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.convertImagebtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.codeOutputBox = new System.Windows.Forms.RichTextBox();
            this.fileOpener = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(228, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Click the button and pick the image to convert.";
            // 
            // convertImagebtn
            // 
            this.convertImagebtn.Location = new System.Drawing.Point(246, 9);
            this.convertImagebtn.Name = "convertImagebtn";
            this.convertImagebtn.Size = new System.Drawing.Size(85, 23);
            this.convertImagebtn.TabIndex = 1;
            this.convertImagebtn.Text = "Convert Image";
            this.convertImagebtn.UseVisualStyleBackColor = true;
            this.convertImagebtn.Click += new System.EventHandler(this.convertImagebtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.codeOutputBox);
            this.groupBox1.Location = new System.Drawing.Point(15, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(316, 199);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Output";
            // 
            // codeOutputBox
            // 
            this.codeOutputBox.Location = new System.Drawing.Point(6, 19);
            this.codeOutputBox.Name = "codeOutputBox";
            this.codeOutputBox.ReadOnly = true;
            this.codeOutputBox.Size = new System.Drawing.Size(304, 174);
            this.codeOutputBox.TabIndex = 0;
            this.codeOutputBox.Text = "Only try converting images 16x16.";
            // 
            // fileOpener
            // 
            this.fileOpener.FileName = "openFileDialog1";
            this.fileOpener.Filter = "PNG Images|*.png";
            this.fileOpener.FileOk += new System.ComponentModel.CancelEventHandler(this.fileOpener_FileOk);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 249);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.convertImagebtn);
            this.Controls.Add(this.label1);
            this.Name = "Main";
            this.Text = "Image to Byte Converter";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button convertImagebtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox codeOutputBox;
        private System.Windows.Forms.OpenFileDialog fileOpener;
    }
}

