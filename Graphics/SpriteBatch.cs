using System;
using System.Text;

namespace engenious.Graphics
{
    /// <summary>
    /// For drawing sprites in batches.
    /// </summary>
    public class SpriteBatch : GraphicsResource
    {
        /// <summary>
        /// Specifies sorting modes for the batching process.
        /// </summary>
        public enum SpriteSortMode
        {
            /// <summary>
            /// No sorting or batching applied and draws them exactly how they are put.
            /// </summary>
            Immediate,
            /// <summary>
            /// Sorts elements from front to back; batches like <see cref="Deffered"/>.
            /// </summary>
            FrontToBack,
            /// <summary>
            /// Sorts elements from back to front; batches like <see cref="Deffered"/>.
            /// </summary>
            BackToFront,
            /// <summary>
            /// No sorting applied but batched to be drawn at the <see cref="SpriteBatch.End"/> call.
            /// </summary>
            Deffered,
            /// <summary>
            /// Sorts elements by texture; batches same textures together.
            /// </summary>
            Texture
        }

        /// <summary>
        /// Specifies simple sprite effects.
        /// </summary>
        [Flags]
        public enum SpriteEffects
        {
            /// <summary>
            /// No effect applied to the sprite.
            /// </summary>
            None,
            /// <summary>
            /// Flips the sprite horizontally.
            /// </summary>
            FlipHorizontally = 1,
            /// <summary>
            /// Flips the sprite vertically.
            /// </summary>
            FlipVertically = 2
        }


        private Matrix _matrix;
        private SpriteSortMode _sortMode;
        private BlendState _blendState;
        private SamplerState _samplerState;
        private DepthStencilState _depthStencilState;
        private RasterizerState _rasterizerState;
        private IModelEffect _effect;
        private readonly BasicEffect _defaultEffect;
        private readonly SpriteBatcher _batcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteBatch"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/>.</param>
        public SpriteBatch(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            _batcher = new SpriteBatcher(graphicsDevice);

            var effect = new BasicEffect(graphicsDevice);
            effect.TextureEnabled = true;
            effect.VertexColorEnabled = true;
            effect.Parameters["View"].SetValue(Matrix.Identity);
            effect.Parameters["Proj"].SetValue(Matrix.Identity);

            _defaultEffect = effect;
            _effect = effect;
        }

