using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessSFML
{
    public class Queen : PieceBase
    {
        public override int SpriteID => 4;
        public Queen(PieceColor color) : base(color)
        {
            MoveVertical = true;
            MoveHorizontal = true;
            MoveDiagonal = true;
        }

        
    }
}
