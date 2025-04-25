namespace VideoInputViewer
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ComboBox cboCameras;
        private System.Windows.Forms.ComboBox cboResolutions;
        private System.Windows.Forms.Button btnSnapshot;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Timer timerFPS;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuRefresh;
        private System.Windows.Forms.ToolStripMenuItem menuSnapshot;
        private System.Windows.Forms.ToolStripMenuItem menuToggleControls;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.cboCameras = new System.Windows.Forms.ComboBox();
            this.cboResolutions = new System.Windows.Forms.ComboBox();
            this.btnSnapshot = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.timerFPS = new System.Windows.Forms.Timer(this.components);
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSnapshot = new System.Windows.Forms.ToolStripMenuItem();
            this.menuToggleControls = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 76);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(800, 374);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // cboCameras
            // 
            this.cboCameras.Dock = System.Windows.Forms.DockStyle.Top;
            this.cboCameras.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCameras.FormattingEnabled = true;
            this.cboCameras.Location = new System.Drawing.Point(0, 28);
            this.cboCameras.Margin = new System.Windows.Forms.Padding(4);
            this.cboCameras.Name = "cboCameras";
            this.cboCameras.Size = new System.Drawing.Size(800, 24);
            this.cboCameras.TabIndex = 1;
            this.cboCameras.SelectedIndexChanged += new System.EventHandler(this.cboCameras_SelectedIndexChanged);
            // 
            // cboResolutions
            // 
            this.cboResolutions.Dock = System.Windows.Forms.DockStyle.Top;
            this.cboResolutions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboResolutions.FormattingEnabled = true;
            this.cboResolutions.Location = new System.Drawing.Point(0, 52);
            this.cboResolutions.Margin = new System.Windows.Forms.Padding(4);
            this.cboResolutions.Name = "cboResolutions";
            this.cboResolutions.Size = new System.Drawing.Size(800, 24);
            this.cboResolutions.TabIndex = 2;
            this.cboResolutions.SelectedIndexChanged += new System.EventHandler(this.cboResolutions_SelectedIndexChanged);
            // 
            // btnSnapshot
            // 
            this.btnSnapshot.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSnapshot.Location = new System.Drawing.Point(600, 76);
            this.btnSnapshot.Margin = new System.Windows.Forms.Padding(4);
            this.btnSnapshot.Name = "btnSnapshot";
            this.btnSnapshot.Size = new System.Drawing.Size(100, 374);
            this.btnSnapshot.TabIndex = 3;
            this.btnSnapshot.Text = "Snapshot";
            this.btnSnapshot.UseVisualStyleBackColor = true;
            this.btnSnapshot.Click += new System.EventHandler(this.btnSnapshot_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRefresh.Location = new System.Drawing.Point(700, 76);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 374);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // timerFPS
            // 
            this.timerFPS.Interval = 1000;
            this.timerFPS.Tick += new System.EventHandler(this.timerFPS_Tick);
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(800, 28);
            this.menuStrip.TabIndex = 5;
            this.menuStrip.Text = "menuStrip";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuRefresh,
            this.menuSnapshot,
            this.menuToggleControls});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(46, 24);
            this.menuFile.Text = "&File";
            // 
            // menuRefresh
            // 
            this.menuRefresh.Name = "menuRefresh";
            this.menuRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.menuRefresh.Size = new System.Drawing.Size(224, 26);
            this.menuRefresh.Text = "&Refresh";
            this.menuRefresh.Click += new System.EventHandler(this.menuRefresh_Click);
            // 
            // menuSnapshot
            // 
            this.menuSnapshot.Name = "menuSnapshot";
            this.menuSnapshot.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuSnapshot.Size = new System.Drawing.Size(224, 26);
            this.menuSnapshot.Text = "&Snapshot";
            this.menuSnapshot.Click += new System.EventHandler(this.menuSnapshot_Click);
            // 
            // menuToggleControls
            // 
            this.menuToggleControls.Name = "menuToggleControls";
            this.menuToggleControls.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.menuToggleControls.Size = new System.Drawing.Size(224, 26);
            this.menuToggleControls.Text = "&Hide Controls";
            this.menuToggleControls.Click += new System.EventHandler(this.menuToggleControls_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSnapshot);
            this.Controls.Add(this.cboResolutions);
            this.Controls.Add(this.cboCameras);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "Webcam Viewer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}