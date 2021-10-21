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
        const string BOARD_TEXTURE_FILE_PATH = "C:\\Users\\Jamison\\Google Drive\\Programming Projects\\Chess\\ChessSFML\\ChessSFML\\res\\board.png";
        const string PIECE_TEXTURE_FILE_PATH = "C:\\Users\\Jamison\\Google Drive\\Programming Projects\\Chess\\ChessSFML\\ChessSFML\\res\\pieces.png";
        static readonly int RawTextureSize = Game.RawTextureSize;
        static readonly int TextureScale = Game.TextureScale;
        static readonly int TextureSize = Game.TextureSize;
        // Load in textures/sprites
        static Texture BoardTexture = new Texture(BOARD_TEXTURE_FILE_PATH);
        static Sprite BoardSprite = new Sprite(BoardTexture);
        static Texture PieceTexture = new Texture(PIECE_TEXTURE_FILE_PATH);
        static Sprite PieceSprite = new Sprite(PieceTexture);
        // Load in sounds
        private static SoundBuffer SelectSoundBuffer = new SoundBuffer("C:\\Users\\Jamison\\Google Drive\\Programming Projects\\Chess\\ChessSFML\\ChessSFML\\res\\sound\\select.ogg");
        private static SoundBuffer DeselectSoundBuffer = new SoundBuffer("C:\\Users\\Jamison\\Google Drive\\Programming Projects\\Chess\\ChessSFML\\ChessSFML\\res\\sound\\deselect.ogg");
        private static SoundBuffer[] MoveSoundBuffer = new SoundBuffer[4];
        private Sound SelectSound = new Sound(SelectSoundBuffer);
        private Sound DeselectSound = new Sound(DeselectSoundBuffer);
        private Sound MoveSound = new Sound();

        public static Square SelectedSquare;
        public HashSet<Square> SelectedPieceMoves = new HashSet<Square>();

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

                    if (grid[i, j] == SelectedSquare)
                    {
                        BoardSprite.TextureRect = new IntRect(2 * RawTextureSize, 0, RawTextureSize, RawTextureSize);
                        BoardSprite.Color = Color.Green;
                        window.Draw(BoardSprite);
                        BoardSprite.Color = Color.White;
                    }
                    if (SelectedPieceMoves.Contains(grid[i, j]))
                    {
                        BoardSprite.TextureRect = new IntRect(2 * RawTextureSize, 0, RawTextureSize, RawTextureSize);
                        if (grid[i, j].getPiece() is King)
                        {
                            BoardSprite.Color = Color.Red;
                            window.Draw(BoardSprite);
                            BoardSprite.Color = Color.White;
                        }
                        else
                            window.Draw(BoardSprite);
                    }

                    if (grid[i, j].hasPiece())
                        window.Draw(DrawPieceSprite(i, j));
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
            SelectedSquare = grid[x, y];
            SelectedPieceMoves.Clear();
            GetSelectedPieceMoves(grid[x, y].getPiece(), x, y);
            SelectSound.Play();
        }
        public void Deselect(bool playSnd = true)
        {
            SelectedSquare = null;
            SelectedPieceMoves.Clear();
            if (playSnd) DeselectSound.Play();
        }

        public Square GetSelectedSquare()
        {
            return SelectedSquare;
        }

        private Sprite DrawPieceSprite(int x, int y)
        {
            PieceBase piece = grid[x, y].getPiece();

            PieceSprite.TextureRect = new IntRect(piece.SpriteID * RawTextureSize,
                piece.SpriteColorID * RawTextureSize, RawTextureSize, RawTextureSize);
            PieceSprite.Position = new Vector2f(x * TextureSize, y * TextureSize);

            return PieceSprite;
        }

        public void MoveSelectedPiece(Square square, int x, int y)
        {
            int oldX = SelectedSquare.posX;
            int oldY = SelectedSquare.posY;
            PieceBase piece = SelectedSquare.getPiece();

            if (piece is Pawn)
                piece.MoveCap = 1;

            // Checkmate
            if (square.hasPiece() && square.getPiece() is King)
                Game.GameOver();
            else
                Game.ChangePlayer();

            square.setPiece(piece);
            grid[x, y] = square;
            grid[oldX, oldY].setPiece(null);
            Deselect(false); // The false is so deselect is not played
            SelectedPieceMoves.Clear();

            Random rnd = new Random();
            MoveSound.SoundBuffer = MoveSoundBuffer[rnd.Next(4)];
            MoveSound.Play();

        }

        private void GetSelectedPieceMoves(PieceBase piece, int x, int y)
        {
            PieceColor color = piece.pieceColor;
            int xMax = 7, xMin = 0;
            int yMax = 7, yMin = 0;

            if (piece.getMoveCap() != 0)
            {
                xMax = x + piece.getMoveCap();
                xMin = x - piece.getMoveCap();
                yMax = y + piece.getMoveCap();
                yMin = y - piece.getMoveCap();
            }

            // Pawn has special move set
            if (piece is Pawn)
                GetPawnMoves(color, x, y, yMin, yMax);

            // Knight has special move set
            if (piece is Knight)
                GetKnightMoves(color, x, y);

            if (piece is Rook)
                GetRookMoves(color, x, y);

            if (piece is Bishop)
                GetBishopMoves(color, x, y);

            if (piece is Queen)
                GetQueenMoves(color, x, y);

            if (piece is King)
                GetKingMoves(color, x, y);
        }

        // TODO: Definitely redo this
        private void GetKingMoves(PieceColor color, int x, int y)
        {
            if (x < 7)
            {
                if (grid[x + 1, y].hasEnemyPiece(color))
                    SelectedPieceMoves.Add(grid[x + 1, y]);
                else if (!grid[x + 1, y].hasPiece())
                    SelectedPieceMoves.Add(grid[x + 1, y]);
                if (y < 7)
                {
                    if (grid[x + 1, y + 1].hasEnemyPiece(color))
                        SelectedPieceMoves.Add(grid[x + 1, y + 1]);
                    else if (!grid[x + 1, y + 1].hasPiece())
                        SelectedPieceMoves.Add(grid[x + 1, y + 1]);
                }
                if (y > 0)
                {
                    if (grid[x + 1, y - 1].hasEnemyPiece(color))
                        SelectedPieceMoves.Add(grid[x + 1, y - 1]);
                    else if (!grid[x + 1, y - 1].hasPiece())
                        SelectedPieceMoves.Add(grid[x + 1, y - 1]);
                }
            }
            if (x > 0)
            {
                if (grid[x - 1, y].hasEnemyPiece(color))
                    SelectedPieceMoves.Add(grid[x - 1, y]);
                else if (!grid[x - 1, y].hasPiece())
                    SelectedPieceMoves.Add(grid[x - 1, y]);
                if (y < 7)
                {
                    if (grid[x - 1, y + 1].hasEnemyPiece(color))
                        SelectedPieceMoves.Add(grid[x - 1, y + 1]);
                    else if (!grid[x - 1, y + 1].hasPiece())
                        SelectedPieceMoves.Add(grid[x - 1, y + 1]);
                }
                if (y > 0)
                {
                    if (grid[x - 1, y - 1].hasEnemyPiece(color))
                        SelectedPieceMoves.Add(grid[x - 1, y - 1]);
                    else if (!grid[x - 1, y - 1].hasPiece())
                        SelectedPieceMoves.Add(grid[x - 1, y - 1]);
                }
            }
            if (y < 7)
            {
                if (grid[x, y + 1].hasEnemyPiece(color))
                    SelectedPieceMoves.Add(grid[x, y + 1]);
                else if (!grid[x, y + 1].hasPiece())
                    SelectedPieceMoves.Add(grid[x, y + 1]);
            }
            if (y > 0)
            {
                if (grid[x, y - 1].hasEnemyPiece(color))
                    SelectedPieceMoves.Add(grid[x, y - 1]);
                else if (!grid[x, y - 1].hasPiece())
                    SelectedPieceMoves.Add(grid[x, y - 1]);
            }
        }

        private void GetQueenMoves(PieceColor color, int x, int y)
        {
            ScanX(color, x, y);
            ScanY(color, x, y);
            ScanDiagonals(color, x, y);
        }

        private void GetRookMoves(PieceColor color, int x, int y)
        {
            ScanX(color, x, y);
            ScanY(color, x, y);
        }

        private void GetBishopMoves(PieceColor color, int x, int y)
        {
            ScanDiagonals(color, x, y);
        }

        private void ScanX(PieceColor color, int x, int y)
        {
            // Scan positive X
            for (int i = x + 1; i < 8; i++)
            {
                if (grid[i, y].hasEnemyPiece(color))
                {
                    SelectedPieceMoves.Add(grid[i, y]);
                    break;
                }
                else if (grid[i, y].hasPiece())
                    break;

                SelectedPieceMoves.Add(grid[i, y]);
            }
            // Scan negative X
            for (int i = x - 1; i >= 0; i--)
            {
                if (grid[i, y].hasEnemyPiece(color))
                {
                    SelectedPieceMoves.Add(grid[i, y]);
                    break;
                }
                else if (grid[i, y].hasPiece())
                    break;

                SelectedPieceMoves.Add(grid[i, y]);
            }
        }

        private void ScanY(PieceColor color, int x, int y)
        {
            // Scan positive Y
            for (int i = y + 1; i < 8; i++)
            {
                if (grid[x, i].hasEnemyPiece(color))
                {
                    SelectedPieceMoves.Add(grid[x, i]);
                    break;
                }
                else if (grid[x, i].hasPiece())
                    break;

                SelectedPieceMoves.Add(grid[x, i]);
            }
            // Scan negative Y
            for (int i = y - 1; i >= 0; i--)
            {
                if (grid[x, i].hasEnemyPiece(color))
                {
                    SelectedPieceMoves.Add(grid[x, i]);
                    break;
                }
                else if (grid[x, i].hasPiece())
                    break;

                SelectedPieceMoves.Add(grid[x, i]);
            }
        }

        // TODO: OPTIMIZE
        private void ScanDiagonals(PieceColor color, int x, int y)
        {
            int j = y + 1;
            // Scan +x +y
            if (j < 8)
                for (int i = x + 1; i < 8; i++)
                {
                    if (grid[i, j].hasEnemyPiece(color))
                    {
                        SelectedPieceMoves.Add(grid[i, j]);
                        break;
                    }
                    else if (grid[i, j].hasPiece())
                        break;

                    SelectedPieceMoves.Add(grid[i, j]);

                    if (j == 7)
                        break;
                    j++;
                }

            j = y + 1;
            // Scan -x +y
            if (j < 8)
                for (int i = x - 1; i >= 0; i--)
                {
                    if (grid[i, j].hasEnemyPiece(color))
                    {
                        SelectedPieceMoves.Add(grid[i, j]);
                        break;
                    }
                    else if (grid[i, j].hasPiece())
                        break;

                    SelectedPieceMoves.Add(grid[i, j]);

                    if (j == 7)
                        break;
                    j++;
                }

            j = y - 1;
            // Scan +x -y
            if (j >= 0)
                for (int i = x + 1; i < 8; i++)
                {
                    if (grid[i, j].hasEnemyPiece(color))
                    {
                        SelectedPieceMoves.Add(grid[i, j]);
                        break;
                    }
                    else if (grid[i, j].hasPiece())
                        break;

                    SelectedPieceMoves.Add(grid[i, j]);

                    if (j == 0)
                        break;
                    j--;
                }

            j = y - 1;
            // Scan -x -y
            if (j >= 0)
                for (int i = x - 1; i >= 0; i--)
                {
                    if (grid[i, j].hasEnemyPiece(color))
                    {
                        SelectedPieceMoves.Add(grid[i, j]);
                        break;
                    }
                    else if (grid[i, j].hasPiece())
                        break;

                    SelectedPieceMoves.Add(grid[i, j]);

                    if (j == 0)
                        break;
                    j--;
                }
        }

        private void GetKnightMoves(PieceColor color, int x, int y)
        {
            // Vertical movement
            if (x - 1 >= 0 && y - 2 >= 0)
                if (!grid[x - 1, y - 2].hasPiece() || grid[x - 1, y - 2].hasEnemyPiece(color))
                    SelectedPieceMoves.Add((grid[x - 1, y - 2]));

            if (x + 1 < 8 && y - 2 >= 0)
                if (!grid[x + 1, y - 2].hasPiece() || grid[x + 1, y - 2].hasEnemyPiece(color))
                    SelectedPieceMoves.Add((grid[x + 1, y - 2]));

            if (x - 1 >= 0 && y + 2 < 8)
                if (!grid[x - 1, y + 2].hasPiece() || grid[x - 1, y + 2].hasEnemyPiece(color))
                    SelectedPieceMoves.Add((grid[x - 1, y + 2]));

            if (x + 1 < 8 && y + 2 < 8)
                if (!grid[x + 1, y + 2].hasPiece() || grid[x + 1, y + 2].hasEnemyPiece(color))
                    SelectedPieceMoves.Add((grid[x + 1, y + 2]));

            // Horizontal movement
            if (x - 2 >= 0 && y - 1 >= 0)
                if (!grid[x - 2, y - 1].hasPiece() || grid[x - 2, y - 1].hasEnemyPiece(color))
                    SelectedPieceMoves.Add((grid[x - 2, y - 1]));

            if (x + 2 < 8 && y - 1 >= 0)
                if (!grid[x + 2, y - 1].hasPiece() || grid[x + 2, y - 1].hasEnemyPiece(color))
                    SelectedPieceMoves.Add((grid[x + 2, y - 1]));

            if (x - 2 >= 0 && y + 1 < 8)
                if (!grid[x - 2, y + 1].hasPiece() || grid[x - 2, y + 1].hasEnemyPiece(color))
                    SelectedPieceMoves.Add((grid[x - 2, y + 1]));

            if (x + 2 < 8 && y + 1 < 8)
                if (!grid[x + 2, y + 1].hasPiece() || grid[x + 2, y + 1].hasEnemyPiece(color))
                    SelectedPieceMoves.Add((grid[x + 2, y + 1]));
        }

        private void GetPawnMoves(PieceColor color, int x, int y, int yMin, int yMax)
        {
            if (color == PieceColor.White)
            {
                if (y > 0)
                    for (int i = y - 1; i >= yMin; i--)
                    {
                        if (i != y - 2)
                        {
                            if (x < 7 && grid[x + 1, i].hasEnemyPiece(color))
                                SelectedPieceMoves.Add(grid[x + 1, i]);
                            if (x > 0 && grid[x - 1, i].hasEnemyPiece(color))
                                SelectedPieceMoves.Add(grid[x - 1, i]);
                        }
                        if (grid[x, i].hasPiece())
                            break;

                        SelectedPieceMoves.Add(grid[x, i]);
                    }
            }
            else
            {
                if (y < 7)
                    for (int i = y + 1; i <= yMax; i++)
                    {
                        if (i != y + 2)
                        {
                            if (x < 7 && grid[x + 1, i].hasEnemyPiece(color))
                                SelectedPieceMoves.Add(grid[x + 1, i]);
                            if (x > 0 && grid[x - 1, i].hasEnemyPiece(color))
                                SelectedPieceMoves.Add(grid[x - 1, i]);
                        }
                        if (grid[x, i].hasPiece())
                            break;

                        SelectedPieceMoves.Add(grid[x, i]);
                    }
            }
        }

        private void initializeBoard()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    grid[i, j] = new Square(i, j);

            grid[0, 7].setPiece(new Rook(PieceColor.White));
            grid[1, 7].setPiece(new Knight(PieceColor.White));
            grid[2, 7].setPiece(new Bishop(PieceColor.White));
            grid[3, 7].setPiece(new King(PieceColor.White));
            grid[4, 7].setPiece(new Queen(PieceColor.White));
            grid[5, 7].setPiece(new Bishop(PieceColor.White));
            grid[6, 7].setPiece(new Knight(PieceColor.White));
            grid[7, 7].setPiece(new Rook(PieceColor.White));
            for (int i = 0; i < 8; i++)
                grid[i, 6].setPiece(new Pawn(PieceColor.White));

            grid[0, 0].setPiece(new Rook(PieceColor.Black));
            grid[1, 0].setPiece(new Knight(PieceColor.Black));
            grid[2, 0].setPiece(new Bishop(PieceColor.Black));
            grid[3, 0].setPiece(new King(PieceColor.Black));
            grid[4, 0].setPiece(new Queen(PieceColor.Black));
            grid[5, 0].setPiece(new Bishop(PieceColor.Black));
            grid[6, 0].setPiece(new Knight(PieceColor.Black));
            grid[7, 0].setPiece(new Rook(PieceColor.Black));
            for (int i = 0; i < 8; i++)
                grid[i, 1].setPiece(new Pawn(PieceColor.Black));

            MoveSoundBuffer[0] = new SoundBuffer("C:\\Users\\Jamison\\Google Drive\\Programming Projects\\Chess\\ChessSFML\\ChessSFML\\res\\sound\\place1.ogg");
            MoveSoundBuffer[1] = new SoundBuffer("C:\\Users\\Jamison\\Google Drive\\Programming Projects\\Chess\\ChessSFML\\ChessSFML\\res\\sound\\place2.ogg");
            MoveSoundBuffer[2] = new SoundBuffer("C:\\Users\\Jamison\\Google Drive\\Programming Projects\\Chess\\ChessSFML\\ChessSFML\\res\\sound\\place3.ogg");
            MoveSoundBuffer[3] = new SoundBuffer("C:\\Users\\Jamison\\Google Drive\\Programming Projects\\Chess\\ChessSFML\\ChessSFML\\res\\sound\\place4.ogg");
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
