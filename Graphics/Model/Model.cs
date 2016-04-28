using System;
using engenious.Graphics;
using System.Collections.Generic;

namespace engenious.Graphics
{
    public class Model : GraphicsResource
    {
        public Model(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            Animation = new Animation();
        }

        public Mesh[] Meshes{ get; set; }

        internal Node RootNode{ get; set; }

        internal List<Node> Nodes{ get; set; }

        internal Animation Animation{ get; set; }

        public void UpdateAnimation(float elapsed)
        {
            Animation.Update(elapsed);

            UpdateAnimation(null, RootNode);
        }

        private void UpdateAnimation(Node parent, Node node)
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

        public void Draw()
        {
            foreach (var item in Meshes)
            {
                item.Draw();
            }
        }

        public void Draw(BasicEffect effect, Texture2D text)
        {
            DrawNode(RootNode, effect, text);
        }

        internal void DrawNode(Node node, BasicEffect effect, Texture2D text)
        {
            if (node.Meshes.Count == 0 && node.Children.Count == 0)
                return;
            effect.Texture = text;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                effect.World = node.GlobalTransform * node.Transformation;

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

