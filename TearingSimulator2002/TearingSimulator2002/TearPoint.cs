using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TearingSimulator2002
{
    class TearPoint
    {
        public Vector2 Position;
        public Texture2D Texture;
        public Vector2 scale = new Vector2(4.0f, 4.0f);
        public float rotation = 0f;

        public float Progress = 0;
        public bool IsDying = false;

        public TearPoint(Vector2 position, Texture2D texture)
        {
            Position = position;
            Texture = texture;
        }

        public TearPoint(Vector2 position, Texture2D texture, Vector2 scale, float rotation)
        {
            Position = position;
            Texture = texture;
            this.scale = scale;
            this.rotation = rotation;
        }

        public void Update(GameTime gameTime)
        {
            if (IsDying == false)
            {
                Progress += (float) gameTime.ElapsedGameTime.TotalSeconds * 8;
                if (Progress >= 1.0)
                {
                    IsDying = true;
                }
            }
            else
                Progress -= (float) gameTime.ElapsedGameTime.TotalSeconds / 2;
        }       

        public void Draw(SpriteBatch spriteBatch)
        {
            Effect effect = DrawManager.LoadEffect("TearingShader.mgfx");


            DrawManager.SetEffect(effect);
            spriteBatch.Draw(Texture, Position, null, Color.White, rotation, new Vector2(Texture.Height / 2f, Texture.Width / 2f), scale, SpriteEffects.None, 0f);
            effect.Parameters["amplification"].SetValue(Progress);
        }
    }
}
