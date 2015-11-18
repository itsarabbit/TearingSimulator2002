using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TearingSimulator2002
{
    static class NoiseManager
    {
        static List<Texture2D> noiseTextures = new List<Texture2D>();
        static int nextTexture;
        const int textureNumbers = 32;
        const int textureSize = 32;
        static GraphicsDevice device;
        
        public static void Initialize(GraphicsDevice device)
        {
            NoiseManager.device = device;
            GenerateTextures(textureNumbers);
        }

        public static Texture2D GetNoise()
        {
            if (nextTexture >= textureNumbers)
            {
                nextTexture = 0;
            }
            return noiseTextures[nextTexture++];
        }

        public static void GenerateTextures(int numberOfTextures)
        {
            for (int i = 0; i < numberOfTextures; i++)
            {
                Texture2D noiseTexture = new Texture2D(device, 32, 32);
                GenerateNoiseMap(textureSize, textureSize, ref noiseTexture, 12);
                noiseTextures.Add(noiseTexture);
            }
        }

        static void GenerateNoiseMap(int width, int height, ref Texture2D noiseTexture, int octaves)
        {
            var data = new float[width*height];

            /// track min and max noise value. Used to normalize the result to the 0 to 1.0 range.
            var min = float.MaxValue;
            var max = float.MinValue;

            /// rebuild the permutation table to get a different noise pattern. 
            /// Leave this out if you want to play with changing the number of octaves while 
            /// maintaining the same overall pattern.
            Noise2d.Reseed();

            var frequency = 10.0f;
            var amplitude = 2.0f;

            for (var octave = 0; octave < octaves; octave++)
            {
                /// parallel loop - easy and fast.
                Parallel.For(0
                    , width * height
                    , (offset) =>
                    {
                        var i = offset % width;
                        var j = offset / width;
                        var noise = Noise2d.Noise(i * frequency * 1f / width, j * frequency * 1f / height);
                        noise = data[j * width + i] += noise * amplitude;

                        min = Math.Min(min, noise);
                        max = Math.Max(max, noise);

                    }
                );

                frequency *= 2;
                amplitude /= 2;
            }


            if (noiseTexture != null && (noiseTexture.Width != width || noiseTexture.Height != height))
            {
                noiseTexture.Dispose();
                noiseTexture = null;
            }
            if (noiseTexture == null)
            {
                noiseTexture = new Texture2D(device, width, height, false, SurfaceFormat.Color);
            }

            var colors = data.Select(
                (f) =>
                {
                    var norm = (f - min) / (max - min);
                    return new Color(norm, norm, norm, 1);
                }
            ).ToArray();

            noiseTexture.SetData(colors);
        }
    }
}
