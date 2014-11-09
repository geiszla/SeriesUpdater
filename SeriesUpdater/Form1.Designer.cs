namespace SeriesUpdater
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.seriesTitleLabel = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sendNotificationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RunOnStartupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addButton = new System.Windows.Forms.Button();
            this.imdbLanguageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultLanguageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customLanguageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIconContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // seriesTitleLabel
            // 
            this.seriesTitleLabel.AutoSize = true;
            this.seriesTitleLabel.Font = new System.Drawing.Font("Kozuka Gothic Pro H", 11F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.seriesTitleLabel.Location = new System.Drawing.Point(12, 9);
            this.seriesTitleLabel.Name = "seriesTitleLabel";
            this.seriesTitleLabel.Size = new System.Drawing.Size(80, 18);
            this.seriesTitleLabel.TabIndex = 0;
            this.seriesTitleLabel.Text = "Sorozatok";
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipText = "Frissítés folyamatban. A frissítés alatt nem tudod megnyitni a programot.";
            this.notifyIcon.BalloonTipTitle = "Frissítés";
            this.notifyIcon.ContextMenuStrip = this.notifyIconContextMenu;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Sorozat figyelő";
            this.notifyIcon.Visible = true;
            // 
            // notifyIconContextMenu
            // 
            this.notifyIconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.imdbLanguageToolStripMenuItem,
            this.sendNotificationsToolStripMenuItem,
            this.RunOnStartupToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.notifyIconContextMenu.Name = "notifyIconControlManagementMenu";
            this.notifyIconContextMenu.Size = new System.Drawing.Size(222, 98);
            this.notifyIconContextMenu.MouseUp += new System.Windows.Forms.MouseEventHandler(this.notifyIconControlManagementMenu_MouseUp);
            // 
            // sendNotificationsToolStripMenuItem
            // 
            this.sendNotificationsToolStripMenuItem.Checked = true;
            this.sendNotificationsToolStripMenuItem.CheckOnClick = true;
            this.sendNotificationsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sendNotificationsToolStripMenuItem.Name = "sendNotificationsToolStripMenuItem";
            this.sendNotificationsToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.sendNotificationsToolStripMenuItem.Text = "Értesítések küldése";
            this.sendNotificationsToolStripMenuItem.Click += new System.EventHandler(this.notificationContextMenuItem_Click);
            // 
            // RunOnStartupToolStripMenuItem
            // 
            this.RunOnStartupToolStripMenuItem.Name = "RunOnStartupToolStripMenuItem";
            this.RunOnStartupToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.RunOnStartupToolStripMenuItem.Text = "Indítás a Windowszal együtt";
            this.RunOnStartupToolStripMenuItem.Click += new System.EventHandler(this.autorunStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(218, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.exitToolStripMenuItem.Text = "Kilépés";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(98, 7);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(23, 23);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "+";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // imdbLanguageToolStripMenuItem
            // 
            this.imdbLanguageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defaultLanguageToolStripMenuItem,
            this.customLanguageToolStripMenuItem});
            this.imdbLanguageToolStripMenuItem.Name = "imdbLanguageToolStripMenuItem";
            this.imdbLanguageToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.imdbLanguageToolStripMenuItem.Text = "IMDB Nyelv";
            // 
            // defaultLanguageToolStripMenuItem
            // 
            this.defaultLanguageToolStripMenuItem.Checked = true;
            this.defaultLanguageToolStripMenuItem.CheckOnClick = true;
            this.defaultLanguageToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.defaultLanguageToolStripMenuItem.Name = "defaultLanguageToolStripMenuItem";
            this.defaultLanguageToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.defaultLanguageToolStripMenuItem.Text = "Alapértelmezett";
            // 
            // customLanguageToolStripMenuItem
            // 
            this.customLanguageToolStripMenuItem.Name = "customLanguageToolStripMenuItem";
            this.customLanguageToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.customLanguageToolStripMenuItem.Text = "Saját";
            this.customLanguageToolStripMenuItem.Click += new System.EventHandler(this.customLanguageToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(138, 40);
            this.ControlBox = false;
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.seriesTitleLabel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Location = new System.Drawing.Point(15, 47);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 5, 5);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.Deactivate += new System.EventHandler(this.Form1_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.notifyIconContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label seriesTitleLabel;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.ContextMenuStrip notifyIconContextMenu;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RunOnStartupToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem sendNotificationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imdbLanguageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defaultLanguageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customLanguageToolStripMenuItem;
    }
}

