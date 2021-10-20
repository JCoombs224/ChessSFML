using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using SFML.Graphics;
using SFML.Audio;
using SFML.Window;
using SFML.System;

namespace ChessSFML
{
    public class Board
    {
        // Settings can be changed in Game class
        const string BoardFilePath = "C:\\Users\\Jamison\\Google Drive\\Programming Projects\\Chess\\ChessSFML\\ChessSFML\\res\\board.png";
        const string PiecesFilePath = "C:\\Users\\Jamison\\Google Drive\\Programming Projects\\Chess\\ChessSFML\\ChessSFML\\res\\pieces.png";
        static readonly int RawTextureSize = Game.RawTextureSize;
        static readonly int TextureScale = Game.TextureScale;
        static readonly int TextureSize = Game.TextureSize;
        // Load in textures/sprites
        static Texture BoardTexture = new Texture(BoardFilePath);
        static Sprite BoardSprite = new Sprite(BoardTexture);
        static Texture PieceTexture = new Texture(PiecesFilePath);
        static Sprite PieceSprite = new Sprite(PieceTexture);

        public static Square SelectedSquare;
        private HashSet<Vector2i> SelectedPieceMoves = new HashSet<Vector2i>();

        // Create Pieces
        Pawn[] wP = new Pawn[8];
        Pawn[] bP = new Pawn[8];
        Rook[] wRook = new Rook[2];
        Rook[] bRook = new Rook[2];
        Bishop[] wBishop = new Bishop[2];
        Bishop[] bBishop = new Bishop[2];
        Knight[] wKnight = new Knight[2];
        Knight[] bKnight = new Knight[2];
        Queen wQ;
        Queen bQ;
        King wK;
        King bK;

        private static Square[,] grid;

        public Board()
        {
            BoardSprite.Scale = new Vector2f(TextureScale, TextureScale);
            PieceSprite.Scale = new Vector2f(TextureScale, TextureScale);
            grid = new Square[8, 8];
            initializeBoard();
        }

