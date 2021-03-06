﻿namespace SeriesUpdater
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.seriesTitleLabel = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.displayLanguageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultLanguageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customLanguageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendNotificationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RunOnStartupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addButton = new System.Windows.Forms.Button();
            this.notifyIconContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // seriesTitleLabel
            // 
            this.seriesTitleLabel.AutoSize = true;
            this.seriesTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.seriesTitleLabel.Location = new System.Drawing.Point(12, 9);
            this.seriesTitleLabel.Name = "seriesTitleLabel";
            this.seriesTitleLabel.Size = new System.Drawing.Size(56, 18);
            this.seriesTitleLabel.TabIndex = 0;
            this.seriesTitleLabel.Text = "Series";
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipText = "Update is in progress. Please wait for the process to finish.";
            this.notifyIcon.BalloonTipTitle = "Updating Series...";
            this.notifyIcon.ContextMenuStrip = this.notifyIconContextMenu;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Series Updater";
            this.notifyIcon.Visible = true;
            // 
            // notifyIconContextMenu
            // 
            this.notifyIconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayLanguageToolStripMenuItem,
            this.sendNotificationsToolStripMenuItem,
            this.RunOnStartupToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.notifyIconContextMenu.Name = "notifyIconControlManagementMenu";
            this.notifyIconContextMenu.Size = new System.Drawing.Size(172, 98);
            this.notifyIconContextMenu.MouseUp += new System.Windows.Forms.MouseEventHandler(this.notifyIconControlManagementMenu_MouseUp);
            // 
            // displayLanguageToolStripMenuItem
            // 
            this.displayLanguageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defaultLanguageToolStripMenuItem,
            this.customLanguageToolStripMenuItem});
            this.displayLanguageToolStripMenuItem.Enabled = false;
            this.displayLanguageToolStripMenuItem.Name = "displayLanguageToolStripMenuItem";
            this.displayLanguageToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.displayLanguageToolStripMenuItem.Text = "Display Language";
            // 
            // defaultLanguageToolStripMenuItem
            // 
            this.defaultLanguageToolStripMenuItem.Checked = true;
            this.defaultLanguageToolStripMenuItem.CheckOnClick = true;
            this.defaultLanguageToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.defaultLanguageToolStripMenuItem.Name = "defaultLanguageToolStripMenuItem";
            this.defaultLanguageToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.defaultLanguageToolStripMenuItem.Text = "Default";
            // 
            // customLanguageToolStripMenuItem
            // 
            this.customLanguageToolStripMenuItem.Name = "customLanguageToolStripMenuItem";
            this.customLanguageToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.customLanguageToolStripMenuItem.Text = "Custom";
            this.customLanguageToolStripMenuItem.Click += new System.EventHandler(this.customLanguageToolStripMenuItem_Click);
            // 
            // sendNotificationsToolStripMenuItem
            // 
            this.sendNotificationsToolStripMenuItem.Checked = true;
            this.sendNotificationsToolStripMenuItem.CheckOnClick = true;
            this.sendNotificationsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sendNotificationsToolStripMenuItem.Name = "sendNotificationsToolStripMenuItem";
            this.sendNotificationsToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.sendNotificationsToolStripMenuItem.Text = "Send Notifications";
            this.sendNotificationsToolStripMenuItem.Click += new System.EventHandler(this.notificationContextMenuItem_Click);
            // 
            // RunOnStartupToolStripMenuItem
            // 
            this.RunOnStartupToolStripMenuItem.Name = "RunOnStartupToolStripMenuItem";
            this.RunOnStartupToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.RunOnStartupToolStripMenuItem.Text = "Run On Startup";
            this.RunOnStartupToolStripMenuItem.Click += new System.EventHandler(this.autorunStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(168, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(74, 8);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(23, 23);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "+";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(120, 40);
            this.ControlBox = false;
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.seriesTitleLabel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Location = new System.Drawing.Point(15, 47);
            this.Name = "MainForm";
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
        private System.Windows.Forms.ToolStripMenuItem displayLanguageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defaultLanguageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customLanguageToolStripMenuItem;
    }
}

