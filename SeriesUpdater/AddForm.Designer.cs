﻿namespace SeriesUpdater
{
    partial class AddForm
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
            this.addButton = new System.Windows.Forms.Button();
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.ImdbIdLabel = new System.Windows.Forms.Label();
            this.imdbIdTextBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.lastViewedEpisodeLabel = new System.Windows.Forms.Label();
            this.lastViewedEpisodeTextBox = new System.Windows.Forms.TextBox();
            this.fillButton = new System.Windows.Forms.Button();
            this.searchButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // addButton
            // 
            this.addButton.Enabled = false;
            this.addButton.Location = new System.Drawing.Point(160, 105);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 7;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(13, 15);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(38, 13);
            this.nameLabel.TabIndex = 1;
            this.nameLabel.Text = "Name:";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(57, 12);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(182, 20);
            this.nameTextBox.TabIndex = 1;
            this.nameTextBox.TextChanged += new System.EventHandler(this.nameTextBox_TextChanged);
            this.nameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nameTextBox_KeyDown);
            // 
            // ImdbIdLabel
            // 
            this.ImdbIdLabel.AutoSize = true;
            this.ImdbIdLabel.Location = new System.Drawing.Point(13, 45);
            this.ImdbIdLabel.Name = "ImdbIdLabel";
            this.ImdbIdLabel.Size = new System.Drawing.Size(51, 13);
            this.ImdbIdLabel.TabIndex = 3;
            this.ImdbIdLabel.Text = "IMDB ID:";
            // 
            // imdbIdTextBox
            // 
            this.imdbIdTextBox.Location = new System.Drawing.Point(70, 42);
            this.imdbIdTextBox.Name = "imdbIdTextBox";
            this.imdbIdTextBox.Size = new System.Drawing.Size(169, 20);
            this.imdbIdTextBox.TabIndex = 3;
            this.imdbIdTextBox.TextChanged += new System.EventHandler(this.imdbIdTextBox_TextChanged);
            this.imdbIdTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.imdbIdTextBox_KeyDown);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(46, 105);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // lastViewedEpisodeLabel
            // 
            this.lastViewedEpisodeLabel.AutoSize = true;
            this.lastViewedEpisodeLabel.Location = new System.Drawing.Point(13, 75);
            this.lastViewedEpisodeLabel.Name = "lastViewedEpisodeLabel";
            this.lastViewedEpisodeLabel.Size = new System.Drawing.Size(172, 13);
            this.lastViewedEpisodeLabel.TabIndex = 6;
            this.lastViewedEpisodeLabel.Text = "Last viewed episode (eg. S04E02):";
            // 
            // lastViewedEpisodeTextBox
            // 
            this.lastViewedEpisodeTextBox.Location = new System.Drawing.Point(191, 72);
            this.lastViewedEpisodeTextBox.Name = "lastViewedEpisodeTextBox";
            this.lastViewedEpisodeTextBox.Size = new System.Drawing.Size(81, 20);
            this.lastViewedEpisodeTextBox.TabIndex = 5;
            this.lastViewedEpisodeTextBox.TextChanged += new System.EventHandler(this.lastViewedEpisodeTextBox_TextChanged);
            // 
            // fillButton
            // 
            this.fillButton.Enabled = false;
            this.fillButton.Image = global::SeriesUpdater.Properties.Resources.check;
            this.fillButton.Location = new System.Drawing.Point(245, 40);
            this.fillButton.Name = "fillButton";
            this.fillButton.Size = new System.Drawing.Size(27, 23);
            this.fillButton.TabIndex = 4;
            this.fillButton.TabStop = false;
            this.fillButton.UseVisualStyleBackColor = true;
            this.fillButton.Click += new System.EventHandler(this.fillButton_Click);
            // 
            // searchButton
            // 
            this.searchButton.Image = global::SeriesUpdater.Properties.Resources.Search1;
            this.searchButton.Location = new System.Drawing.Point(245, 10);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(27, 23);
            this.searchButton.TabIndex = 2;
            this.searchButton.TabStop = false;
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 141);
            this.ControlBox = false;
            this.Controls.Add(this.fillButton);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.lastViewedEpisodeTextBox);
            this.Controls.Add(this.lastViewedEpisodeLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.imdbIdTextBox);
            this.Controls.Add(this.ImdbIdLabel);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.addButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Form2";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label ImdbIdLabel;
        private System.Windows.Forms.TextBox imdbIdTextBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label lastViewedEpisodeLabel;
        private System.Windows.Forms.TextBox lastViewedEpisodeTextBox;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Button fillButton;
    }
}