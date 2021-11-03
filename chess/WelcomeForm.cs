using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace chess
{
    public partial class WelcomeForm : Form
    {
        public WelcomeForm()
        {
            InitializeComponent();
        }

        private void newGameButton_Click(object sender, EventArgs e)
        {
            string firstPlayerName = firstPlayerTextField.Text == ""?"Player 1": firstPlayerTextField.Text;
            string secondPlayerName = secondPlayerTextField.Text == "" ? "Player 2" : secondPlayerTextField.Text;
            GameForm gameForm = new GameForm(firstPlayerName,secondPlayerName);
            Hide();
            gameForm.ShowDialog();
            Show();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
