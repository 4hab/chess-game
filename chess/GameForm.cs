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
        private Board board;
        private GameObserver observer;
        private void gameOver()
        {
            DialogResult result = MessageBox.Show(GameObserver.instance.otherPlayer.name + " Won the match\nPlay again?", "Game Over!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            stayOrLeave(result);
        }
        private void stayOrLeave(DialogResult result)
        {
            if (result == DialogResult.Yes)
            {
                GameObserver.instance.reset();
                this.Close();
            }
            else
            {
                Environment.Exit(0);
            }
        }
        private void gameOverDraw()
        {
            DialogResult result = MessageBox.Show("Draw No one won\nPlay again?", "Game Over!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            stayOrLeave(result);
        }
        public GameForm(string firstPlayerName, string secondPlayerName)
        {
            board = Board.instance;
            observer = GameObserver.instance;
            GameObserver.instance.setPlayersNames(firstPlayerName, secondPlayerName);
            observer.scoreUpdated += updateScore;
            observer.gameOverDraw += gameOverDraw;
            observer.gameOver += gameOver;
            InitializeComponent();
            SetBounds(Bounds.X, Bounds.Y, 740, 740);
            drawBoard();
        }
        private void updateScore()
        {
            int score = observer.score;
            scoreLabel1.Text = score > 0 ? "+"+score.ToString() : "";
            scoreLabel2.Text = score < 0 ? "+"+(score * -1).ToString() : "";
        }
        private void drawBoard()
        {
            blackPlayerNameLabel.Text = GameObserver.instance.blackPlayer.name;
            whitePlayerNameLabel.Text = GameObserver.instance.whitePlayer.name;
            blackPlayerNameLabel.SetBounds(120, 60, blackPlayerNameLabel.Size.Width, blackPlayerNameLabel.Size.Height);
            whitePlayerNameLabel.SetBounds(120, 600, whitePlayerNameLabel.Size.Width, whitePlayerNameLabel.Size.Height);
            scoreLabel2.SetBounds(300, 60, scoreLabel2.Size.Width, scoreLabel2.Size.Height);
            scoreLabel1.SetBounds(300, 600, scoreLabel1.Size.Width, scoreLabel1.Size.Height);

            boardPanel.SetBounds(120, 100, 80 * 8, 80 * 8);
            int x, y=0;
            int width=60, height=60;
            for(int row = 0; row < 8; row++)
            {
                x = 0;
                for(int column = 0; column < 8; column++)
                {
                    BoardTile tile = board.of(row,column);
                    tile.SetBounds(x, y, width, height);
                    boardPanel.Controls.Add(tile);
                    x += width;
                }
                y += height;
            }
        }

        private void unDoButton_Click(object sender, EventArgs e)
        {
            board.unDoMove();
        }

        private void reDoButton_Click(object sender, EventArgs e)
        {
            board.reDoMove();
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)
            {
                board.unDoMove();
            } 
            else if(e.Control&&e.KeyCode==Keys.Y)
            {
                board.reDoMove();
            }
            //commnt
        }
    }
}
