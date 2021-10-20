using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessSFML
{
    public class Knight : PieceBase
    {
        public override int SpriteID => 2;
        public Knight(PieceColor color) : base(color)
        {
            
        }
    }
}
