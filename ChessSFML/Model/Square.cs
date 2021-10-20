using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessSFML
{
    public class Square
    {
        private PieceBase currentPiece;
        public bool isSelected = false;

        public Square()
        {
            currentPiece = null;
        }
        public Square(PieceBase piece)
        {
            this.currentPiece = piece;
        }

        public void Select()
        {
            isSelected = true;
        }
        public void Deselect()
        {
            isSelected = false;
        }

        public void setPiece(PieceBase piece)
        {
            this.currentPiece = piece;
        }

        public bool hasPiece()
        {
            return currentPiece != null;
        }

        public PieceBase getPiece()
        {
            return this.currentPiece;
        }

        public void removePiece()
        {
            if(this.hasPiece())
                this.currentPiece = null;
        }

        public void movePiece(Square other)
        {
            other.setPiece(this.currentPiece);
            this.removePiece();
        }
    }
}
