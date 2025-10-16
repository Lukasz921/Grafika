namespace Proj1.Components
{
    partial class ConstForm
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
            TLP1 = new TableLayoutPanel();
            NUP1 = new NumericUpDown();
            TLP2 = new TableLayoutPanel();
            B1 = new Button();
            B2 = new Button();
            TLP1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)NUP1).BeginInit();
            TLP2.SuspendLayout();
            SuspendLayout();
            // 
            // TLP1
            // 
            TLP1.ColumnCount = 1;
            TLP1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TLP1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            TLP1.Controls.Add(NUP1, 0, 0);
            TLP1.Location = new Point(12, 12);
            TLP1.Name = "TLP1";
            TLP1.RowCount = 1;
            TLP1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TLP1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            TLP1.Size = new Size(210, 110);
            TLP1.TabIndex = 1;
            // 
            // NUP1
            // 
            NUP1.Anchor = AnchorStyles.None;
            NUP1.Location = new Point(45, 43);
            NUP1.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            NUP1.Name = "NUP1";
            NUP1.Size = new Size(120, 23);
            NUP1.TabIndex = 0;
            // 
            // TLP2
            // 
            TLP2.ColumnCount = 2;
            TLP2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TLP2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TLP2.Controls.Add(B1, 0, 0);
            TLP2.Controls.Add(B2, 1, 0);
            TLP2.Location = new Point(12, 137);
            TLP2.Name = "TLP2";
            TLP2.RowCount = 1;
            TLP2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            TLP2.Size = new Size(210, 62);
            TLP2.TabIndex = 2;
            // 
            // B1
            // 
            B1.Anchor = AnchorStyles.None;
            B1.Location = new Point(15, 19);
            B1.Name = "B1";
            B1.Size = new Size(75, 23);
            B1.TabIndex = 0;
            B1.Text = "OK";
            B1.UseVisualStyleBackColor = true;
            B1.Click += B1_Click;
            // 
            // B2
            // 
            B2.Anchor = AnchorStyles.None;
            B2.Location = new Point(120, 19);
            B2.Name = "B2";
            B2.Size = new Size(75, 23);
            B2.TabIndex = 1;
            B2.Text = "Cancel";
            B2.UseVisualStyleBackColor = true;
            B2.Click += B2_Click;
            // 
            // ConstForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(234, 211);
            Controls.Add(TLP2);
            Controls.Add(TLP1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ConstForm";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "ConstForm";
            TLP1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)NUP1).EndInit();
            TLP2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel TLP1;
        private TableLayoutPanel TLP2;
        internal NumericUpDown NUP1;
        internal Button B1;
        internal Button B2;
    }
}