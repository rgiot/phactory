namespace Project.View
{
    partial class FindDialog
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
            this.BtnClose = new System.Windows.Forms.Button();
            this.BtnFindNext = new System.Windows.Forms.Button();
            this.filter = new System.Windows.Forms.TextBox();
            this.ignoreCase = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // BtnClose
            // 
            this.BtnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnClose.Location = new System.Drawing.Point(376, 47);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(75, 23);
            this.BtnClose.TabIndex = 7;
            this.BtnClose.Text = "Close";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // BtnFindNext
            // 
            this.BtnFindNext.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnFindNext.Enabled = false;
            this.BtnFindNext.Location = new System.Drawing.Point(295, 47);
            this.BtnFindNext.Name = "BtnFindNext";
            this.BtnFindNext.Size = new System.Drawing.Size(75, 23);
            this.BtnFindNext.TabIndex = 6;
            this.BtnFindNext.Text = "Find Next";
            this.BtnFindNext.UseVisualStyleBackColor = true;
            this.BtnFindNext.Click += new System.EventHandler(this.BtnFindNext_Click);
            // 
            // filter
            // 
            this.filter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.filter.Location = new System.Drawing.Point(12, 11);
            this.filter.Name = "filter";
            this.filter.Size = new System.Drawing.Size(438, 20);
            this.filter.TabIndex = 4;
            this.filter.TextChanged += new System.EventHandler(this.filter_TextChanged);
            this.filter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.filter_KeyDown);
            // 
            // ignoreCase
            // 
            this.ignoreCase.AutoSize = true;
            this.ignoreCase.Checked = true;
            this.ignoreCase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ignoreCase.Location = new System.Drawing.Point(12, 52);
            this.ignoreCase.Name = "ignoreCase";
            this.ignoreCase.Size = new System.Drawing.Size(82, 17);
            this.ignoreCase.TabIndex = 8;
            this.ignoreCase.Text = "Ignore case";
            this.ignoreCase.UseVisualStyleBackColor = true;
            this.ignoreCase.CheckedChanged += new System.EventHandler(this.ignoreCase_CheckedChanged);
            // 
            // FindDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 80);
            this.Controls.Add(this.ignoreCase);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.BtnFindNext);
            this.Controls.Add(this.filter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Find";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.Button BtnFindNext;
        private System.Windows.Forms.TextBox filter;
        private System.Windows.Forms.CheckBox ignoreCase;

    }
}