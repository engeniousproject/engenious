namespace engenious
{
    /// <summary>
    /// Transformation information for animations.
    /// </summary>
    public class AnimationTransform
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationTransform"/> class.
        /// </summary>
        /// <param name="name">An associated name.</param>
        /// <param name="location">The translation.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="quaternion">The rotation.</param>
        public AnimationTransform(string name,Vector3 location, Vector3 scale, Quaternion quaternion)
        {
            //dubi += name + ": " + quaternion.ToString() + "\n";
            Location = location;
            if (name.Contains("$")){
                Location = new Vector3(location.X,location.Z,location.Y);
            }
            Scale = scale;
            Rotation = quaternion;
        }

        /// <summary>
        /// Gets a <see cref="Quaternion"/> indicating the rotational transformation part.
        /// </summary>
        public Quaternion Rotation{get;private set;}

        /// <summary>
        /// Gets a <see cref="Vector3"/> indicating the translational transformation part.
        /// </summary>
        public Vector3 Location{get;private set;}

        /// <summary>
        /// Gets a <see cref="Vector3"/> indicating the scaling transformation part.
        /// </summary>
        public Vector3 Scale{get;private set;}

        /// <summary>
        /// Converts this <see cref="AnimationTransform"/> to a transformation <see cref="Matrix"/>.
        /// </summary>
        /// <returns>The resulting transformation <see cref="Matrix"/>.</returns>
        public Matrix ToMatrix()
        {
            var res = Matrix.CreateFromQuaternion(Rotation.X, Rotation.Y, Rotation.Z, Rotation.W);
            res.M11 *= Scale.X;
            res.M12 *= Scale.X;
            res.M13 *= Scale.X;
            res.M21 *= Scale.Y;
            res.M22 *= Scale.Y;
            res.M23 *= Scale.Y;
            res.M31 *= Scale.Z;
            res.M32 *= Scale.Z;
            res.M33 *= Scale.Z;
            res.M41 = Location.X;
            res.M42 = Location.Y;
            res.M43 = Location.Z;
            return res;
        }

        /// <summary>
        /// Lerps between two animation transforms using a given <paramref name="amount"/>.
        /// </summary>
        /// <param name="transform1">The <see cref="AnimationTransform"/> to lerp from.</param>
        /// <param name="transform2">The <see cref="AnimationTransform"/> to lerp to.</param>
        /// <param name="amount">The amount to lerp by.</param>
        /// <returns>The resulting interpolated <see cref="AnimationTransform"/>.</returns>
        public static AnimationTransform Lerp(AnimationTransform transform1,AnimationTransform transform2,float amount)
        {
            return new AnimationTransform(string.Empty,Vector3.Lerp(transform1.Location,transform2.Location,amount),
                                            Vector3.Lerp(transform1.Scale,transform2.Scale,amount),
                                            Quaternion.Lerp(transform1.Rotation,transform2.Rotation,amount));
        }

        /// <summary>
        /// Concats two animation transforms.
        /// </summary>
        /// <param name="t1">The first <see cref="AnimationTransform"/>.</param>
        /// <param name="t2">The second <see cref="AnimationTransform"/>.</param>
        /// <returns>The resulting concatenated <see cref="AnimationTransform"/>.</returns>
        public static AnimationTransform operator +(AnimationTransform t1,AnimationTransform t2)
        {
            return new AnimationTransform(string.Empty,t1.Location+t2.Location,t1.Scale*t2.Scale,t1.Rotation*t2.Rotation);
        }

        /// <summary>
        /// Transforms an <see cref="AnimationTransform"/> by a <see cref="Matrix"/>.
        /// </summary>
        /// <param name="t">The <see cref="AnimationTransform"/>.</param>
        /// <param name="transformation">The transformation <see cref="Matrix"/>.</param>
        /// <returns>The transformed <see cref="AnimationTransform"/>.</returns>
        public static AnimationTransform Transform(AnimationTransform t,Matrix transformation)
        {
            return new AnimationTransform(string.Empty,Vector3.Transform(t.Location,transformation),
                t.Scale,
                t.Rotation);
        }
    }
}

