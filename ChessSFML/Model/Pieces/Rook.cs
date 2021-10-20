using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessSFML
{
    public class Rook : PieceBase
    {
        public override int SpriteID => 1;
        public Rook(PieceColor color) : base(color)
        {
            MoveVertical = true;
            MoveHorizontal = true;
        }
    }
}
