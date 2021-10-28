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
        public GameForm()
        {
            GameObserver.initBoard();
            board = GameObserver.board;

            InitializeComponent();
            SetBounds(Bounds.X, Bounds.Y, 740, 740);
            drawBoard();

        }

        private void drawBoard()
        {
            blackPlayerNameLabel.Text = GameObserver.blackPlayer.name;
            whitePlayerNameLabel.Text = GameObserver.whitePlayer.name;
            blackPlayerNameLabel.SetBounds(120, 60, blackPlayerNameLabel.Size.Width, blackPlayerNameLabel.Size.Height);
            whitePlayerNameLabel.SetBounds(120, 600, whitePlayerNameLabel.Size.Width, whitePlayerNameLabel.Size.Height);
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
                    if (tile.piece != null) 
                    {
                        tile.Image =tile.piece.image;
                    }
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
