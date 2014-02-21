namespace PixelCalculator.Tool
{
    partial class View
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
            this.RightPixel = new System.Windows.Forms.Label();
            this.LeftPixel = new System.Windows.Forms.Label();
            this.Mode0LeftPixel = new System.Windows.Forms.TextBox();
            this.Mode0RightPixel = new System.Windows.Forms.TextBox();
            this.Mode0Result = new System.Windows.Forms.TextBox();
            this.Result = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Mode1Pixel0 = new System.Windows.Forms.TextBox();
            this.Mode1Result = new System.Windows.Forms.TextBox();
            this.Mode1Pixel1 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.Mode1Pixel2 = new System.Windows.Forms.TextBox();
            this.Mode1Pixel3 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.Close = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // RightPixel
            // 
            this.RightPixel.AutoSize = true;
            this.RightPixel.Location = new System.Drawing.Point(103, 16);
            this.RightPixel.Name = "RightPixel";
            this.RightPixel.Size = new System.Drawing.Size(87, 13);
            this.RightPixel.TabIndex = 2;
            this.RightPixel.Text = "Pixel1 (Pen 0-15)";
            // 
            // LeftPixel
            // 
            this.LeftPixel.AutoSize = true;
            this.LeftPixel.Location = new System.Drawing.Point(6, 16);
            this.LeftPixel.Name = "LeftPixel";
            this.LeftPixel.Size = new System.Drawing.Size(87, 13);
            this.LeftPixel.TabIndex = 3;
            this.LeftPixel.Text = "Pixel0 (Pen 0-15)";
            // 
            // Mode0LeftPixel
            // 
            this.Mode0LeftPixel.Location = new System.Drawing.Point(9, 32);
            this.Mode0LeftPixel.Name = "Mode0LeftPixel";
            this.Mode0LeftPixel.Size = new System.Drawing.Size(72, 20);
            this.Mode0LeftPixel.TabIndex = 4;
            this.Mode0LeftPixel.TextChanged += new System.EventHandler(this.Mode0LeftPixel_TextChanged);
            // 
            // Mode0RightPixel
            // 
            this.Mode0RightPixel.Location = new System.Drawing.Point(106, 32);
            this.Mode0RightPixel.Name = "Mode0RightPixel";
            this.Mode0RightPixel.Size = new System.Drawing.Size(72, 20);
            this.Mode0RightPixel.TabIndex = 5;
            this.Mode0RightPixel.TextChanged += new System.EventHandler(this.Mode0RightPixel_TextChanged);
            // 
            // Mode0Result
            // 
            this.Mode0Result.Location = new System.Drawing.Point(203, 32);
            this.Mode0Result.Name = "Mode0Result";
            this.Mode0Result.Size = new System.Drawing.Size(72, 20);
            this.Mode0Result.TabIndex = 7;
            // 
            // Result
            // 
            this.Result.AutoSize = true;
            this.Result.Location = new System.Drawing.Point(200, 16);
            this.Result.Name = "Result";
            this.Result.Size = new System.Drawing.Size(37, 13);
            this.Result.TabIndex = 6;
            this.Result.Text = "Result";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(87, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "+";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(184, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "=";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.LeftPixel);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.RightPixel);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.Mode0LeftPixel);
            this.groupBox1.Controls.Add(this.Mode0Result);
            this.groupBox1.Controls.Add(this.Mode0RightPixel);
            this.groupBox1.Controls.Add(this.Result);
            this.groupBox1.Location = new System.Drawing.Point(8, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(478, 65);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mode 0";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.Mode1Pixel2);
            this.groupBox2.Controls.Add(this.Mode1Pixel3);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.Mode1Pixel0);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.Mode1Pixel1);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.Mode1Result);
            this.groupBox2.Location = new System.Drawing.Point(8, 96);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(478, 65);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mode 1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Pixel0 (Pen 0-3)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(378, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "=";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(87, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(13, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "+";
            // 
            // Mode1Pixel0
            // 
            this.Mode1Pixel0.Location = new System.Drawing.Point(9, 32);
            this.Mode1Pixel0.Name = "Mode1Pixel0";
            this.Mode1Pixel0.Size = new System.Drawing.Size(72, 20);
            this.Mode1Pixel0.TabIndex = 12;
            this.Mode1Pixel0.TextChanged += new System.EventHandler(this.Mode1Pixel0_TextChanged);
            // 
            // Mode1Result
            // 
            this.Mode1Result.Location = new System.Drawing.Point(397, 32);
            this.Mode1Result.Name = "Mode1Result";
            this.Mode1Result.Size = new System.Drawing.Size(72, 20);
            this.Mode1Result.TabIndex = 15;
            // 
            // Mode1Pixel1
            // 
            this.Mode1Pixel1.Location = new System.Drawing.Point(106, 32);
            this.Mode1Pixel1.Name = "Mode1Pixel1";
            this.Mode1Pixel1.Size = new System.Drawing.Size(72, 20);
            this.Mode1Pixel1.TabIndex = 13;
            this.Mode1Pixel1.TextChanged += new System.EventHandler(this.Mode1Pixel1_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(394, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Result";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(103, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Pixel1 (Pen 0-3)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(297, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Pixel3 (Pen 0-3)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(200, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Pixel2 (Pen 0-3)";
            // 
            // Mode1Pixel2
            // 
            this.Mode1Pixel2.Location = new System.Drawing.Point(203, 32);
            this.Mode1Pixel2.Name = "Mode1Pixel2";
            this.Mode1Pixel2.Size = new System.Drawing.Size(72, 20);
            this.Mode1Pixel2.TabIndex = 20;
            this.Mode1Pixel2.TextChanged += new System.EventHandler(this.Mode1Pixel2_TextChanged);
            // 
            // Mode1Pixel3
            // 
            this.Mode1Pixel3.Location = new System.Drawing.Point(300, 32);
            this.Mode1Pixel3.Name = "Mode1Pixel3";
            this.Mode1Pixel3.Size = new System.Drawing.Size(72, 20);
            this.Mode1Pixel3.TabIndex = 21;
            this.Mode1Pixel3.TextChanged += new System.EventHandler(this.Mode1Pixel3_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(281, 35);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(13, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "+";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(184, 35);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(13, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "+";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.BackColor = System.Drawing.Color.LightGreen;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(8, 9);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(478, 13);
            this.label12.TabIndex = 12;
            this.label12.Text = "Pixels from LEFT to RIGHT";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Close
            // 
            this.Close.Location = new System.Drawing.Point(411, 172);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(75, 23);
            this.Close.TabIndex = 13;
            this.Close.Text = "OK";
            this.Close.UseVisualStyleBackColor = true;
            this.Close.Click += new System.EventHandler(this.Close_Click_1);
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 207);
            this.Controls.Add(this.Close);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "View";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Pixel Calculator";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label RightPixel;
        private System.Windows.Forms.Label LeftPixel;
        private System.Windows.Forms.TextBox Mode0LeftPixel;
        private System.Windows.Forms.TextBox Mode0RightPixel;
        private System.Windows.Forms.TextBox Mode0Result;
        private System.Windows.Forms.Label Result;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox Mode1Pixel2;
        private System.Windows.Forms.TextBox Mode1Pixel3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Mode1Pixel0;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox Mode1Pixel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox Mode1Result;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button Close;
    }
}