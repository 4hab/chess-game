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
            this.SetBounds(this.Bounds.X, this.Bounds.Y, 740, 740);
            generateBoard();

        }

        private void generateBoard()
        {
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

    }
}
