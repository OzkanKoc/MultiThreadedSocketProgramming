namespace ServerSide
{
    partial class B
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
            this.grdB = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.grdB)).BeginInit();
            this.SuspendLayout();
            // 
            // grdB
            // 
            this.grdB.AllowUserToDeleteRows = false;
            this.grdB.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdB.Location = new System.Drawing.Point(0, 0);
            this.grdB.Name = "grdB";
            this.grdB.ReadOnly = true;
            this.grdB.Size = new System.Drawing.Size(800, 450);
            this.grdB.TabIndex = 0;
            // 
            // B
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.grdB);
            this.Name = "B";
            this.Text = "B";
            ((System.ComponentModel.ISupportInitialize)(this.grdB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdB;
    }
}

