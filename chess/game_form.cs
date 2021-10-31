using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chess
{
    public partial class GameForm : Form
    {
        private BoardTile[,] board;
        private void gameOver()
        {
            MessageBox.Show(Player.otherPlayer.name + " Won the match", "Game Over!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        }
        public GameForm()
        {
            Board.init();
            board = Board.instance;

            /*GameObserver.scoreUpdated+=updateScore;
            GameObserver.check += check;*/
            GameObserver.gameOver += gameOver;
            GameObserver.scoreUpdated += updateScore;
            InitializeComponent();
            SetBounds(Bounds.X, Bounds.Y, 740, 740);
            drawBoard();
        }
        private void updateScore()
        {
            int score = GameObserver.score;
            scoreLabel1.Text = score > 0 ? "+"+score.ToString() : "";
            scoreLabel2.Text = score < 0 ? "+"+(score * -1).ToString() : "";
        }
        private void drawBoard()
        {
            blackPlayerNameLabel.Text = Player.blackPlayer.name;
            whitePlayerNameLabel.Text = Player.whitePlayer.name;
            blackPlayerNameLabel.SetBounds(120, 60, blackPlayerNameLabel.Size.Width, blackPlayerNameLabel.Size.Height);
            whitePlayerNameLabel.SetBounds(120, 600, whitePlayerNameLabel.Size.Width, whitePlayerNameLabel.Size.Height);
            scoreLabel2.SetBounds(200, 60, scoreLabel2.Size.Width, scoreLabel2.Size.Height);
            scoreLabel1.SetBounds(200, 600, scoreLabel1.Size.Width, scoreLabel1.Size.Height);

            boardPanel.SetBounds(120, 100, 80 * 8, 80 * 8);
            int x, y=0;
            int width=60, height=60;
            for(int row = 0; row < 8; row++)
            {
                x = 0;
                for(int column = 0; column < 8; column++)
                {
                    BoardTile tile = board[row, column];
                    tile.SetBounds(x, y, width, height);
                    boardPanel.Controls.Add(tile);
                    x += width;
                }
                y += height;
            }
        }

        private void unDoButton_Click(object sender, EventArgs e)
        {
            GameObserver.unDoMove();
        }

        private void reDoButton_Click(object sender, EventArgs e)
        {
            GameObserver.reDoMove();
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)
            {
                GameObserver.unDoMove();
            } 
            else if(e.Control&&e.KeyCode==Keys.Y)
            {
                GameObserver.reDoMove();
            }
        }
    }
}
