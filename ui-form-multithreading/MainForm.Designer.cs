namespace ui_form_multithreading
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridViewEx = new ui_form_multithreading.DataGridViewEx();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEx)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewEx
            // 
            this.dataGridViewEx.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewEx.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewEx.Name = "dataGridViewEx";
            this.dataGridViewEx.RowHeadersWidth = 62;
            this.dataGridViewEx.RowTemplate.Height = 33;
            this.dataGridViewEx.Size = new System.Drawing.Size(478, 744);
            this.dataGridViewEx.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 744);
            this.Controls.Add(this.dataGridViewEx);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main Form";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEx)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DataGridViewEx dataGridViewEx;
    }
}