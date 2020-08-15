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
        public Model(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            Animations = new List<Animation>();
            Transform = Matrix.Identity;
        }

        /// <summary>
        /// Gets or sets the meshes this <see cref="Model"/> consists of.
        /// </summary>
        public IMesh[] Meshes{ get; set; }

        internal Node RootNode{ get; set; }

        internal List<Node> Nodes{ get; set; }

        /// <summary>
        /// Gets or sets the animations available for this model.
        /// </summary>
        public List<Animation> Animations{ get; set; }

        /// <summary>
        /// Gets or sets the current animation to animate.
        /// </summary>
        public Animation CurrentAnimation{get;set;}

        /// <summary>
        /// Gets or sets the transform of this model.
        /// </summary>
        public Matrix Transform{get;set;}

        /// <summary>
        /// Advances the animation by a given amount.
        /// </summary>
        /// <param name="elapsed">The amount to advance the animation by.</param>
        public void UpdateAnimation(float elapsed)
        {
            CurrentAnimation.Update(elapsed);

            UpdateAnimation(null, RootNode);
        }

        internal void UpdateAnimation(Node parent, Node node)
        {

            if (parent == null)
                node.GlobalTransform = node.LocalTransform;
            else
                node.GlobalTransform = parent.GlobalTransform * node.LocalTransform;

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
            DrawNode(RootNode, effect, text);
        }

        internal void DrawNode(Node node, IModelEffect effect, Texture2D text)
        {
            if (node.Meshes.Count == 0 && node.Children.Count == 0)
                return;
            effect.Texture = text;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                
                effect.World = Transform*node.GlobalTransform * node.Transformation;

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

