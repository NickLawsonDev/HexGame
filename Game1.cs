using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HexGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Model model;
        private Camera cam;
        private List<Hex> hexes = new List<Hex>();
        private SpriteFont font;
        private BasicEffect basicEffect;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            cam = new Camera(GraphicsDevice, this.Window);
            cam.Position = new Vector3(0, 0, 10);
            cam.LookAtDirection = Vector3.Forward;

            model = Content.Load<Model>(@"Models/hex");
            font = Content.Load<SpriteFont>(@"Fonts/BrightStar");

            for (var colummn = 0; colummn < 10; colummn++)
            {
                for (var row = 0; row < 10; row++)
                {
                    var hex = new Hex(colummn, row, model);
                    hexes.Add(hex);
                }
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            basicEffect = new BasicEffect(GraphicsDevice)
            {
                TextureEnabled = true,
                VertexColorEnabled = true,
            };
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                    Keys.Escape))
                Exit();

            cam.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Matrix invertY = Matrix.CreateScale(1, -1, 1);

            basicEffect.World = invertY;
            basicEffect.View = Matrix.Identity;
            basicEffect.Projection = cam.Projection;

            foreach (var hex in hexes)
            {
                Vector3 viewSpaceTextPosition = Vector3.Transform(hex.GetPosition(), cam.View * invertY);
                hex.Draw(gameTime, GraphicsDevice, cam.View, cam.Projection);
            _spriteBatch.Begin(0, null, null, DepthStencilState.DepthRead, RasterizerState.CullNone, basicEffect);
                _spriteBatch.DrawString(font, $"({hex.Q}, {hex.R})", new Vector2(viewSpaceTextPosition.X, viewSpaceTextPosition.Y), Color.Black, 0, font.MeasureString($"({hex.Q}, {hex.R})") / 2, 0.025f, 0, viewSpaceTextPosition.Z);
            _spriteBatch.End();
            }

            var h = new Hex(100, 100, model);
            h.Draw(gameTime, GraphicsDevice, cam.View, cam.Projection);
            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, $"Mouse X: {Mouse.GetState().X}", new Vector2(5, 5), Color.Black);
            _spriteBatch.DrawString(font, $"Mouse Y: {Mouse.GetState().Y}", new Vector2(5, 22), Color.Black);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}