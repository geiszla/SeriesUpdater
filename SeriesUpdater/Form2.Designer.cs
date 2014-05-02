namespace SeriesUpdater
{
    partial class Form2
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
            this.acceptAddButton = new System.Windows.Forms.Button();
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.englishNameLabel = new System.Windows.Forms.Label();
            this.imdbIdTextBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.lastViewedEpisodeLabel = new System.Windows.Forms.Label();
            this.lastViewedEpisodeTextBox = new System.Windows.Forms.TextBox();
            this.fillButton = new System.Windows.Forms.Button();
            this.searchButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // acceptAddButton
            // 
            this.acceptAddButton.Location = new System.Drawing.Point(160, 105);
            this.acceptAddButton.Name = "acceptAddButton";
            this.acceptAddButton.Size = new System.Drawing.Size(75, 23);
            this.acceptAddButton.TabIndex = 7;
            this.acceptAddButton.Text = "Hozzáadás";
            this.acceptAddButton.UseVisualStyleBackColor = true;
            this.acceptAddButton.Click += new System.EventHandler(this.acceptAddButton_Click);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(13, 15);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(30, 13);
            this.nameLabel.TabIndex = 1;
            this.nameLabel.Text = "Név:";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(49, 12);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(190, 20);
            this.nameTextBox.TabIndex = 1;
            this.nameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nameTextBox_KeyDown);
            // 
            // englishNameLabel
            // 
            this.englishNameLabel.AutoSize = true;
            this.englishNameLabel.Location = new System.Drawing.Point(13, 45);
            this.englishNameLabel.Name = "englishNameLabel";
            this.englishNameLabel.Size = new System.Drawing.Size(51, 13);
            this.englishNameLabel.TabIndex = 3;
            this.englishNameLabel.Text = "IMDB ID:";
            // 
            // imdbIdTextBox
            // 
            this.imdbIdTextBox.Location = new System.Drawing.Point(70, 42);
            this.imdbIdTextBox.Name = "imdbIdTextBox";
            this.imdbIdTextBox.Size = new System.Drawing.Size(169, 20);
            this.imdbIdTextBox.TabIndex = 3;
            this.imdbIdTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.imdbIdTextBox_KeyDown);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(46, 105);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Mégse";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // lastViewedEpisodeLabel
            // 
            this.lastViewedEpisodeLabel.AutoSize = true;
            this.lastViewedEpisodeLabel.Location = new System.Drawing.Point(13, 75);
            this.lastViewedEpisodeLabel.Name = "lastViewedEpisodeLabel";
            this.lastViewedEpisodeLabel.Size = new System.Drawing.Size(191, 13);
            this.lastViewedEpisodeLabel.TabIndex = 6;
            this.lastViewedEpisodeLabel.Text = "Legutolsó megnézett rész (pl. S04E02):";
            // 
            // lastViewedEpisodeTextBox
            // 
            this.lastViewedEpisodeTextBox.Location = new System.Drawing.Point(210, 72);
            this.lastViewedEpisodeTextBox.Name = "lastViewedEpisodeTextBox";
            this.lastViewedEpisodeTextBox.Size = new System.Drawing.Size(62, 20);
            this.lastViewedEpisodeTextBox.TabIndex = 5;
            // 
            // fillButton
            // 
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
            this.Controls.Add(this.englishNameLabel);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.acceptAddButton);
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

        private System.Windows.Forms.Button acceptAddButton;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label englishNameLabel;
        private System.Windows.Forms.TextBox imdbIdTextBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label lastViewedEpisodeLabel;
        private System.Windows.Forms.TextBox lastViewedEpisodeTextBox;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Button fillButton;
    }
}