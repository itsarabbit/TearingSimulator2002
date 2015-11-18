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

        public float Progress = 0;
        public bool IsDying = false;

        public TearPoint(Vector2 position, Texture2D texture)
        {
            Position = position;
            Texture = texture;
        }

        public void Update(GameTime gameTime)
        {
            if (IsDying == false)
            {
                Progress += (float) gameTime.ElapsedGameTime.TotalSeconds;
                if (Progress >= 1.0)
                {
                    IsDying = true;
                }
            }
            else
                Progress -= (float) gameTime.ElapsedGameTime.TotalSeconds;
        }       

        public void Draw(SpriteBatch spriteBatch)
        {
            Effect effect = DrawManager.LoadEffect("TearingShader.mgfx");
            effect.Parameters["amplification"].SetValue(Progress);

            DrawManager.SetEffect(effect);
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
