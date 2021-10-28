using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace chess
{
    class BoardTile : PictureBox
    {
        public bool isEmpty() => piece == null;

        private bool isMarked;
        private void onClick(object sender, EventArgs eventArgs)
        {
            BoardTile tile = sender as BoardTile;
            Piece piece = tile.piece;
            BoardTile selectedTile = GameObserver.selectedTile;
            //no piece here
            if (piece == null)
            {
                if (selectedTile != null && tile.isMarked)
                {
                    selectedTile.switchMark();
                    selectedTile.piece.unmarkAvailableCells();
                    selectedTile.piece.moveTo(tile.coordinates);
                    tile.setPiece(selectedTile.piece);
                    selectedTile.setPiece(null);
                    GameObserver.selectedTile = null;
                    GameObserver.switchPlayer();
                }
                return;
            }
            if (GameObserver.currentPlayerColor != this.piece.color && selectedTile == null) 
                return;
            //unmark or do nothing
            if (selectedTile != null) 
            {
                //unmark
                if(tile.coordinates == selectedTile.coordinates)
                {
                    tile.switchMark();
                    piece.unmarkAvailableCells();
                    GameObserver.selectedTile = null;
                }
                //move to
                else if (tile.isMarked)
                {
                    selectedTile.switchMark();
                    selectedTile.piece.unmarkAvailableCells();
                    selectedTile.piece.moveTo(tile.coordinates);
                    tile.setPiece(selectedTile.piece);
                    selectedTile.setPiece(null);
                    GameObserver.selectedTile = null;
                    GameObserver.switchPlayer();
                }
            }
            //mark
            else if (piece != null)
            {
                this.switchMark();
                piece.markAvailableCells();
                GameObserver.selectedTile = tile;
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
        public Colors color => (coordinates.x() + coordinates.y()) % 2 == 0 ? Colors.white: Colors.black;
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
