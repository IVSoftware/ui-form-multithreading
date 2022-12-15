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
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxWithDataBinding = new System.Windows.Forms.TextBox();
            this.dataGridViewEx = new DataGridViewEx();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEx)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox
            // 
            this.richTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox.Location = new System.Drawing.Point(0, 51);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(478, 519);
            this.richTextBox.TabIndex = 0;
            this.richTextBox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Data Bound TextBox";
            // 
            // textBoxWithDataBinding
            // 
            this.textBoxWithDataBinding.Location = new System.Drawing.Point(191, 14);
            this.textBoxWithDataBinding.Name = "textBoxWithDataBinding";
            this.textBoxWithDataBinding.Size = new System.Drawing.Size(275, 31);
            this.textBoxWithDataBinding.TabIndex = 2;
            // 
            // dataGridView1
            // 
            this.dataGridViewEx.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEx.Location = new System.Drawing.Point(0, 591);
            this.dataGridViewEx.Name = "dataGridView1";
            this.dataGridViewEx.RowHeadersWidth = 62;
            this.dataGridViewEx.RowTemplate.Height = 33;
            this.dataGridViewEx.Size = new System.Drawing.Size(478, 456);
            this.dataGridViewEx.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 1043);
            this.Controls.Add(this.dataGridViewEx);
            this.Controls.Add(this.textBoxWithDataBinding);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main Form";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEx)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RichTextBox richTextBox;
        private Label label1;
        private TextBox textBoxWithDataBinding;
        private DataGridViewEx dataGridViewEx;
    }
}