using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using SFML.Graphics;

namespace ChessSFML
{
    public abstract class PieceBase
    {
        public readonly PieceColor pieceColor;

        protected bool MoveVertical;
        protected bool MoveHorizontal;
        protected bool MoveDiagonal;
        protected int MoveCap = 0;


        public PieceBase(PieceColor color)
        {
            pieceColor = color;
        }

        public bool canMoveVertical()
        {
            return MoveVertical;
        }
        public bool canMoveHorizontal()
        {
            return MoveHorizontal;
        }
        public bool canMoveDiagonal()
        {
            return MoveDiagonal;
        }

        public int getMoveCap()
        {
            return MoveCap;
        }


    }

    public enum PieceColor
    {
        White,
        Black
    }

}
