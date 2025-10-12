namespace WinFormsApp1
{
    partial class Form1
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
            B3 = new Button();
            NUD1 = new NumericUpDown();
            B2 = new Button();
            B1 = new Button();
            TLP1 = new TableLayoutPanel();
            B7 = new Button();
            B5 = new Button();
            B6 = new Button();
            B4 = new Button();
            NUD2 = new NumericUpDown();
            TLP2 = new TableLayoutPanel();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)NUD1).BeginInit();
            TLP1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)NUD2).BeginInit();
            TLP2.SuspendLayout();
            SuspendLayout();
            // 
            // B3
            // 
            B3.Anchor = AnchorStyles.None;
            B3.Location = new Point(34, 144);
            B3.Name = "B3";
            B3.Size = new Size(75, 23);
            B3.TabIndex = 2;
            B3.Text = "Semicircle";
            B3.UseVisualStyleBackColor = true;
            B3.Click += B3_Click;
            // 
            // NUD1
            // 
            NUD1.Anchor = AnchorStyles.None;
            NUD1.Location = new Point(83, 132);
            NUD1.Name = "NUD1";
            NUD1.Size = new Size(120, 23);
            NUD1.TabIndex = 1;
            NUD1.ValueChanged += NUD1_ValueChanged;
            // 
            // B2
            // 
            B2.Anchor = AnchorStyles.None;
            B2.Location = new Point(34, 40);
            B2.Name = "B2";
            B2.Size = new Size(75, 23);
            B2.TabIndex = 1;
            B2.Text = "Normal";
            B2.UseVisualStyleBackColor = true;
            B2.Click += B2_Click;
            // 
            // B1
            // 
            B1.Anchor = AnchorStyles.None;
            B1.Location = new Point(34, 561);
            B1.Name = "B1";
            B1.Size = new Size(75, 23);
            B1.TabIndex = 1;
            B1.Text = "Add Vertex";
            B1.UseVisualStyleBackColor = true;
            B1.Click += B1_Click;
            // 
            // TLP1
            // 
            TLP1.ColumnCount = 2;
            TLP1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TLP1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TLP1.Controls.Add(B7, 1, 1);
            TLP1.Controls.Add(B5, 1, 4);
            TLP1.Controls.Add(B6, 1, 0);
            TLP1.Controls.Add(B4, 1, 3);
            TLP1.Controls.Add(B1, 0, 5);
            TLP1.Controls.Add(B2, 0, 0);
            TLP1.Controls.Add(NUD2, 1, 2);
            TLP1.Controls.Add(B3, 0, 1);
            TLP1.Location = new Point(1286, 223);
            TLP1.Name = "TLP1";
            TLP1.RowCount = 6;
            TLP1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6666679F));
            TLP1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6666679F));
            TLP1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6666679F));
            TLP1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6666679F));
            TLP1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6666679F));
            TLP1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6666679F));
            TLP1.Size = new Size(286, 626);
            TLP1.TabIndex = 3;
            // 
            // B7
            // 
            B7.Anchor = AnchorStyles.None;
            B7.Location = new Point(177, 144);
            B7.Name = "B7";
            B7.Size = new Size(75, 23);
            B7.TabIndex = 6;
            B7.Text = "Const";
            B7.UseVisualStyleBackColor = true;
            B7.Click += B7_Click;
            // 
            // B5
            // 
            B5.Anchor = AnchorStyles.None;
            B5.Location = new Point(177, 456);
            B5.Name = "B5";
            B5.Size = new Size(75, 23);
            B5.TabIndex = 5;
            B5.Text = "Lock45";
            B5.UseVisualStyleBackColor = true;
            B5.Click += B5_Click;
            // 
            // B6
            // 
            B6.Anchor = AnchorStyles.None;
            B6.Location = new Point(177, 40);
            B6.Name = "B6";
            B6.Size = new Size(75, 23);
            B6.TabIndex = 5;
            B6.Text = "Normal";
            B6.UseVisualStyleBackColor = true;
            B6.Click += B6_Click;
            // 
            // B4
            // 
            B4.Anchor = AnchorStyles.None;
            B4.Location = new Point(177, 352);
            B4.Name = "B4";
            B4.Size = new Size(75, 23);
            B4.TabIndex = 5;
            B4.Text = "Vertical";
            B4.UseVisualStyleBackColor = true;
            B4.Click += B4_Click;
            // 
            // NUD2
            // 
            NUD2.Anchor = AnchorStyles.None;
            NUD2.Location = new Point(154, 248);
            NUD2.Name = "NUD2";
            NUD2.Size = new Size(120, 23);
            NUD2.TabIndex = 5;
            // 
            // TLP2
            // 
            TLP2.ColumnCount = 1;
            TLP2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TLP2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            TLP2.Controls.Add(label1, 0, 0);
            TLP2.Controls.Add(NUD1, 0, 1);
            TLP2.Location = new Point(1286, 12);
            TLP2.Name = "TLP2";
            TLP2.RowCount = 2;
            TLP2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            TLP2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            TLP2.Size = new Size(286, 192);
            TLP2.TabIndex = 4;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.Font = new Font("Segoe UI", 20F);
            label1.Location = new Point(69, 16);
            label1.Name = "label1";
            label1.Size = new Size(147, 63);
            label1.TabIndex = 5;
            label1.Text = "Edges";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1584, 861);
            Controls.Add(TLP2);
            Controls.Add(TLP1);
            MaximumSize = new Size(1600, 900);
            MinimumSize = new Size(1600, 900);
            Name = "Form1";
            Text = "Projekt 1 - Łukasz Przybylski";
            ((System.ComponentModel.ISupportInitialize)NUD1).EndInit();
            TLP1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)NUD2).EndInit();
            TLP2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        internal NumericUpDown NUD1;
        internal Button B1;
        private Button B3;
        private Button B2;
        private TableLayoutPanel TLP1;
        private TableLayoutPanel TLP2;
        private Label label1;
        private Button B4;
        private Button B5;
        internal NumericUpDown NUD2;
        private Button B7;
        private Button B6;
    }
}
