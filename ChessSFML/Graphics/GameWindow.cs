using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using SFML.Audio;
using SFML.Window;
using System.IO;

namespace ChessSFML
{
    public class GameWindow
    {
        //const string FONT_NAME = "";
        const string BoardFilePath = "C:\\Users\\Jamison\\Google Drive\\Programming Projects\\Chess\\ChessSFML\\ChessSFML\\res\\board.png";
        const string PiecesFilePath = "C:\\Users\\Jamison\\Google Drive\\Programming Projects\\Chess\\ChessSFML\\ChessSFML\\res\\pieces.png";
        const int RawTextureSize = 32;
        const int TextureScale = 4;
        const int TextureSize = RawTextureSize * TextureScale;

        static readonly int[] BoardMap = 
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

        static readonly int[] PieceMap = 
        {
            1, 2, 3, 5, 4, 3, 2, 1,
            0, 0, 0, 0, 0, 0, 0, 0,
        };


        public void Show()
        {
            float deltaTime = 0.00f;
            Clock clock = new Clock();

            Texture BoardTexture = new Texture(BoardFilePath); 
            Sprite BoardSprite = new Sprite(BoardTexture);
            BoardSprite.Scale = new Vector2f(TextureScale, TextureScale);

            Texture PieceTexture = new Texture(PiecesFilePath);
            Sprite PiecesSprite = new Sprite(PieceTexture);
            PiecesSprite.Scale = new Vector2f(TextureScale, TextureScale);

            VideoMode mode = new VideoMode(1024, 1024);
            RenderWindow window = new RenderWindow(mode, "Chess", Styles.Titlebar | Styles.Close);

            window.KeyPressed += OnKeyPressed;
            window.Closed += OnClosed;

            Vector2u windowSize = window.Size;
            float vCenterX = (8 * TextureSize) / 2;
            float vCenterY = (8 * TextureSize) / 2;

            View GameView = new View(new Vector2f(vCenterX, vCenterY), new Vector2f(8, 8));
            window.SetView(GameView);
            window.SetVerticalSyncEnabled(true);

            while(window.IsOpen)
            {
                deltaTime = clock.Restart().AsSeconds();
                window.Clear(new Color(18, 77, 122));
                window.SetView(window.DefaultView);

                drawBoard(ref window, BoardSprite);
                drawPieces(ref window, PiecesSprite);
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

        public void drawBoard(ref RenderWindow window, Sprite sprite)
        {
            int z = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    sprite.TextureRect = new IntRect(BoardMap[z++] * RawTextureSize, 0, RawTextureSize, RawTextureSize);
                    sprite.Position = new Vector2f(i * TextureSize,j * TextureSize);
                    window.Draw(sprite);
                }
            }
        }

        public void drawPieces(ref RenderWindow window, Sprite sprite)
        {
            int z = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    sprite.TextureRect = new IntRect(PieceMap[z++] * RawTextureSize, 0, RawTextureSize, RawTextureSize);
                    sprite.Position = new Vector2f(j * TextureSize, i * TextureSize);
                    window.Draw(sprite);
                }
            }
            for (int i = 6; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    sprite.TextureRect = new IntRect(PieceMap[--z] * RawTextureSize, 1 * RawTextureSize, RawTextureSize, RawTextureSize);
                    sprite.Position = new Vector2f(j * TextureSize, i * TextureSize);
                    window.Draw(sprite);
                }
            }
        }

        
    }


}
