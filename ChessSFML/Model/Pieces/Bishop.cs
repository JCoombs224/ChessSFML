using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessSFML
{
    public class Bishop : PieceBase
    {
        public Bishop(PieceColor color) :  base(color)
        {
            MoveDiagonal = true;
        }
    }
}
