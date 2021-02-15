namespace engenious.Graphics
{
    /// <summary>
    /// Defines the render view dimensions to project to.
    /// </summary>
    public struct Viewport
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Viewport"/> struct.
        /// </summary>
        /// <param name="x">The horizontal lower bound of the view.</param>
        /// <param name="y">The vertical lower bound of the view.</param>
        /// <param name="width">The width of the view.</param>
        /// <param name="height">The height of the view.</param>
        /// <param name="minDepth">The minimal depth of the view.</param>
        /// <param name="maxDepth">The maximum depth of the view.</param>
        public Viewport(int x, int y, int width, int height, float minDepth = 0.0f, float maxDepth = 1.0f)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            MinDepth = minDepth;
            MaxDepth = maxDepth;
        }

        internal Viewport(System.Drawing.Rectangle bounds)
            : this(bounds.X, bounds.Y, bounds.Width, bounds.Height)
        {
			
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Viewport"/> struct.
        /// </summary>
        /// <param name="bounds">The view bounds.</param>
        public Viewport(Rectangle bounds)
            : this(bounds.X, bounds.Y, bounds.Width, bounds.Height)
        {

        }

        /// <summary>
        /// Gets the views aspect ratio.
        /// </summary>
        public float AspectRatio => (float)Width / Height;

        /// <summary>
        /// Gets the view width.
        /// </summary>
        public int Width{ get; internal set; }

        /// <summary>
        /// Gets the view height.
        /// </summary>
        public int Height{ get; internal set; }

        /// <summary>
        /// Gets the views horizontal lower bound.
        /// </summary>
        public int X{ get; internal set; }

        /// <summary>
        /// Gets the views vertical lower bound.
        /// </summary>
        public int Y{ get; internal set; }

        /// <summary>
        /// Gets the views maximum depth.
        /// </summary>
        public float MaxDepth{ get; internal set; }

        /// <summary>
        /// Gets the views minimal depth.
        /// </summary>
        public float MinDepth{ get; internal set; }

        /// <summary>
        /// Gets the views bounds.
        /// </summary>
        public Rectangle Bounds
        {
            get => new Rectangle(X, Y, Width, Height);
            internal set
            {
                X = value.X;
                Y = value.Y;
                Width = value.Width;
                Height = value.Height;
            }
        }

        /// <summary>
        /// Projects a <see cref="Vector3"/> from screen space into object space using given matrix.
        /// </summary>
        /// <param name="source">The <see cref="Vector3"/> to project.</param>
        /// <param name="matrix">The matrix to use for projection.</param>
        /// <returns>The <see cref="Vector3"/> in object space.</returns>
        public Vector3 Unproject(Vector3 source, Matrix matrix)
        {
            matrix = Matrix.Invert(matrix);
            source.X = (((source.X - X) / Width) * 2f) - 1f;
            source.Y = -((((source.Y - Y) / Height) * 2f) - 1f);
            source.Z = (source.Z - MinDepth) / (MaxDepth - MinDepth);
            var vector = Vector3.Transform(matrix, source);
            var a = (((source.X * matrix.M41) + (source.Y * matrix.M42)) + (source.Z * matrix.M43)) + matrix.M44;
            //float a = (((source.X * matrix.M41) + (source.Y * matrix.M42)) + (source.Z * matrix.M43)) + matrix.M44;
            vector.X = vector.X / a;
            vector.Y = vector.Y / a;
            vector.Z = vector.Z / a;

            return vector;
        }

        /// <summary>
        /// Projects a <see cref="Vector3"/> from screen space into object space using given world, view and projection matrices.
        /// </summary>
        /// <param name="source">The <see cref="Vector3"/> to project.</param>
        /// <param name="projection">The projection matrix.</param>
        /// <param name="view">The view matrix.</param>
        /// <param name="world">The world matrix.</param>
        /// <returns>The <see cref="Vector3"/> in object space.</returns>
        public Vector3 Unproject(Vector3 source, Matrix projection, Matrix view, Matrix world)
        {
            return Unproject(source, projection * view * world);
        }

        /// <summary>
        /// Projects a <see cref="Vector3"/> from object space into screen space using given matrix.
        /// </summary>
        /// <param name="source">The <see cref="Vector3"/> to project.</param>
        /// <param name="matrix">The matrix to use for projection.</param>
        /// <returns>The <see cref="Vector3"/> in screen space.</returns>
        public Vector3 Project(Vector3 source, Matrix matrix)
        {

            var vector = Vector3.Transform(matrix, source);
            var a = (((source.X * matrix.M14) + (source.Y * matrix.M24)) + (source.Z * matrix.M34)) + matrix.M44;

            vector.X = vector.X / a;
            vector.Y = vector.Y / a;
            vector.Z = vector.Z / a;

            vector.X = (((vector.X + 1f) * 0.5f) * Width) + X;
            vector.Y = (((-vector.Y + 1f) * 0.5f) * Height) + Y;
            vector.Z = (vector.Z * (MaxDepth - MinDepth)) + MinDepth;
            return vector;
        }

        /// <summary>
        /// Projects a <see cref="Vector3"/> from object space into screen space using given world, view and projection matrices.
        /// </summary>
        /// <param name="source">The <see cref="Vector3"/> to project.</param>
        /// <param name="projection">The projection matrix.</param>
        /// <param name="view">The view matrix.</param>
        /// <param name="world">The world matrix.</param>
        /// <returns>The <see cref="Vector3"/> in screen space.</returns>
        public Vector3 Project(Vector3 source, Matrix projection, Matrix view, Matrix world)
        {
            return Project(source, projection * view * world);

        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format("[Viewport: X={0}, Y={1}, Width={2}, Height={3}]", X, Y, Width, Height);
        }

        /*public override bool Equals (object obj)
		{
			if (!(obj is Viewport))
				return false;
			Viewport v2 = (Viewport)obj;
			return this == v2;
		}

		public static bool operator==(Viewport v1,Viewport v2){
			return v1.X == v2.X && v1.Y == v2.Y && v1.Width == v2.Width && v1.Height == v2.Height;
		}
		public static bool operator!=(Viewport v1,Viewport v2){
			return v1.X != v2.X || v1.Y != v2.Y || v1.Width != v2.Width ||  v1.Height != v2.Height;
		}*/

    }
			
}