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
        public int posX;
        public int posY;

        public Square(int x, int y)
        {
            currentPiece = null;
            posX = x;
            posY = y;
        }
        public Square(PieceBase piece, int x, int y)
        {
            this.currentPiece = piece;
            posX = x;
            posY = y;
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

        public bool hasEnemyPiece(PieceColor currentPlayerColor)
        {
            if (currentPiece == null)
                return false;
            if (currentPlayerColor == currentPiece.pieceColor)
                return false;

            return true;
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
