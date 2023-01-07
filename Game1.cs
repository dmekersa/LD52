using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LD52
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private FontManager _fontManager;
        private SceneManager _sceneManager;
        static Rectangle CANVAS = new Rectangle(0, 0, 800 / 4, 480 / 4);
        private const int ScreenWidth = 800;
        private const int ScreenHeight = 480;
        private RenderTarget2D _renderTarget;
        private TileSets _tileSets;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            ServiceLocator.RegisterService(Content);

            Gamecodeur.GCControlManager controlManager = new Gamecodeur.GCControlManager();
            ServiceLocator.RegisterService<Gamecodeur.GCControlManager>(controlManager);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Window.AllowUserResizing = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ServiceLocator.RegisterService(_spriteBatch);

            // TODO: use this.Content to load your game content here
            _renderTarget = new RenderTarget2D(GraphicsDevice, CANVAS.Width, CANVAS.Height);

            _fontManager = new FontManager(Content);
            ServiceLocator.RegisterService(_fontManager);

            _tileSets = new TileSets();
            ServiceLocator.RegisterService<TileSets>(_tileSets);

            _sceneManager = new SceneManager();
            ServiceLocator.RegisterService((SceneService)_sceneManager);

            _sceneManager.ChangeScene(SceneManager.sceneType.Menu);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            _sceneManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(new Color(26, 28, 44));

            _spriteBatch.Begin();
            _sceneManager.Draw();
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            float ratio = 1;
            int marginV = 0;
            int marginH = 0;
            float currentAspect = Window.ClientBounds.Width / (float)Window.ClientBounds.Height;
            float virtualAspect = (float)CANVAS.Width / (float)CANVAS.Height;
            if (CANVAS.Height != this.Window.ClientBounds.Height)
            {
                if (currentAspect > virtualAspect)
                {
                    ratio = Window.ClientBounds.Height / (float)CANVAS.Height;
                    marginH = (int)((Window.ClientBounds.Width - CANVAS.Width * ratio) / 2);
                }
                else
                {
                    ratio = Window.ClientBounds.Width / (float)CANVAS.Width;
                    marginV = (int)((Window.ClientBounds.Height - CANVAS.Height * ratio) / 2);
                }
            }

            Rectangle dst = new Rectangle(marginH, marginV, (int)(CANVAS.Width * ratio), (int)(CANVAS.Height * ratio));

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            _spriteBatch.Draw(_renderTarget, dst, Color.White);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            _sceneManager.DrawUI();
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}