        /// <summary>
        /// Begins batching of following draw calls to this <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="sortMode">The way the draw calls are to be sorted and batched by.</param>
        /// <param name="blendState">The blend state to use for drawing.</param>
        /// <param name="samplerState">The sampler state to use for drawing.</param>
        /// <param name="depthStencilState">The depth stencil state to use for drawing.</param>
        /// <param name="rasterizerState">The rasterizer state to use for drawing.</param>
        /// <param name="effect">A custom effect to use for rendering the sprites.</param>
        /// <param name="transformMatrix">A transformation to apply to all sprites.</param>
        public void Begin(SpriteSortMode sortMode = SpriteSortMode.Deffered, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, IModelEffect effect = null, Matrix? transformMatrix = null)
        {
            _sortMode = sortMode;
            _blendState = blendState;
            _samplerState = samplerState;
            if (depthStencilState == null)
                _depthStencilState = DepthStencilState.None;
            else
                _depthStencilState = depthStencilState;
            _rasterizerState = rasterizerState;
            //TODO: this.effect = effect;
            if (transformMatrix.HasValue)
                _matrix = transformMatrix.Value;
            else
                _matrix = Matrix.Identity;

            _effect = effect ?? _defaultEffect;

            _batcher.Begin(sortMode);
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
        {
            Draw(texture, destinationRectangle, null, color);
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2D texture, RectangleF destinationRectangle, Color color)
        {
            Draw(texture, destinationRectangle, null, color);
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            Draw(texture, new Vector2(destinationRectangle.X, destinationRectangle.Y), sourceRectangle, color, 0f, new Vector2(), new Vector2(destinationRectangle.Width, destinationRectangle.Height), SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2D texture, RectangleF destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            Draw(texture, new Vector2(destinationRectangle.X, destinationRectangle.Y), sourceRectangle, color, 0f, new Vector2(), new Vector2(destinationRectangle.Width, destinationRectangle.Height), SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2D texture, Vector2 position, Color color)
        {
            Draw(texture, position, null, color);
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            Draw(texture, position, sourceRectangle, color, 0, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None);//TODO?
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        /// <param name="rotation">The rotation of the sprite in radians.</param>
        /// <param name="origin">The point of origin for the sprites rotation.</param>
        /// <param name="scale">The scale to draw the texture with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth = 0.0f)
        {
            Draw(texture, position, sourceRectangle, color, rotation, origin, new Vector2(scale * texture.Width, scale * texture.Height), effects, layerDepth);//TODO?
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        /// <param name="rotation">The rotation of the sprite in radians.</param>
        /// <param name="origin">The point of origin for the sprites rotation.</param>
        /// <param name="size">The size to draw the texture with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 size, SpriteEffects effects, float layerDepth)
        {
            _batcher.AddBatch(SpriteBatcher.BatchPool.AquireBatch(texture, position, sourceRectangle, color, rotation, origin, size, effects, layerDepth, _sortMode));
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2D texture, Rectangle destinationRectangle, RectangleF sourceRectangle, Color color)
        {
            //TODO
            Draw(texture, new Vector2(destinationRectangle.X, destinationRectangle.Y), sourceRectangle, color, 0f, new Vector2(), new Vector2(destinationRectangle.Width, destinationRectangle.Height), SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2D texture, RectangleF destinationRectangle, RectangleF sourceRectangle, Color color)
        {
            //TODO
            Draw(texture, new Vector2(destinationRectangle.X, destinationRectangle.Y), sourceRectangle, color, 0f, new Vector2(), new Vector2(destinationRectangle.Width, destinationRectangle.Height), SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2D texture, Vector2 position, RectangleF sourceRectangle, Color color)
        {
            Draw(texture, position, sourceRectangle, color, 0, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None);//TODO?
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        /// <param name="rotation">The rotation of the sprite in radians.</param>
        /// <param name="origin">The point of origin for the sprites rotation.</param>
        /// <param name="scale">The scale to draw the texture with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        public void Draw(Texture2D texture, Vector2 position, RectangleF sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth = 0.0f)
        {
            Draw(texture, position, sourceRectangle, color, rotation, origin, new Vector2(scale * texture.Width, scale * texture.Height), effects, layerDepth);//TODO?
        }
        
        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        /// <param name="rotation">The rotation of the sprite in radians.</param>
        /// <param name="origin">The point of origin for the sprites rotation.</param>
        /// <param name="size">The size to draw the texture with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        public void Draw(Texture2D texture, Vector2 position, RectangleF sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 size, SpriteEffects effects, float layerDepth)
        {
            _batcher.AddBatch(SpriteBatcher.BatchPool.AquireBatch(texture, position, sourceRectangle, color, rotation, origin, size, effects, layerDepth, _sortMode));
        }

        /// <summary>
        /// Draws text using a given <see cref="SpriteFont"/>.
        /// </summary>
        /// <param name="spriteFont">The sprite font to use to draw the characters.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The position to draw the text at.</param>
        /// <param name="color">The color to draw the text with.</param>
        /// <param name="rotation">The rotation of the text in radians.</param>
        /// <param name="origin">The origin point to rotate the text at.</param>
        /// <param name="scale">The scale to draw the text with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation = 0.0f, Vector2 origin = new Vector2(), float scale = 1.0f, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0.0f)
        {
            DrawString(spriteFont, text, position, color, rotation, origin, new Vector2(scale, scale), effects, layerDepth);
        }

        /// <summary>
        /// Draws text using a given <see cref="SpriteFont"/>.
        /// </summary>
        /// <param name="spriteFont">The sprite font to use to draw the characters.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The position to draw the text at.</param>
        /// <param name="color">The color to draw the text with.</param>
        /// <param name="rotation">The rotation of the text in radians.</param>
        /// <param name="origin">The origin point to rotate the text at.</param>
        /// <param name="scale">The scale to draw the text with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0.0f)
        {
            var offset = new Vector2(0.0f, 0.0f);
            for (var i = 0; i < text.Length; i++)
            {

                var c = text[i];
                if (!spriteFont.CharacterMap.TryGetValue(c, out var fontChar))
                {
                    if (c == '\n')
                    {
                        offset.X = 0;
                        offset.Y += spriteFont.LineSpacing;
                        continue;
                    }
                    if (!spriteFont.DefaultCharacter.HasValue || !spriteFont.CharacterMap.TryGetValue(spriteFont.DefaultCharacter.Value, out fontChar))
                    {
                        continue;
                    }
                }
                if (fontChar == null)
                    continue;
                
                Draw(spriteFont.Texture, position + offset + fontChar.Offset, fontChar.TextureRegion, color, rotation, origin - offset, 1.0f, SpriteEffects.None, layerDepth);
                offset.X += fontChar.Advance;
                if (i < text.Length - 1)
                {
                    if (spriteFont.Kernings.TryGetValue(SpriteFont.GetKerningKey(c, text[i + 1]), out var kerning))
                        offset.X += kerning;
                }
            }
        }

        /// <summary>
        /// Draws text using a given <see cref="SpriteFont"/>.
        /// </summary>
        /// <param name="spriteFont">The sprite font to use to draw the characters.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="startIndex">The index to start drawing the text from.</param>
        /// <param name="length">The length of characters to draw from the text.</param>
        /// <param name="position">The position to draw the text at.</param>
        /// <param name="color">The color to draw the text with.</param>
        /// <param name="rotation">The rotation of the text in radians.</param>
        /// <param name="origin">The origin point to rotate the text at.</param>
        /// <param name="scale">The scale to draw the text with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        public void DrawString(SpriteFont spriteFont, string text, int startIndex, int length, Vector2 position, Color color, float rotation = 0.0f, Vector2 origin = new Vector2(), float scale = 1.0f, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0.0f)
        {
            DrawString(spriteFont, text, startIndex, length, position, color, rotation, origin, new Vector2(scale, scale), effects, layerDepth);
        }

        /// <summary>
        /// Draws text using a given <see cref="SpriteFont"/>.
        /// </summary>
        /// <param name="spriteFont">The sprite font to use to draw the characters.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The position to draw the text at.</param>
        /// <param name="color">The color to draw the text with.</param>
        /// <param name="rotation">The rotation of the text in radians.</param>
        /// <param name="origin">The origin point to rotate the text at.</param>
        /// <param name="scale">The scale to draw the text with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation = 0.0f, Vector2 origin = new Vector2(), float scale = 1.0f, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0.0f)
        {
            DrawString(spriteFont, text, position, color, rotation, origin, new Vector2(scale, scale), effects, layerDepth);
        }

        /// <summary>
        /// Draws text using a given <see cref="SpriteFont"/>.
        /// </summary>
        /// <param name="spriteFont">The sprite font to use to draw the characters.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The position to draw the text at.</param>
        /// <param name="color">The color to draw the text with.</param>
        /// <param name="rotation">The rotation of the text in radians.</param>
        /// <param name="origin">The origin point to rotate the text at.</param>
        /// <param name="scale">The scale to draw the text with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0.0f)
        {
            DrawString(spriteFont, text, 0, text.Length, position, color, rotation, origin, scale, effects, layerDepth);
        }

        /// <summary>
        /// Draws text using a given <see cref="SpriteFont"/>.
        /// </summary>
        /// <param name="spriteFont">The sprite font to use to draw the characters.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="startIndex">The index to start drawing the text from.</param>
        /// <param name="length">The length of characters to draw from the text.</param>
        /// <param name="position">The position to draw the text at.</param>
        /// <param name="color">The color to draw the text with.</param>
        /// <param name="rotation">The rotation of the text in radians.</param>
        /// <param name="origin">The origin point to rotate the text at.</param>
        /// <param name="scale">The scale to draw the text with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        public void DrawString(SpriteFont spriteFont, string text, int startIndex, int length, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0.0f)
        {
            var offset = new Vector2(0.0f, 0.0f);
            for (var i = startIndex; i < startIndex + length; i++)
            {

                var c = text[i];
                if (!spriteFont.CharacterMap.TryGetValue(c, out var fontChar))
                {
                    if (c == '\n')
                    {
                        offset.X = 0;
                        offset.Y += spriteFont.LineSpacing;
                        continue;
                    }
                    if (!spriteFont.DefaultCharacter.HasValue || !spriteFont.CharacterMap.TryGetValue(spriteFont.DefaultCharacter.Value, out fontChar))
                    {
                        continue;
                    }
                }
                if (fontChar == null)
                    continue;
                
                Draw(spriteFont.Texture, position + offset + fontChar.Offset, fontChar.TextureRegion, color, rotation, origin - offset, 1.0f, SpriteEffects.None, layerDepth);
                offset.X += fontChar.Advance;
                if (i < text.Length - 1)
                {
                    if (spriteFont.Kernings.TryGetValue(SpriteFont.GetKerningKey(c, text[i + 1]), out var kerning))
                        offset.X += kerning;
                }
            }
        }

        /// <summary>
        /// Ends the drawing process and batches all draws together to finally draw them.
        /// </summary>
        public void End()
        {
            GraphicsDevice.BlendState = _blendState;
            GraphicsDevice.RasterizerState = _rasterizerState;
            GraphicsDevice.DepthStencilState = _depthStencilState;
            _batcher.SamplerState = _samplerState;

            var projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, -1);

            _effect.World = projection * _matrix;
            
            _batcher.End(_effect);


        }

        /// <inheritdoc />
        public override void Dispose()
        {
            _batcher.Dispose();
            base.Dispose();
        }
    }
}

