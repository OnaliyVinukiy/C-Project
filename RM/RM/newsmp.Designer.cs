namespace RM
{
    partial class newsmp
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
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.btnAdd.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnAdd.HoverState.ImageSize = new System.Drawing.Size(57, 57);
            this.btnAdd.ImageFlip = Guna.UI2.WinForms.Enums.FlipOrientation.Normal;
            this.btnAdd.PressedState.ImageSize = new System.Drawing.Size(55, 55);
            // 
            // lblSearch
            // 
            this.lblSearch.Location = new System.Drawing.Point(857, 40);
            this.lblSearch.Size = new System.Drawing.Size(70, 27);
            // 
            // newsmp
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1200, 759);
            this.Font = new System.Drawing.Font("Segoe Script", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "newsmp";
            this.Text = "newsmp";
            this.Load += new System.EventHandler(this.newsmp_Load);
            this.Controls.SetChildIndex(this.lblSearch, 0);
            this.Controls.SetChildIndex(this.btnAdd, 0);
            this.Controls.SetChildIndex(this.lblHeader, 0);
            this.Controls.SetChildIndex(this.guna2Separator1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}