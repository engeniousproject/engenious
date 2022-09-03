using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        private BlendState? _blendState;
        private SamplerState? _samplerState;
        private DepthStencilState? _depthStencilState;
        private RasterizerState? _rasterizerState;
        private IModelEffect? _effect;
        private bool _useScreenSpace;
        private readonly BasicEffect _defaultEffect;
        private readonly MsdfEffect _fontEffect;
        private readonly SpriteBatcher _batcher;
        
        private IModelTechnique GetCurrentTechnique(bool asArray)
        {
            if (_effect is not null)
                return (IModelTechnique)_effect.CurrentTechnique!;
            return asArray ? _defaultEffect.MainTechniqueArray : _defaultEffect.MainTechnique;
        }

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
            
            var fontEffect = new MsdfEffect(graphicsDevice);
            fontEffect.View = Matrix.Identity;
            fontEffect.Projection = Matrix.Identity;
            _fontEffect = fontEffect;
            
            _effect = effect;
        }
        
        /// <inheritdoc cref="GraphicsResource.GraphicsDevice"/>
        public new GraphicsDevice GraphicsDevice => base.GraphicsDevice!;
        
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
        /// <param name="useScreenSpace">A value indicating whether the sprites should be rendered in screenspace.</param>
        public void Begin(SpriteSortMode sortMode = SpriteSortMode.Deffered, BlendState? blendState = null, SamplerState? samplerState = null, DepthStencilState? depthStencilState = null, RasterizerState? rasterizerState = null, IModelEffect? effect = null, Matrix? transformMatrix = null, bool useScreenSpace = true)
        {
            _sortMode = sortMode;
            _blendState = blendState;
            _samplerState = samplerState;
            _depthStencilState = depthStencilState ?? DepthStencilState.None;
            _rasterizerState = rasterizerState;

            _matrix = transformMatrix ?? Matrix.Identity;

            _effect = effect;

            _useScreenSpace = useScreenSpace;

            _batcher.Begin(sortMode);
        }
