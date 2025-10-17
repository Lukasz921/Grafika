namespace Proj1
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
            MS = new MenuStrip();
            helpToolStripMenuItem = new ToolStripMenuItem();
            MS.SuspendLayout();
            SuspendLayout();
            // 
            // MS
            // 
            MS.ImageScalingSize = new Size(20, 20);
            MS.Items.AddRange(new ToolStripItem[] { helpToolStripMenuItem });
            MS.Location = new Point(0, 0);
            MS.Name = "MS";
            MS.Size = new Size(800, 28);
            MS.TabIndex = 0;
            MS.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(55, 24);
            helpToolStripMenuItem.Text = "Help";
            helpToolStripMenuItem.Click += helpToolStripMenuItem_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 453);
            Controls.Add(MS);
            MainMenuStrip = MS;
            MinimumSize = new Size(800, 500);
            Name = "MainForm";
            Text = "Projekt 1";
            MS.ResumeLayout(false);
            MS.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip MS;
        private ToolStripMenuItem helpToolStripMenuItem;
    }
}
