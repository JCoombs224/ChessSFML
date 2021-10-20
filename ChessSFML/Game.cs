using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using SFML.Audio;
using SFML.Window;

namespace ChessSFML
{
    public class Game
    {
        // Settings
        public readonly uint GameResolution;
        public static readonly int RawTextureSize = 32;
        public static readonly int TextureScale = 4;
        public static readonly int TextureSize = RawTextureSize * TextureScale;
        static PieceColor CurrentColor = PieceColor.White;
        static Board board = new Board();

        private int mouseX;
        private int mouseY;

        public Game(uint res)
        {
            GameResolution = res;
        }

        public void Show()
        {
            float deltaTime = 0.00f;
            Clock clock = new Clock();

            VideoMode mode = new VideoMode(GameResolution, GameResolution);
            RenderWindow window = new RenderWindow(mode, "Chess", Styles.Titlebar | Styles.Close);

            window.KeyPressed += OnKeyPressed;
            window.Closed += OnClosed;
            window.MouseButtonPressed += OnMouseClick;

            Vector2u windowSize = window.Size;
            float vCenterX = (8 * TextureSize) / 2;
            float vCenterY = (8 * TextureSize) / 2;

            View GameView = new View(new Vector2f(vCenterX, vCenterY), new Vector2f(8, 8));
            window.SetView(GameView);
            window.SetVerticalSyncEnabled(true);

            //mouseX = (int)pos.X / TextureSize;
            //mouseY = (int)pos.Y / TextureSize;

            while (window.IsOpen)
            {
                deltaTime = clock.Restart().AsSeconds();
                window.Clear(new Color(18, 77, 122));
                window.SetView(window.DefaultView);

                Vector2i mPosWindow = Mouse.GetPosition(window);
                Vector2f pos = window.MapPixelToCoords(mPosWindow);
                mouseX = mPosWindow.X / TextureSize;
                mouseY = mPosWindow.Y / TextureSize;

                board.DrawBoard(ref window);
                window.Display();

                window.DispatchEvents();
            }
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
                Square squareClicked = board.SqaureAt(mouseX, mouseY);
                if (squareClicked != null && squareClicked.hasPiece() && squareClicked.getPiece().pieceColor == CurrentColor)
                    board.SelectSquare(mouseX, mouseY);
                else
                    board.Deselect();

                Console.Out.Write(mouseX + " ");
                Console.Out.Write(mouseY + "\n");

            }
        }


    }
}
