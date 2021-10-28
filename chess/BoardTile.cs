using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace chess
{
    class BoardTile : PictureBox
    {
        public static BoardTile of(Coordinates coordinates)
        {
            return GameObserver.board[coordinates.x, coordinates.y];
        }
        public bool isEmpty() => piece == null;

        private bool isMarked;
        private void onClick(object sender, EventArgs eventArgs)
        {
            BoardTile destinationTile = sender as BoardTile;
            Piece piece = destinationTile.piece;
            BoardTile sourceTile = GameObserver.selectedTile;
            //no piece here
            if (piece == null)
            {
                //move to empty cell
                if (sourceTile != null && destinationTile.isMarked)
                {
                    GameObserver.performMove(sourceTile, destinationTile);
                }
                return;
            }
            if (GameObserver.currentPlayerColor != this.piece.color && sourceTile == null) 
                return;
            if (sourceTile != null) 
            {
                //unmark
                if(destinationTile.coordinates == sourceTile.coordinates)
                {
                    destinationTile.switchMark();
                    piece.unmarkAvailableCells();
                    GameObserver.selectedTile = null;
                }
                //move to occupied cell => dead piece
                else if (destinationTile.isMarked)
                {
                    GameObserver.performMove(sourceTile, destinationTile);
                }
            }
            //mark
            else if (piece != null)
            {
                this.switchMark();
                piece.markAvailableCells();
                GameObserver.selectedTile = destinationTile;
            }
        }
        public void switchMark()
        {
            if (!isMarked)
            {
                this.BackColor = this.BackColor == Color.White ? Color.LimeGreen : Color.Green;
                isMarked = true;
            }
            else if (isMarked)
            {
                this.BackColor = this.BackColor == Color.LimeGreen ? Color.White : Color.Brown;
                isMarked = false;
            }
        }

        private Piece _piece;
        private Coordinates _coordinates;
        public Colors color => (coordinates.x + coordinates.y) % 2 == 0 ? Colors.white: Colors.black;
        public Coordinates coordinates => _coordinates;
        public Piece piece => _piece;

        public void setPiece(Piece piece)
        {
            _piece = piece;
            this.Image = piece?.image;
        }
        public BoardTile(Coordinates coordinates, Piece piece = null)
        {
            this._coordinates = coordinates;
            this.BackColor = color==Colors.white ? Color.White : Color.Brown;
            this._piece = piece;
            this.Click += onClick;
            this.isMarked = false;
        }
    }
}
