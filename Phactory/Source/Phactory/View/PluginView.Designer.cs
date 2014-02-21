namespace Project.View
{
    partial class PluginView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginView));
            this.PluginContainer = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // PluginContainer
            // 
            this.PluginContainer.BackColor = System.Drawing.SystemColors.Control;
            this.PluginContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PluginContainer.Location = new System.Drawing.Point(0, 0);
            this.PluginContainer.Name = "PluginContainer";
            this.PluginContainer.Size = new System.Drawing.Size(292, 271);
            this.PluginContainer.TabIndex = 0;
            // 
            // PluginView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 271);
            this.Controls.Add(this.PluginContainer);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PluginView";
            this.ShowInTaskbar = false;
            this.Text = "PluginView";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PluginView_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel PluginContainer;

    }
}