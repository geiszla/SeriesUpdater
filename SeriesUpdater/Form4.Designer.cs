namespace SeriesUpdater
{
    partial class Form4
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
            this.titleLabel1 = new System.Windows.Forms.Label();
            this.titleLabel2 = new System.Windows.Forms.Label();
            this.emailTitleLabel = new System.Windows.Forms.Label();
            this.passwordTitleLabel = new System.Windows.Forms.Label();
            this.rememberPasswordCheckBox = new System.Windows.Forms.CheckBox();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.loginBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // titleLabel1
            // 
            this.titleLabel1.AutoSize = true;
            this.titleLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.titleLabel1.Location = new System.Drawing.Point(13, 9);
            this.titleLabel1.Name = "titleLabel1";
            this.titleLabel1.Size = new System.Drawing.Size(282, 13);
            this.titleLabel1.TabIndex = 0;
            this.titleLabel1.Text = "A bejelentkezéshez adja meg a Google fiókjához";
            // 
            // titleLabel2
            // 
            this.titleLabel2.AutoSize = true;
            this.titleLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.titleLabel2.Location = new System.Drawing.Point(10, 24);
            this.titleLabel2.Name = "titleLabel2";
            this.titleLabel2.Size = new System.Drawing.Size(194, 13);
            this.titleLabel2.TabIndex = 1;
            this.titleLabel2.Text = " tartozó email-címét és jelszavát!";
            // 
            // emailTitleLabel
            // 
            this.emailTitleLabel.AutoSize = true;
            this.emailTitleLabel.Location = new System.Drawing.Point(12, 54);
            this.emailTitleLabel.Name = "emailTitleLabel";
            this.emailTitleLabel.Size = new System.Drawing.Size(59, 13);
            this.emailTitleLabel.TabIndex = 2;
            this.emailTitleLabel.Text = "E-mail cím:";
            // 
            // passwordTitleLabel
            // 
            this.passwordTitleLabel.AutoSize = true;
            this.passwordTitleLabel.Location = new System.Drawing.Point(13, 80);
            this.passwordTitleLabel.Name = "passwordTitleLabel";
            this.passwordTitleLabel.Size = new System.Drawing.Size(39, 13);
            this.passwordTitleLabel.TabIndex = 3;
            this.passwordTitleLabel.Text = "Jelszó:";
            // 
            // rememberPasswordCheckBox
            // 
            this.rememberPasswordCheckBox.AutoSize = true;
            this.rememberPasswordCheckBox.Location = new System.Drawing.Point(16, 103);
            this.rememberPasswordCheckBox.Name = "rememberPasswordCheckBox";
            this.rememberPasswordCheckBox.Size = new System.Drawing.Size(119, 17);
            this.rememberPasswordCheckBox.TabIndex = 4;
            this.rememberPasswordCheckBox.Text = "Jelszó megjegyzése";
            this.rememberPasswordCheckBox.UseVisualStyleBackColor = true;
            // 
            // emailTextBox
            // 
            this.emailTextBox.Location = new System.Drawing.Point(79, 51);
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(212, 20);
            this.emailTextBox.TabIndex = 5;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(79, 77);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(212, 20);
            this.passwordTextBox.TabIndex = 6;
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(91, 126);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(127, 29);
            this.loginButton.TabIndex = 7;
            this.loginButton.Text = "Bejelentkezés";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // loginBrowser
            // 
            this.loginBrowser.Location = new System.Drawing.Point(32, -14);
            this.loginBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.loginBrowser.Name = "loginBrowser";
            this.loginBrowser.Size = new System.Drawing.Size(20, 20);
            this.loginBrowser.TabIndex = 8;
            this.loginBrowser.Visible = false;
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 171);
            this.Controls.Add(this.loginBrowser);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.emailTextBox);
            this.Controls.Add(this.rememberPasswordCheckBox);
            this.Controls.Add(this.passwordTitleLabel);
            this.Controls.Add(this.emailTitleLabel);
            this.Controls.Add(this.titleLabel2);
            this.Controls.Add(this.titleLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form4";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bejelentkezés";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel1;
        private System.Windows.Forms.Label titleLabel2;
        private System.Windows.Forms.Label emailTitleLabel;
        private System.Windows.Forms.Label passwordTitleLabel;
        private System.Windows.Forms.CheckBox rememberPasswordCheckBox;
        private System.Windows.Forms.TextBox emailTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.WebBrowser loginBrowser;
    }
}