using System.Net.Mime;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace TearingSimulator2002
{
    public static class DrawManager
    {
        public static List<Effect> EffectList = new List<Effect>();
        public static SpriteBatch SpriteBatch;
        public static ContentManager Content;

        public static void Initialize(SpriteBatch spriteBatch, ContentManager content)
        {
            SpriteBatch = spriteBatch;
            Content = content;
        }

        public static void AddEffect(Effect effect)
        {
            EffectList.Add(effect);
        }

        public static Effect LoadEffect(string effectName)
        {
            foreach (Effect effect in EffectList)
            {
                if (effect.Name == effectName)
                {
                    return effect;
                }
            }
            Effect tempEffect = Content.Load<Effect>(effectName);
            tempEffect.Name = effectName;
            AddEffect(tempEffect);
            return tempEffect;
        }

        public static void SetEffect(Effect effect)
        {
            SpriteBatch.End();
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, effect, null);
        }

        public static void SetEffect(string effectName)
        {
            SetEffect(LoadEffect(effectName));
        }
    }
}