        public void DrawBoard(ref RenderWindow window)
        {
            int z = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    BoardSprite.TextureRect = new IntRect(BoardSpriteMap[z++] * RawTextureSize, 0, RawTextureSize, RawTextureSize);
                    BoardSprite.Position = new Vector2f(i * TextureSize, j * TextureSize);
                    window.Draw(BoardSprite);

                    if(grid[i, j] == SelectedSquare)
                    {
                        BoardSprite.TextureRect = new IntRect(2 * RawTextureSize, 0, RawTextureSize, RawTextureSize);
                        window.Draw(BoardSprite);
                    }

                    if (grid[i, j].hasPiece())
                        window.Draw(DrawPieceSprite(i, j));
                }
            }
        }

        private Sprite DrawPieceSprite(int x, int y)
        {
            PieceBase piece = grid[x, y].getPiece();

            PieceSprite.TextureRect = new IntRect(piece.SpriteID * RawTextureSize,
                piece.SpriteColorID * RawTextureSize, RawTextureSize, RawTextureSize);
            PieceSprite.Position = new Vector2f(x * TextureSize, y * TextureSize);

            return PieceSprite;
        }

        private void DrawSelectedPieceMoves(ref RenderWindow window, PieceBase piece, int x, int y)
        {
            
        }

        private void GetSelectedPieceMoves(PieceBase piece, int x, int y)
        {
            PieceColor color = piece.pieceColor;
            int xMax = 7, xMin = 0;
            int yMax = 7, yMin = 0;

            if(piece.getMoveCap() != 0)
            {
                xMax = x + piece.getMoveCap();
                xMin = x - piece.getMoveCap();
                yMax = y + piece.getMoveCap();
                yMin = y - piece.getMoveCap();
            }

            if(piece is Pawn)
            {
                Pawn pawn = (Pawn)piece;
                if(color == PieceColor.White)
                {
                    
                }
            }

        }

        public Square SqaureAt(int x, int y)
        {
            if (x < 0 || x > 7 || y < 0 || y > 7)
                return null;

            return grid[x, y];
        }

        public void SelectSquare(int x, int y)
        {
            SelectedSquare = grid[x,y];
            GetSelectedPieceMoves(grid[x,y].getPiece(), x, y);
        }
        public void Deselect()
        {
            SelectedSquare = null;
            SelectedPieceMoves.Clear();
        }

        public Square GetSelectedSquare()
        {
            return SelectedSquare;
        }

        public int scanPosX(int x, int y, int max)
        {
            for(int i = x; i < 8; i++)
            {
                if (grid[i, y].hasPiece()) return i;
            }
            return 0;
        }
        public int scanNegX(int x, int y, int max)
        {
            for (int i = x; i >= 0; i--)
            {
                if (grid[i, y].hasPiece()) return i;
            }
            return 0;
        }
        public int scanPosY(int x, int y, int max)
        {
            for (int i = y; i < 8; i++)
            {
                if (grid[x, i].hasPiece()) return i;
            }
            return 0;
        }
        public int scanNegY(int x, int y, int max)
        {
            for (int i = y; i >= 0; i--)
            {
                if (grid[x, i].hasPiece()) return i;
            }
            return 0;
        }
        public int scanPosXY(int x, int y, int max)
        {
            int j = y;
            for (int i = x; i < 8; i++)
            {
                if (grid[i, j].hasPiece()) return i;
                j++;
            }
            return 0;
        }
        public int scanNegXY(int x, int y, int max)
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
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    grid[i, j] = new Square();

            grid[0, 7].setPiece(wRook[0] = new Rook(PieceColor.White));
            grid[1, 7].setPiece(wKnight[0] = new Knight(PieceColor.White));
            grid[2, 7].setPiece(wBishop[0] = new Bishop(PieceColor.White));
            grid[3, 7].setPiece(wK = new King(PieceColor.White));
            grid[4, 7].setPiece(wQ = new Queen(PieceColor.White));
            grid[5, 7].setPiece(wBishop[1] = new Bishop(PieceColor.White));
            grid[6, 7].setPiece(wKnight[1] = new Knight(PieceColor.White));
            grid[7, 7].setPiece(wRook[1] = new Rook(PieceColor.White));
            for(int i = 0; i < 8; i++)
                grid[i, 6].setPiece(wP[i] = new Pawn(PieceColor.White));

            grid[0, 0].setPiece(bRook[0] = new Rook(PieceColor.Black));
            grid[1, 0].setPiece(bKnight[0] = new Knight(PieceColor.Black));
            grid[2, 0].setPiece(bBishop[0] = new Bishop(PieceColor.Black));
            grid[3, 0].setPiece(bK = new King(PieceColor.Black));
            grid[4, 0].setPiece(bQ = new Queen(PieceColor.Black));
            grid[5, 0].setPiece(bBishop[1] = new Bishop(PieceColor.Black));
            grid[6, 0].setPiece(bKnight[1] = new Knight(PieceColor.Black));
            grid[7, 0].setPiece(bRook[1] = new Rook(PieceColor.Black));
            for (int i = 0; i < 8; i++)
                grid[i, 1].setPiece(bP[i] = new Pawn(PieceColor.Black));
        }

        // 0 = white sqaure, 1 = 'black' square
        static readonly int[] BoardSpriteMap =
        {
            0, 1, 0, 1, 0, 1, 0, 1,
            1, 0, 1, 0, 1, 0, 1, 0,
            0, 1, 0, 1, 0, 1, 0, 1,
            1, 0, 1, 0, 1, 0, 1, 0,
            0, 1, 0, 1, 0, 1, 0, 1,
            1, 0, 1, 0, 1, 0, 1, 0,
            0, 1, 0, 1, 0, 1, 0, 1,
            1, 0, 1, 0, 1, 0, 1, 0,
        };
    }
}
