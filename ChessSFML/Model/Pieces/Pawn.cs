using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessSFML
{
    public class Pawn : PieceBase
    {
        private bool isFirstMove = true;

        public Pawn(PieceColor color) : base(color)
        {
            MoveVertical = true;
            MoveCap = 2;
        }

        public bool IsFirstMove()
        {
            return isFirstMove;
        }
        public void doFirstMove()
        {
            MoveCap = 1;
            isFirstMove = false;
        }

    }
}
