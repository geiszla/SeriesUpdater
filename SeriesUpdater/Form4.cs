using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SeriesUpdater
{
    public partial class Form4 : Form
    {
        string email;
        string password;

        public Form4()
        {
            InitializeComponent();
        }

        #region Events
        private void loginButton_Click(object sender, EventArgs e)
        {
            email = emailTextBox.Text;
            password = passwordTextBox.Text;
            login();
        }
        #endregion

        #region WebBrowser Functions
        void login()
        {
            loginBrowser.Navigate("https://secure.imdb.com/oauth/login");
            loginBrowser.DocumentCompleted += loginBrowser_ImdbCompleted;
        }

        void loginBrowser_ImdbCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            loginBrowser.DocumentCompleted -= loginBrowser_ImdbCompleted;

            string content = loginBrowser.DocumentText;

            string pattern = "onclick=\"window.open\\('(.*)',";
            List<string> links = new List<string>();
            foreach (Match match in Regex.Matches(content, pattern))
            {
                links.Add(match.Groups[1].Value);
            }

            loginBrowser.Navigate(links[2]);
            loginBrowser.DocumentCompleted += loginBrowser_GoogleCompleted;
        }

        private void loginBrowser_GoogleCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            loginBrowser.DocumentCompleted -= loginBrowser_GoogleCompleted;

            loginBrowser.Document.GetElementById("Email").SetAttribute("value", email);
            loginBrowser.Document.GetElementById("Passwd").SetAttribute("value", password);
            loginBrowser.Document.GetElementById("signIn").InvokeMember("click");

            loginBrowser.DocumentCompleted += loginBrowser_LoginCompleted;
        }

        void loginBrowser_LoginCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            loginBrowser.DocumentCompleted -= loginBrowser_LoginCompleted;

            string cookies = loginBrowser.Document.Cookie;
            MessageBox.Show(cookies);
        }
        #endregion
    }
}
