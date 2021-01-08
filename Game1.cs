using System;
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

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            cam = new Camera(GraphicsDevice, this.Window);
            cam.Position = new Vector3(0, 0, 10);
            cam.LookAtDirection = Vector3.Forward;

            model = Content.Load<Model>(@"Models/hex");
            font = Content.Load<SpriteFont>(@"Fonts/BrightStar");

            for (var colummn = 0; colummn < 10; colummn++)
            {
                for (var row = 0; row < 10; row++)
                {
                    var rnd = new Random();
                    var hex = new Hex(colummn, row, model, new Vector3(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255)));
                    hexes.Add(hex);
                }
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
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
            _spriteBatch.Begin();

            foreach (var hex in hexes)
            {
                hex.Draw(gameTime, GraphicsDevice, cam.View, cam.Projection);
            }

            _spriteBatch.DrawString(font, cam.View.Translation.ToString(), new Vector2(100, 100), Color.Black);
            _spriteBatch.DrawString(font, cam.Projection.Translation.ToString(), new Vector2(100, 200), Color.Black);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}