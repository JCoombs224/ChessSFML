using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace ChessSFML
{
    public class Board
    {
        private Square[,] grid = new Square[8,8];

        public Board()
        {
            initializeBoard();
        }

        public Square sqaureAt(int x, int y)
        {
            return grid[x, y];
        }

        public int scanPosX(int x, int y)
        {
            for(int i = x; i < 8; i++)
            {
                if (grid[i, y].hasPiece()) return i;
            }
            return 0;
        }
        public int scanNegX(int x, int y)
        {
            for (int i = x; i >= 0; i--)
            {
                if (grid[i, y].hasPiece()) return i;
            }
            return 0;
        }
        public int scanPosY(int x, int y)
        {
            for (int i = y; i < 8; i++)
            {
                if (grid[x, i].hasPiece()) return i;
            }
            return 0;
        }
        public int scanNegY(int x, int y)
        {
            for (int i = y; i >= 0; i--)
            {
                if (grid[x, i].hasPiece()) return i;
            }
            return 0;
        }
        public int scanPosXY(int x, int y)
        {
            int j = y;
            for (int i = x; i < 8; i++)
            {
                if (grid[i, j].hasPiece()) return i;
                j++;
            }
            return 0;
        }
        public int scanNegXY(int x, int y)
        {
            int j = y;
            for (int i = x; i >= 0; i--)
            {
                if (grid[i, j].hasPiece()) return i;
                j++;
            }
            return 0;
        }

        private void initializeBoard()
        {
            grid[0, 0].setPiece(new Rook(PieceColor.White));
            grid[1, 0].setPiece(new Knight(PieceColor.White));
            grid[2, 0].setPiece(new Bishop(PieceColor.White));
            grid[3, 0].setPiece(new King(PieceColor.White));
            grid[4, 0].setPiece(new Queen(PieceColor.White));
            grid[5, 0].setPiece(new Bishop(PieceColor.White));
            grid[6, 0].setPiece(new Knight(PieceColor.White));
            grid[7, 0].setPiece(new Rook(PieceColor.White));
            grid[0, 1].setPiece(new Pawn(PieceColor.White));
            grid[1, 1].setPiece(new Pawn(PieceColor.White));
            grid[2, 1].setPiece(new Pawn(PieceColor.White));
            grid[3, 1].setPiece(new Pawn(PieceColor.White));
            grid[4, 1].setPiece(new Pawn(PieceColor.White));
            grid[5, 1].setPiece(new Pawn(PieceColor.White));
            grid[6, 1].setPiece(new Pawn(PieceColor.White));
            grid[7, 1].setPiece(new Pawn(PieceColor.White));

            grid[0, 7].setPiece(new Rook(PieceColor.Black));
            grid[1, 7].setPiece(new Knight(PieceColor.Black));
            grid[2, 7].setPiece(new Bishop(PieceColor.Black));
            grid[3, 7].setPiece(new King(PieceColor.Black));
            grid[4, 7].setPiece(new Queen(PieceColor.Black));
            grid[5, 7].setPiece(new Bishop(PieceColor.Black));
            grid[6, 7].setPiece(new Knight(PieceColor.Black));
            grid[7, 7].setPiece(new Rook(PieceColor.Black));
            grid[0, 6].setPiece(new Pawn(PieceColor.Black));
            grid[1, 6].setPiece(new Pawn(PieceColor.Black));
            grid[2, 6].setPiece(new Pawn(PieceColor.Black));
            grid[3, 6].setPiece(new Pawn(PieceColor.Black));
            grid[4, 6].setPiece(new Pawn(PieceColor.Black));
            grid[5, 6].setPiece(new Pawn(PieceColor.Black));
            grid[6, 6].setPiece(new Pawn(PieceColor.Black));
            grid[7, 6].setPiece(new Pawn(PieceColor.Black));
        }
    }
}
