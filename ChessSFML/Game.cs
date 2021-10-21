using System;
using SFML.System;
using SFML.Graphics;
using SFML.Window;
using System.IO;

namespace ChessSFML
{
    public class Game
    {
        // Settings
        public readonly uint GameResolutionY;
        public readonly uint GameResolutionX;
        public static readonly int RawTextureSize = 32;
        public static readonly int TextureScale = 4;
        public static readonly int TextureSize = RawTextureSize * TextureScale;
        static string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
        static string FontPath = String.Format("{0}Resources\\font\\ARLRDBD.TTF", Path.GetFullPath(Path.Combine(RunningPath, @"")));
        public static PieceColor CurrentColor = PieceColor.White;

        private static bool GameWon = false;
        static Board board = new Board();

        private int mouseX;
        private int mouseY;

        // Text Settings
        static Font font = new Font(FontPath);
        static Text playerText = new Text();

        public Game(uint res)
        {
            GameResolutionY = res;
            GameResolutionX = (uint)(res * 1.3);
        }

        public void Show()
        {
            float deltaTime = 0.00f;
            Clock clock = new Clock();

            VideoMode mode = new VideoMode(GameResolutionX, GameResolutionY);
            RenderWindow window = new RenderWindow(mode, "Chess", Styles.Titlebar | Styles.Close);

            window.KeyPressed += OnKeyPressed;
            window.Closed += OnClosed;
            window.MouseButtonPressed += OnMouseClick;
            
            // Window Settings
            Vector2u windowSize = window.Size;
            float vCenterX = (8 * TextureSize) / 2;
            float vCenterY = (8 * TextureSize) / 2;
            View GameView = new View(new Vector2f(vCenterX, vCenterY), new Vector2f(8, 8));
            window.SetView(GameView);
            window.SetVerticalSyncEnabled(true);

            //mouseX = (int)pos.X / TextureSize;
            //mouseY = (int)pos.Y / TextureSize;

            playerText.Font = font;
            playerText.CharacterSize = 28;
            playerText.Position = new Vector2f(GameResolutionY + 60, vCenterY - 28);
            playerText.DisplayedString = "White's Turn";
            playerText.FillColor = Color.White;

            while (window.IsOpen)
            {
                deltaTime = clock.Restart().AsSeconds();
                window.Clear(new Color(161, 117, 82));
                window.SetView(window.DefaultView);

                Vector2i mPosWindow = Mouse.GetPosition(window);
                Vector2f pos = window.MapPixelToCoords(mPosWindow);
                mouseX = mPosWindow.X / TextureSize;
                mouseY = mPosWindow.Y / TextureSize;

                board.DrawBoard(ref window);
                window.Draw(playerText);
                window.Display();

                window.DispatchEvents();
            }
        }

        public static void ChangePlayer()
        {
            if (CurrentColor == PieceColor.White)
            {
                CurrentColor = PieceColor.Black;
                playerText.DisplayedString = "Black's Turn";
                playerText.FillColor = Color.Black;
            }
            else
            {
                CurrentColor = PieceColor.White;
                playerText.DisplayedString = "White's Turn";
                playerText.FillColor = Color.White;
            }
        }

        public static void GameOver()
        {
            playerText.DisplayedString = "Checkmate!!\n" + CurrentColor.ToString() + " Wins!!";
            GameWon = true;
        }

        /// <summary>
        /// Function called when the window is closed
        /// </summary>
        static void OnClosed(object sender, EventArgs e)
        {
            var window = (RenderWindow)sender;
            window.Close();
        }

        private void OnKeyPressed(object sender, SFML.Window.KeyEventArgs e)
        {
            var window = (SFML.Window.Window)sender;
            if (e.Code == SFML.Window.Keyboard.Key.Escape)
            {
                window.Close();
            }

        }

        private void OnMouseClick(object sender, SFML.Window.MouseButtonEventArgs e)
        {
            var window = (SFML.Window.Window)sender;
            if (e.Button == SFML.Window.Mouse.Button.Left)
            {
                if (mouseX < 0 || mouseX > 7 || mouseY < 0 || mouseY > 7)
                    return;

                if (!GameWon)
                {
                    Square squareClicked = board.SqaureAt(mouseX, mouseY);
                    if (squareClicked == null)
                        board.Deselect();
                    if (board.SelectedPieceMoves.Contains(squareClicked))
                    {
                        board.MoveSelectedPiece(squareClicked, mouseX, mouseY);
                    }
                    else if (squareClicked.hasPiece() && squareClicked.getPiece().pieceColor == CurrentColor)
                        board.SelectSquare(mouseX, mouseY);
                    else
                        board.Deselect();
                }

                Console.Out.Write(mouseX + " ");
                Console.Out.Write(mouseY + "\n");
            }
        }


    }
}
