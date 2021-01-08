using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexGame
{
    public class Hex
    {
        //https://www.redblobgames.com/grids/hexagons/
        // Q + R + S = 0
        // S = -(Q + R)

        public int Q { get; private set; } //column
        public int R { get; private set; } //row
        public int S { get; private set; }
        public Model @Model { get; private set; }
        public Vector3 ColorVector { get; set; }
        private static float WidthMultiplier = (float)Math.Sqrt(3) / 2;
        private Matrix View = Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), Vector3.UnitY);
        private Matrix Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 100f);

        public Hex(int q, int r, Model model)
        {
            this.Q = q;
            this.R = r;
            this.S = -(q + r);
            this.Model = model;
            this.ColorVector = Color.Green.ToVector3();
        }

        public Hex(int q, int r, Model model, Vector3 color)
        {
            this.Q = q;
            this.R = r;
            this.S = -(q + r);
            this.Model = model;
            this.ColorVector = color;
        }

        public Vector3 GetPosition()
        {
            var radius = 1f;
            var height = radius * 2;
            var width = WidthMultiplier * height;
            var horizantalSpacing = width;
            var verticalSpacing = height * 0.75f;

            return new Vector3(horizantalSpacing * (Q + R / 2f), 0, verticalSpacing * R);
        }

        public void Draw(GameTime gameTime, GraphicsDevice graphicsDevice, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in this.Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.DiffuseColor = ColorVector;
                    effect.AmbientLightColor = ColorVector;
                    effect.View = view;
                    effect.World = Matrix.CreateWorld(GetPosition(), Vector3.Forward, Vector3.Up);
                    effect.Projection = projection;
                }
                mesh.Draw();
            }
        }
    }
}