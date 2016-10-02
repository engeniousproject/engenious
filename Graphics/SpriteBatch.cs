using System;
using OpenTK;

namespace engenious.Graphics
{
    public class SpriteBatch : GraphicsResource
    {
        public enum SpriteSortMode
        {
            Immediate,
            FrontToBack,
            BackToFront,
            Deffered,
            Texture
        }

        [FlagsAttribute]
        public enum SpriteEffects
        {
            None,
            FlipHorizontally = 1,
            FlipVertically = 2
        }


        private Matrix matrix;
        private SpriteSortMode sortMode;
        private BlendState blendState;
        private SamplerState samplerState;
        private DepthStencilState depthStencilState;
        private RasterizerState rasterizerState;
        private Effect effect;
        private SpriteBatcher batcher;
        private EffectParameter worldViewProj;

        public SpriteBatch(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            batcher = new SpriteBatcher(graphicsDevice);

            var effect = new BasicEffect(graphicsDevice);
            effect.TextureEnabled = true;
            effect.VertexColorEnabled = true;
            effect.Parameters["View"].SetValue(Matrix.Identity);
            effect.Parameters["Proj"].SetValue(Matrix.Identity);

            this.effect = effect;
        }


        public void Begin(SpriteSortMode sortMode = SpriteSortMode.Deffered, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, Effect effect = null, Matrix? transformMatrix = null)
        {
            this.sortMode = sortMode;
            this.blendState = blendState;
            this.samplerState = samplerState;
            if (depthStencilState == null)
                this.depthStencilState = DepthStencilState.None;
            else
                this.depthStencilState = depthStencilState;
            this.rasterizerState = rasterizerState;
            //TODO: this.effect = effect;
            if (transformMatrix.HasValue)
                this.matrix = transformMatrix.Value;
            else
                this.matrix = Matrix.Identity;
            worldViewProj = this.effect.Parameters["World"];
            batcher.Begin(sortMode);
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
        {
            Draw(texture, destinationRectangle, null, color);
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Nullable<Rectangle> sourceRectangle, Color color)
        {
            //TODO
            Draw(texture, new Vector2(destinationRectangle.X, destinationRectangle.Y), sourceRectangle, color, 0f, new Vector2(), new Vector2(destinationRectangle.Width, destinationRectangle.Height), SpriteEffects.None, 0.0f);
        }

        public void Draw(Texture2D texture, Vector2 position, Color color)
        {
            Draw(texture, position, null, color);
        }

        public void Draw(Texture2D texture, Vector2 position, Nullable<Rectangle> sourceRectangle, Color color)
        {
            Draw(texture, position, sourceRectangle, color, 0, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0.0f);//TODO?
        }

        public void Draw(Texture2D texture, Vector2 position, Nullable<Rectangle> sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth = 0.0f)
        {
            Draw(texture, position, sourceRectangle, color, rotation, origin, new Vector2(scale * texture.Width, scale * texture.Height), effects, layerDepth);//TODO?
        }

        public void Draw(Texture2D texture, Vector2 position, Nullable<Rectangle> sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 size, SpriteEffects effects, float layerDepth)
        {
            batcher.Batches.Add(new SpriteBatcher.BatchItem(texture, position, sourceRectangle, color, rotation, origin, size, effects, layerDepth, sortMode));
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, RectangleF sourceRectangle, Color color)
        {
            //TODO
            Draw(texture, new Vector2(destinationRectangle.X, destinationRectangle.Y), sourceRectangle, color, 0f, new Vector2(), new Vector2(destinationRectangle.Width, destinationRectangle.Height), SpriteEffects.None, 0.0f);
        }

        public void Draw(Texture2D texture, Vector2 position, RectangleF sourceRectangle, Color color)
        {
            Draw(texture, position, sourceRectangle, color, 0, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0.0f);//TODO?
        }

        public void Draw(Texture2D texture, Vector2 position, RectangleF sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth = 0.0f)
        {
            Draw(texture, position, sourceRectangle, color, rotation, origin, new Vector2(scale * texture.Width, scale * texture.Height), effects, layerDepth);//TODO?
        }

        public void Draw(Texture2D texture, Vector2 position, RectangleF sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 size, SpriteEffects effects, float layerDepth)
        {
            batcher.Batches.Add(new SpriteBatcher.BatchItem(texture, position, sourceRectangle, color, rotation, origin, size, effects, layerDepth, sortMode));
        }


        public void DrawString(SpriteFont spriteFont, System.Text.StringBuilder text, Vector2 position, Color color)
        {
            DrawString(spriteFont, text, position, color, 0.0f, new Vector2(), 1.0f);
        }

        public void DrawString(SpriteFont spriteFont, System.Text.StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0.0f)
        {
            DrawString(spriteFont, text, position, color, rotation, origin, new Vector2(scale, scale), effects, layerDepth);
        }

        public void DrawString(SpriteFont spriteFont, System.Text.StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0.0f)
        {
            DrawString(spriteFont, text.ToString(), position, color, rotation, origin, scale, effects, layerDepth);
        }

        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            DrawString(spriteFont, text, position, color, 0.0f, new Vector2(), 1.0f);
        }

        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0.0f)
        {
            DrawString(spriteFont, text, position, color, rotation, origin, new Vector2(scale, scale), effects, layerDepth);
        }


        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0.0f)
        {
            Vector2 offset = new Vector2(0.0f, 0.0f);
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                FontCharacter fontChar;
                if (!spriteFont.characterMap.TryGetValue(c, out fontChar))
                {
                    if (c == '\n')
                    {
                        offset.X = 0;
                        offset.Y += spriteFont.LineSpacing;
                        continue;
                    }
                    if (!spriteFont.DefaultCharacter.HasValue || !spriteFont.characterMap.TryGetValue(spriteFont.DefaultCharacter.Value, out fontChar))
                    {
                        continue;
                    }
                }
                if (fontChar == null)
                    continue;
                
                Draw(spriteFont.texture, position + offset - new Vector2(-fontChar.Offset.X,fontChar.Offset.Y-spriteFont.BaseLine), fontChar.TextureRegion, color, rotation, origin - offset, 1.0f, SpriteEffects.None, layerDepth);
                offset.X += fontChar.Advance;
                if (i < text.Length - 1)
                {
                    int kerning = 0;
                    if (spriteFont.kernings.TryGetValue(SpriteFont.getKerningKey(c, text[i + 1]), out kerning))
                        offset.X += kerning;
                }
            }
        }

        public void End()
        {
            GraphicsDevice.BlendState = blendState;
            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.DepthStencilState = depthStencilState;
            batcher.samplerState = samplerState;

            Matrix projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, -1);

            worldViewProj.SetValue(projection * matrix);
            batcher.End(effect);


        }

        public override void Dispose()
        {
            batcher.Dispose();
            base.Dispose();
        }
    }
}

