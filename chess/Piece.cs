using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace chess
{
    class Piece
    {
        protected Coordinates _coordinates;
        protected string _type;
        protected int _val;
        protected PieceColor _color;
        protected Image _image;

        public int val => _val;
        public string type => _type;
        public Piece(Coordinates coordinates, PieceColor color)
        {
            _coordinates = coordinates;
            _color = color;
        }
        public PieceColor color => _color;
        public Image image => _image;

        public Coordinates coordinates => _coordinates;
        public virtual int markAvailableCells() 
        {
            return lookForAvailableCells(false);
        }
        public virtual int unmarkAvailableCells()
        {
            return lookForAvailableCells(false);
        }
        protected virtual int lookForAvailableCells(bool countOnly) { return 0; }
        public virtual void moveTo(Coordinates coordinates)
        {
            _coordinates = coordinates;
        }

        public int countAvailableTiles()
        {
            return lookForAvailableCells(true);
        }
        public bool isEnemy()
        {
            return color != Player.currentPlayer.color;
        }
        protected bool inRange(int x, int y)
        {
            return x >= 0 && x < 8 && y >= 0 && y < 8;
        }
        protected bool inRange(Coordinates coordinates)
        {
            return coordinates.x >= 0 && coordinates.x < 8 && coordinates.y >= 0 && coordinates.y < 8;
        }
        protected int multiStepsMoving(int[] dx, int[] dy,bool countOnly)
        {
            int ret = 0;
            Board.instance.of(coordinates).switchMark(countOnly);
            for (int i = 0; i < dx.Length; i++)
            {
                Coordinates c = new Coordinates(coordinates.x + dx[i], coordinates.y + dy[i]);
                while (true)
                {
                    if (inRange(c))
                    {
                        BoardTile tile = Board.instance.of(c);
                        if (tile.isEmpty())
                        {
                            if (GameObserver.isSafeMove(coordinates, c))
                            {
                                tile.switchMark(countOnly);
                                ret++;
                            }
                        }
                        else if (tile.piece.isEnemy())
                        {
                            if(GameObserver.isSafeMove(coordinates, c))
                            {
                                tile.switchMark(countOnly);
                                ret++;
                            }
                            break;
                        }
                        else break;
                        c = new Coordinates(c.x + dx[i], c.y + dy[i]);
                    }
                    else break;
                }
            }
            return ret;
        }
        protected void multiStepsAttack(int[] dx, int[] dy)
        {
            for (int i = 0; i < dx.Length; i++)
            {
                Coordinates c = new Coordinates(coordinates.x + dx[i], coordinates.y + dy[i]);
                while (true)
                {
                    if (inRange(c))
                    {
                        BoardTile tile = Board.instance.of(c);
                        if (tile.isEmpty())
                        {
                            tile.markDanger(true);
                        }
                        else
                        {
                            tile.markDanger(true);
                            break;
                        }
                        c = new Coordinates(c.x + dx[i], c.y + dy[i]);
                    }
                    else break;
                }
            }
        }

        public virtual void attack()
        {

        }
    }
    
    
    
    
    

}
