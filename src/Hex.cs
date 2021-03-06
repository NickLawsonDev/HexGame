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
        private static float WidthMultiplier = (float)Math.Sqrt(3) / 2;

        public Hex(int q, int r, Model model)
        {
            this.Q = q;
            this.R = r;
            this.S = -(q + r);
            this.Model = model;
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
                    effect.View = view;
                    effect.Projection = projection;
                    effect.World = Matrix.CreateWorld(GetPosition(), Vector3.Forward, Vector3.Up);
                }
                mesh.Draw();
            }
        }
    }
}