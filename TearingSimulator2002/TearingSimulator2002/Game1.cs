using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TearingSimulator2002
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D noiseMap;
        Vector2 previousMousePosition, mousePosition, resolution;
        List<TearPoint> tearList = new List<TearPoint>(); 
        private float timer = 1.0f;

        Effect shader;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            NoiseManager.Initialize(GraphicsDevice);
            shader = Content.Load<Effect>("TearingShader.mgfx");
            noiseMap = NoiseManager.GetNoise();
            DrawManager.Initialize(spriteBatch, Content);
            resolution.X = GraphicsDevice.Viewport.Width;
            resolution.Y = GraphicsDevice.Viewport.Height;
        }

        protected override void Update(GameTime gameTime)
        {
            previousMousePosition = mousePosition;
            mousePosition = Mouse.GetState().Position.ToVector2();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer <= 0)
            {
                noiseMap = NoiseManager.GetNoise();
                timer = 1.0f;
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                float distance = (float)Math.Sqrt(((mousePosition.X - previousMousePosition.X) * (mousePosition.X - previousMousePosition.X)) + ((mousePosition.Y - previousMousePosition.Y) * (mousePosition.Y - previousMousePosition.Y)));
                for(int i = 1; i < distance; i+=2)
                {
                    Vector2 position = previousMousePosition +
                                       (Vector2.Normalize(new Vector2((mousePosition.X - previousMousePosition.X),
                                           (mousePosition.Y - previousMousePosition.Y)))*i);
                    float angle =
                        (float)Math.Atan2(position.Y - resolution.Y / 2f, position.X - resolution.X / 2f);
                    tearList.Add(new TearPoint(position, NoiseManager.GetNoise(),
                        new Vector2(1.0f, 2.0f), angle));
                }
            }

            for (int i = 0; i < tearList.Count; i++ )
            {
                tearList[i].Update(gameTime);
                if (tearList[i].IsDying == true && tearList[i].Progress <= 0)
                {
                    tearList.RemoveAt(i);
                    i--;
                }
            }

            Debug.WriteLine(1.0/gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            DrawManager.SetEffect("TearingShader.mgfx");
            Effect effect = DrawManager.LoadEffect("TearingShader.mgfx");
            float amplitude = (float)((Math.Sin(gameTime.TotalGameTime.TotalSeconds * 4) + 1.0f) / 2.0f);
            effect.Parameters["amplification"].SetValue(amplitude);
            DrawManager.SetEffect(effect);
            spriteBatch.Draw(noiseMap, Vector2.Zero, null, Color.White);


            foreach (TearPoint tearPoint in tearList)
            {
                tearPoint.Draw(spriteBatch);
            }
            DrawManager.SetEffect((Effect)null);
            spriteBatch.Draw(noiseMap, new Vector2(32f, 0f), null, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
