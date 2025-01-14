﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace chess
{
    class BoardTile : PictureBox
    {
        private bool _isAvailable = false;
        private bool _danger = false;
        private Piece _piece;
        private Coordinates _coordinates;

        public bool isEmpty() => piece == null;
        public PieceColor color => (coordinates.x + coordinates.y) % 2 == 0 ? PieceColor.white : PieceColor.black;
        public Coordinates coordinates => _coordinates;
        public Piece piece => _piece;
        public bool isAvailable => _isAvailable;
        public bool danger => _danger;
        public BoardTile(Coordinates coordinates, Piece piece = null)
        {
            _coordinates = coordinates;
            BackColor = color == PieceColor.white ? Color.White : Color.DarkGray;
            Click += onClick;
            _isAvailable = false;
            setPiece(piece);
        }

        public void markDanger(bool val)
        {
            _danger = val;
            if (val == true)
            {
                //other player's king is attacked
                if (piece != null && piece is King && GameObserver.instance.otherPlayer.isMyPiece(piece))
                {
                    GameObserver.instance.otherPlayer.inDanger(true);
                    if (!GameObserver.instance.currentPlayer.virtualAttack)
                        BackColor = Color.Red;
                }
            }
        }
        private void onClick(object sender, EventArgs eventArgs)
        {
            Board board = Board.instance;
            if (board.selectedTile != null)
            {
                BoardTile tileA = board.selectedTile;
                BoardTile tileB = sender as BoardTile;
                if (tileA.coordinates == tileB.coordinates)
                {
                    //disselect
                    board.select(null);
                    //unmark available cells
                    tileA.piece.unmarkAvailableCells();
                }
                else if (tileB.isAvailable)
                {
                    //move 
                    board.move(tileA.coordinates, tileB.coordinates);
                }
                else
                {
                    return;
                }
            }
            else
            {
                BoardTile tile = sender as BoardTile;
                if (!tile.isEmpty() && GameObserver.instance.currentPlayer.isMyPiece(tile.piece))
                {
                    board.select(tile);
                    tile.piece.markAvailableCells();
                }
                else
                    return;
            }
        }
        public void switchMark(bool countOnly)
        {
            if (countOnly)
                return;
            _isAvailable = !_isAvailable;
            if (isAvailable) 
            {
                BackColor = color == PieceColor.white ? Color.LimeGreen : Color.Green;
            }
            else
            {
                BackColor = color == PieceColor.white ? Color.White : Color.DarkGray;
            }
        }
        public void setPiece(Piece piece, bool virt = false)
        {
            _piece = piece;
            if (!virt)
                Image = piece?.image;
        }
    }
}
