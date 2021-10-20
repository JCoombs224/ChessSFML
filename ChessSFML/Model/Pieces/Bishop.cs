using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessSFML
{
    public class Bishop : PieceBase
    {
        public override int SpriteID => 3;
        public Bishop(PieceColor color) :  base(color)
        {
            MoveDiagonal = true;
        }
    }
}