#region Texture2DArrayEffects

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2DArray texture, uint textureIndex, Rectangle destinationRectangle, Color color)
        {
            Draw(effectTechnique, texture, textureIndex, destinationRectangle, null, color);
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2DArray texture, uint textureIndex, RectangleF destinationRectangle, Color color)
        {
            Draw(effectTechnique, texture, textureIndex, destinationRectangle, null, color);
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2DArray texture, uint textureIndex, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            Draw(effectTechnique, texture, textureIndex, new Vector2(destinationRectangle.X, destinationRectangle.Y), sourceRectangle, color, 0f, new Vector2(), new Vector2(destinationRectangle.Width, destinationRectangle.Height), SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2DArray texture, uint textureIndex, RectangleF destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            Draw(effectTechnique, texture, textureIndex, new Vector2(destinationRectangle.X, destinationRectangle.Y), sourceRectangle, color, 0f, new Vector2(), new Vector2(destinationRectangle.Width, destinationRectangle.Height), SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2DArray texture, uint textureIndex, Vector2 position, Color color)
        {
            Draw(effectTechnique, texture, textureIndex, position, null, color);
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2DArray texture, uint textureIndex, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            Draw(effectTechnique, texture, textureIndex, position, sourceRectangle, color, 0, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None);//TODO?
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        /// <param name="rotation">The rotation of the sprite in radians.</param>
        /// <param name="origin">The point of origin for the sprites rotation.</param>
        /// <param name="scale">The scale to draw the texture with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2DArray texture, uint textureIndex, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth = 0.0f)
        {
            Draw(effectTechnique, texture, textureIndex, position, sourceRectangle, color, rotation, origin, new Vector2(scale * texture.Width, scale * texture.Height), effects, layerDepth);//TODO?
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        /// <param name="rotation">The rotation of the sprite in radians.</param>
        /// <param name="origin">The point of origin for the sprites rotation.</param>
        /// <param name="size">The size to draw the texture with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2DArray texture, uint textureIndex, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 size, SpriteEffects effects, float layerDepth)
        {
            _batcher.AddBatch(SpriteBatcher.BatchPool.AquireBatch(effectTechnique, texture, textureIndex, position, sourceRectangle, color, rotation, origin, size, effects, layerDepth, _sortMode));
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2DArray texture, uint textureIndex, Rectangle destinationRectangle, RectangleF sourceRectangle, Color color)
        {
            Draw(effectTechnique, texture, textureIndex, new Vector2(destinationRectangle.X, destinationRectangle.Y), sourceRectangle, color, 0f, new Vector2(), new Vector2(destinationRectangle.Width, destinationRectangle.Height), SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2DArray texture, uint textureIndex, RectangleF destinationRectangle, RectangleF sourceRectangle, Color color)
        {
            //TODO
            Draw(effectTechnique, texture, textureIndex, new Vector2(destinationRectangle.X, destinationRectangle.Y), sourceRectangle, color, 0f, new Vector2(), new Vector2(destinationRectangle.Width, destinationRectangle.Height), SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2DArray texture, uint textureIndex, Vector2 position, RectangleF sourceRectangle, Color color)
        {
            Draw(effectTechnique, texture, textureIndex, position, sourceRectangle, color, 0, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None);//TODO?
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        /// <param name="rotation">The rotation of the sprite in radians.</param>
        /// <param name="origin">The point of origin for the sprites rotation.</param>
        /// <param name="scale">The scale to draw the texture with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2DArray texture, uint textureIndex, Vector2 position, RectangleF sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth = 0.0f)
        {
            Draw(effectTechnique, texture, textureIndex, position, sourceRectangle, color, rotation, origin, new Vector2(scale * texture.Width, scale * texture.Height), effects, layerDepth);//TODO?
        }
        
        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        /// <param name="rotation">The rotation of the sprite in radians.</param>
        /// <param name="origin">The point of origin for the sprites rotation.</param>
        /// <param name="size">The size to draw the texture with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2DArray texture, uint textureIndex, Vector2 position, RectangleF sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 size, SpriteEffects effects, float layerDepth)
        {
            _batcher.AddBatch(SpriteBatcher.BatchPool.AquireBatch(effectTechnique, texture, 0, position, sourceRectangle, color, rotation, origin, size, effects, layerDepth, _sortMode));
        }
#endregion
#region Texture2DEffects

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2D texture, Rectangle destinationRectangle, Color color)
        {
            Draw(effectTechnique, texture, destinationRectangle, null, color);
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2D texture, RectangleF destinationRectangle, Color color)
        {
            Draw(effectTechnique, texture, destinationRectangle, null, color);
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            Draw(effectTechnique, texture, new Vector2(destinationRectangle.X, destinationRectangle.Y), sourceRectangle, color, 0f, new Vector2(), new Vector2(destinationRectangle.Width, destinationRectangle.Height), SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2D texture, RectangleF destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            Draw(effectTechnique, texture, new Vector2(destinationRectangle.X, destinationRectangle.Y), sourceRectangle, color, 0f, new Vector2(), new Vector2(destinationRectangle.Width, destinationRectangle.Height), SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2D texture, Vector2 position, Color color)
        {
            Draw(effectTechnique, texture, position, null, color);
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            Draw(effectTechnique, texture, position, sourceRectangle, color, 0, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None);//TODO?
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        /// <param name="rotation">The rotation of the sprite in radians.</param>
        /// <param name="origin">The point of origin for the sprites rotation.</param>
        /// <param name="scale">The scale to draw the texture with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth = 0.0f)
        {
            Draw(effectTechnique, texture, position, sourceRectangle, color, rotation, origin, new Vector2(scale * texture.Width, scale * texture.Height), effects, layerDepth);//TODO?
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        /// <param name="rotation">The rotation of the sprite in radians.</param>
        /// <param name="origin">The point of origin for the sprites rotation.</param>
        /// <param name="size">The size to draw the texture with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 size, SpriteEffects effects, float layerDepth)
        {
            _batcher.AddBatch(SpriteBatcher.BatchPool.AquireBatch(effectTechnique,  texture, position, sourceRectangle, color, rotation, origin, size, effects, layerDepth, _sortMode));
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2D texture, Rectangle destinationRectangle, RectangleF sourceRectangle, Color color)
        {
            Draw(effectTechnique, texture, new Vector2(destinationRectangle.X, destinationRectangle.Y), sourceRectangle, color, 0f, new Vector2(), new Vector2(destinationRectangle.Width, destinationRectangle.Height), SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2D texture, RectangleF destinationRectangle, RectangleF sourceRectangle, Color color)
        {
            //TODO
            Draw(effectTechnique, texture, new Vector2(destinationRectangle.X, destinationRectangle.Y), sourceRectangle, color, 0f, new Vector2(), new Vector2(destinationRectangle.Width, destinationRectangle.Height), SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2D texture, Vector2 position, RectangleF sourceRectangle, Color color)
        {
            Draw(effectTechnique, texture, position, sourceRectangle, color, 0, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None);//TODO?
        }

        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        /// <param name="rotation">The rotation of the sprite in radians.</param>
        /// <param name="origin">The point of origin for the sprites rotation.</param>
        /// <param name="scale">The scale to draw the texture with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2D texture, Vector2 position, RectangleF sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth = 0.0f)
        {
            Draw(effectTechnique, texture, position, sourceRectangle, color, rotation, origin, new Vector2(scale * texture.Width, scale * texture.Height), effects, layerDepth);//TODO?
        }
        
        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters sprite with.</param>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        /// <param name="rotation">The rotation of the sprite in radians.</param>
        /// <param name="origin">The point of origin for the sprites rotation.</param>
        /// <param name="size">The size to draw the texture with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        public void Draw(IModelTechnique effectTechnique,Texture2D texture, Vector2 position, RectangleF sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 size, SpriteEffects effects, float layerDepth)
        {
            _batcher.AddBatch(SpriteBatcher.BatchPool.AquireBatch(effectTechnique, texture, 0, position, sourceRectangle, color, rotation, origin, size, effects, layerDepth, _sortMode));
        }
#endregion

        /// <summary>
        /// Draws text using a given <see cref="SpriteFont"/>.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters with.</param>
        /// <param name="spriteFont">The sprite font to use to draw the characters.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The position to draw the text at.</param>
        /// <param name="color">The color to draw the text with.</param>
        /// <param name="rotation">The rotation of the text in radians.</param>
        /// <param name="origin">The origin point to rotate the text at.</param>
        /// <param name="scale">The scale to draw the text with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        public void DrawString(IModelTechnique effectTechnique,SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation = 0.0f, Vector2 origin = new Vector2(), float scale = 1.0f, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0.0f)
        {
            DrawString(effectTechnique, spriteFont, text, position, color, rotation, origin, new Vector2(scale, scale), effects, layerDepth);
        }

        /// <summary>
        /// Draws text using a given <see cref="SpriteFont"/>.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters with.</param>
        /// <param name="spriteFont">The sprite font to use to draw the characters.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The position to draw the text at.</param>
        /// <param name="color">The color to draw the text with.</param>
        /// <param name="rotation">The rotation of the text in radians.</param>
        /// <param name="origin">The origin point to rotate the text at.</param>
        /// <param name="scale">The scale to draw the text with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        /// <param name="overwritePalette">The palette to use for multicolor glyph rendering instead of the default palette.</param>
        public void DrawString(IModelTechnique effectTechnique,SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0.0f, FontPalette? overwritePalette = null)
        {
            var offset = new Vector2(0.0f, 0.0f);
            foreach (var (rune, nextRune) in new StringBuilderRuneEnumerable(text))
            {
                DrawCharacter(effectTechnique, spriteFont, rune, nextRune, position, overwritePalette, color, rotation, origin, scale, effects, layerDepth, ref offset);
            }
        }

        /// <summary>
        /// Draws text using a given <see cref="SpriteFont"/>.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters with.</param>
        /// <param name="spriteFont">The sprite font to use to draw the characters.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The position to draw the text at.</param>
        /// <param name="color">The color to draw the text with.</param>
        /// <param name="rotation">The rotation of the text in radians.</param>
        /// <param name="origin">The origin point to rotate the text at.</param>
        /// <param name="scale">The scale to draw the text with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        /// <param name="overwritePalette">The palette to use for multicolor glyph rendering instead of the default palette.</param>
        public void DrawString(IModelTechnique effectTechnique,SpriteFont spriteFont, ReadOnlySpan<char> text, Vector2 position, Color color, float rotation = 0.0f, Vector2 origin = new Vector2(), float scale = 1.0f, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0.0f, FontPalette? overwritePalette = null)
        {
            DrawString(effectTechnique, spriteFont, text, position, color, rotation, origin, new Vector2(scale, scale), effects, layerDepth, overwritePalette);
        }

        /// <summary>
        /// Draws text using a given <see cref="SpriteFont"/>.
        /// </summary>
        /// <param name="effectTechnique">The technique to draw the characters with.</param>
        /// <param name="spriteFont">The sprite font to use to draw the characters.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="position">The position to draw the text at.</param>
        /// <param name="color">The color to draw the text with.</param>
        /// <param name="rotation">The rotation of the text in radians.</param>
        /// <param name="origin">The origin point to rotate the text at.</param>
        /// <param name="scale">The scale to draw the text with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        /// <param name="overwritePalette">The palette to use for multicolor glyph rendering instead of the default palette.</param>
        public void DrawString(IModelTechnique effectTechnique, SpriteFont spriteFont, ReadOnlySpan<char> text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0.0f, FontPalette? overwritePalette = null)
        {
            var offset = new Vector2(0.0f, 0.0f);
            foreach(var (rune, nextRune) in new CharSpanRuneEnumerable(text))
            {
                DrawCharacter(effectTechnique, spriteFont, rune, nextRune, position, overwritePalette, color, rotation, origin, scale, effects, layerDepth, ref offset);
            }
        }

        private void DrawCharacter(IModelTechnique effectTechnique, SpriteFont spriteFont, Rune character,
            Rune? nextCharacter, Vector2 position, FontPalette? overwritePalette,
            Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth,
            ref Vector2 offset)
        {
            if (!spriteFont.CharacterMap.TryGetValue(character, out var fontChar))
            {
                if (character.Value == '\n')
                {
                    offset.X = 0;
                    offset.Y += spriteFont.LineSpacing * scale.Y;
                    return;
                }

                if (!spriteFont.DefaultCharacter.HasValue ||
                    !spriteFont.CharacterMap.TryGetValue(spriteFont.DefaultCharacter.Value, out fontChar))
                {
                    return;
                }
            }

            var palette = spriteFont.Palettes.FirstOrDefault();
            
            if (fontChar.GlyphLayers.Length == 0 || palette is null)
                DrawGlyph(effectTechnique, spriteFont, palette, overwritePalette, position, color, rotation, origin, scale, effects, layerDepth, offset, fontChar.Glyph);
            else
            {
                foreach (var glyphLayer in fontChar.GlyphLayers)
                {
                    DrawGlyph(effectTechnique, spriteFont, palette, overwritePalette, position, color, rotation, origin, scale, effects, layerDepth, offset, glyphLayer);
                }
            }
            offset.X += fontChar.Advance * scale.X;
            
            if (nextCharacter != null && spriteFont.Kernings.TryGetValue(new RunePair(character, nextCharacter.Value), out var kerning))
                offset.X += kerning * scale.X;
        }

        private void DrawGlyph(IModelTechnique effectTechnique, SpriteFont spriteFont, FontPalette? palette, FontPalette? overwritePalette, Vector2 position, Color color,
            float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth, Vector2 offset, FontGlyph glyph)
        {
            var destSize = scale * new Vector2(glyph.Size.X / glyph.TextureRegion.Width,
                glyph.Size.Y / glyph.TextureRegion.Height);


            static bool TryGetColor(FontPalette? palette, int index, out Color color)
            {
                if (palette is null || (index < 0 || index >= palette.Colors.Length))
                {
                    color = default;
                    return false;
                }

                color = palette.Colors[index];
                return true;
            }

            if ((!TryGetColor(overwritePalette, glyph.ColorIndex, out var glyphColor) && !TryGetColor(palette, glyph.ColorIndex, out glyphColor)))
                glyphColor = color;

            Draw(effectTechnique, spriteFont.Texture, position + offset + glyph.Offset * scale, glyph.TextureRegion,
                glyphColor, rotation, origin - offset, destSize, effects, layerDepth);
        }


        #region "Without Effects"
        
        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
            => Draw(GetCurrentTechnique(false), texture, destinationRectangle, color);


        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2D texture, RectangleF destinationRectangle, Color color)
            => Draw(GetCurrentTechnique(false), texture, destinationRectangle, color);


        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
            => Draw(GetCurrentTechnique(false), texture, destinationRectangle, sourceRectangle, color);


        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2D texture, RectangleF destinationRectangle, Rectangle? sourceRectangle, Color color)
            => Draw(GetCurrentTechnique(false), texture, destinationRectangle, sourceRectangle, color);


        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2D texture, Vector2 position, Color color)
            => Draw(GetCurrentTechnique(false), texture, position, color);


        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
            => Draw(GetCurrentTechnique(false), texture, position, sourceRectangle, color);


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
            => Draw(GetCurrentTechnique(false), texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);


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
            => Draw(GetCurrentTechnique(false), texture, position, sourceRectangle, color, rotation, origin, size, effects, layerDepth);


        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2D texture, Rectangle destinationRectangle, RectangleF sourceRectangle, Color color)
            => Draw(GetCurrentTechnique(false), texture, destinationRectangle, sourceRectangle, color);


        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2D texture, RectangleF destinationRectangle, RectangleF sourceRectangle, Color color)
            => Draw(GetCurrentTechnique(false), texture, destinationRectangle, sourceRectangle, color);


        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2D texture, Vector2 position, RectangleF sourceRectangle, Color color)
            => Draw(GetCurrentTechnique(false), texture, position, sourceRectangle, color);


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
            => Draw(GetCurrentTechnique(false), texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth = 0.0f);

        
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
            => Draw(GetCurrentTechnique(false), texture, position, sourceRectangle, color, rotation, origin, size, effects, layerDepth);

        private IModelTechnique GetFontTechnique(SpriteFontType fontType)
        {
            switch (fontType)
            {
                case SpriteFontType.BitmapFont:
                    return GetCurrentTechnique(false);
                case SpriteFontType.MultiSignedDistanceField:
                case SpriteFontType.MultiSignedAndTrueDistanceField:
                    return _fontEffect.MultiSignedTechnique;
                default:
                    throw new NotImplementedException();
            }
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
        public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color,
            float rotation = 0.0f, Vector2 origin = new Vector2(), float scale = 1.0f,
            SpriteEffects effects = SpriteEffects.None, float layerDepth = 0.0f)
            => DrawString(GetFontTechnique(spriteFont.FontType), spriteFont, text, position, color, rotation, origin, scale, effects,
                layerDepth);


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
            => DrawString(GetFontTechnique(spriteFont.FontType), spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);


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
        public void DrawString(SpriteFont spriteFont, ReadOnlySpan<char> text, Vector2 position, Color color, float rotation = 0.0f, Vector2 origin = new Vector2(), float scale = 1.0f, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0.0f)
            => DrawString(GetFontTechnique(spriteFont.FontType), spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);


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
        /// <param name="overwritePalette">The palette to use for multicolor glyph rendering instead of the default palette.</param>
        public void DrawString(SpriteFont spriteFont, ReadOnlySpan<char> text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0.0f, FontPalette? overwritePalette = null)
            => DrawString(GetFontTechnique(spriteFont.FontType), spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth, overwritePalette);

        #endregion
        
        #region "Array Without Effects"
        
        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2DArray texture, uint textureIndex, Rectangle destinationRectangle, Color color)
            => Draw(GetCurrentTechnique(true), texture, textureIndex, destinationRectangle, color);


        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2DArray texture, uint textureIndex, RectangleF destinationRectangle, Color color)
            => Draw(GetCurrentTechnique(true), texture, textureIndex, destinationRectangle, color);


        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2DArray texture, uint textureIndex, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
            => Draw(GetCurrentTechnique(true), texture, textureIndex, destinationRectangle, sourceRectangle, color);


        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2DArray texture, uint textureIndex, RectangleF destinationRectangle, Rectangle? sourceRectangle, Color color)
            => Draw(GetCurrentTechnique(true), texture, textureIndex, destinationRectangle, sourceRectangle, color);


        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2DArray texture, uint textureIndex, Vector2 position, Color color)
            => Draw(GetCurrentTechnique(true), texture, textureIndex, position, color);


        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2DArray texture, uint textureIndex, Vector2 position, Rectangle? sourceRectangle, Color color)
            => Draw(GetCurrentTechnique(true), texture, textureIndex, position, sourceRectangle, color);


        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        /// <param name="rotation">The rotation of the sprite in radians.</param>
        /// <param name="origin">The point of origin for the sprites rotation.</param>
        /// <param name="scale">The scale to draw the texture with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        public void Draw(Texture2DArray texture, uint textureIndex, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth = 0.0f)
            => Draw(GetCurrentTechnique(true), texture, textureIndex, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);


        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        /// <param name="rotation">The rotation of the sprite in radians.</param>
        /// <param name="origin">The point of origin for the sprites rotation.</param>
        /// <param name="size">The size to draw the texture with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        public void Draw(Texture2DArray texture, uint textureIndex, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 size, SpriteEffects effects, float layerDepth)
            => Draw(GetCurrentTechnique(true), texture, textureIndex, position, sourceRectangle, color, rotation, origin, size, effects, layerDepth);


        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2DArray texture, uint textureIndex, Rectangle destinationRectangle, RectangleF sourceRectangle, Color color)
            => Draw(GetCurrentTechnique(true), texture, textureIndex, destinationRectangle, sourceRectangle, color);


        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="destinationRectangle">The destination region to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2DArray texture, uint textureIndex, RectangleF destinationRectangle, RectangleF sourceRectangle, Color color)
            => Draw(GetCurrentTechnique(true), texture, textureIndex, destinationRectangle, sourceRectangle, color);


        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        public void Draw(Texture2DArray texture, uint textureIndex, Vector2 position, RectangleF sourceRectangle, Color color)
            => Draw(GetCurrentTechnique(true), texture, textureIndex, position, sourceRectangle, color);


        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        /// <param name="rotation">The rotation of the sprite in radians.</param>
        /// <param name="origin">The point of origin for the sprites rotation.</param>
        /// <param name="scale">The scale to draw the texture with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        public void Draw(Texture2DArray texture, uint textureIndex, Vector2 position, RectangleF sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth = 0.0f)
            => Draw(GetCurrentTechnique(true), texture, textureIndex, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth = 0.0f);

        
        /// <summary>
        /// Draws a texture sprite.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="textureIndex">The texture index to draw from the texture array.</param>
        /// <param name="position">The position to draw the texture at.</param>
        /// <param name="sourceRectangle">The source rectangle to crop out of the texture to draw. Or null to use the whole texture.</param>
        /// <param name="color">The color to use for colorizing the texture. Use <see cref="Color.White"/> to draw the texture without colorizing.</param>
        /// <param name="rotation">The rotation of the sprite in radians.</param>
        /// <param name="origin">The point of origin for the sprites rotation.</param>
        /// <param name="size">The size to draw the texture with.</param>
        /// <param name="effects">The sprite effects to be applied to the sprite.</param>
        /// <param name="layerDepth">The layer depth of this sprite used for depth sorting.</param>
        public void Draw(Texture2DArray texture, uint textureIndex, Vector2 position, RectangleF sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 size, SpriteEffects effects, float layerDepth)
            => Draw(GetCurrentTechnique(true), texture, textureIndex, position, sourceRectangle, color, rotation, origin, size, effects, layerDepth);
        
        #endregion
        
        
        /// <summary>
        /// Ends the drawing process and batches all draws together to finally draw them.
        /// </summary>
        public void End()
        {
            GraphicsDevice.BlendState = _blendState;
            GraphicsDevice.RasterizerState = _rasterizerState;
            GraphicsDevice.DepthStencilState = _depthStencilState;
            _batcher.SamplerState = _samplerState;

            if (_useScreenSpace)
            {
                var projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, -1);
               
                _batcher.End(_matrix, projection);
            }
            else
            {
                _batcher.End(_matrix, Matrix.Identity);
            }
            


        }

        /// <inheritdoc />
        public override void Dispose()
        {
            _batcher.Dispose();
            if (_effect is IDisposable disp)
            {
                disp.Dispose();
            }
            base.Dispose();
        }
    }
}

