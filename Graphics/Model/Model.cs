using System;
using System.Collections.Generic;

namespace engenious.Graphics
{
    /// <summary>
    /// Describes a drawable model.
    /// </summary>
    public class Model : GraphicsResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Model"/> class.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="meshCount">The number of meshes for this model.</param>
        public Model(GraphicsDevice graphicsDevice, int meshCount)
            : base(graphicsDevice)
        {
            Meshes = new IMesh[meshCount];
            Animations = new List<Animation>();
            Nodes = new List<Node>();
            Transform = Matrix.Identity;
        }

        /// <inheritdoc cref="GraphicsResource.GraphicsDevice"/>
        public new GraphicsDevice GraphicsDevice => base.GraphicsDevice!;

        /// <summary>
        /// Gets or sets the meshes this <see cref="Model"/> consists of.
        /// </summary>
        public IMesh[] Meshes{ get; }

        internal Node? RootNode{ get; set; }

        internal List<Node> Nodes{ get; }

        /// <summary>
        /// Gets or sets the animations available for this model.
        /// </summary>
        public List<Animation> Animations{ get; }

        /// <summary>
        /// Gets or sets the current animation to animate.
        /// </summary>
        public Animation? CurrentAnimation { get; set; }

        /// <summary>
        /// Gets or sets the transform of this model.
        /// </summary>
        public Matrix Transform { get; set; }

        /// <summary>
        /// Advances the animation by a given amount.
        /// </summary>
        /// <param name="elapsed">The amount to advance the animation by.</param>
        public void UpdateAnimation(float elapsed)
        {
            if (RootNode == null || CurrentAnimation == null)
                return;
            CurrentAnimation.Update(elapsed);

            UpdateAnimation(null, RootNode);
        }

        internal void UpdateAnimation(Node? parent, Node node)
        {

            if (parent == null)
                node.GlobalTransform = node.LocalTransform * node.Transformation;
            else
                node.GlobalTransform = parent.GlobalTransform * node.LocalTransform * node.Transformation;

            foreach (var child in node.Children)
            {
                UpdateAnimation(node, child);
            }
        }

        /// <summary>
        /// Renders the model.
        /// </summary>
        public void Draw()
        {
            foreach (var item in Meshes)
            {
                item.Draw();
            }
        }

        /// <summary>
        /// Renders the model using a given custom model effect, as well as a texture.
        /// </summary>
        /// <param name="effect">The model effect to use for rendering.</param>
        /// <param name="text">The texture to use for rendering.</param>
        public void Draw(IModelEffect effect, Texture2D text)
        {
            if (RootNode == null)
                return;
            DrawNode(RootNode, effect, text);
        }

        internal void DrawNode(Node node, IModelEffect effect, Texture2D text)
        {
            if (node.Meshes.Count == 0 && node.Children.Count == 0)
                return;
            effect.Texture = text;

            if (effect.CurrentTechnique == null)
                throw new ArgumentException("The effect has no technique selected", nameof(effect));

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                effect.World = Transform * node.GlobalTransform;
                

                foreach (var mesh in node.Meshes)
                {
                    mesh.Draw();
                }

                foreach (var child in node.Children)
                    DrawNode(child, effect, text);
            }
        }
    }
}

