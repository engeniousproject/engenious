using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace engenious
{
    /// <summary>
    /// Helper class for handling ImageSharp.
    /// </summary>
    public static class ImageSharpHelper
    {
        /// <summary>
        /// The preferred configuration for engenious images.
        /// </summary>
        public static Configuration Config;

        static ImageSharpHelper()
        {
            Config = Configuration.Default.Clone();
            Config.PreferContiguousImageBuffers = true;
        }

        /// <summary>
        /// Converts a given <see cref="Image"/> to a in memory continuous <see cref="Image{TPixel}"/> .
        /// </summary>
        /// <param name="image">The <see cref="Image"/> to convert.</param>
        /// <typeparam name="TPixel">The destination pixel format.</typeparam>
        /// <returns>
        /// Converted <see cref="Image{TPixel}"/> if input was not continous and in <typeparamref name="TPixel"/> format;
        /// otherwise casted <paramref name="image"/>.
        /// </returns>
        public static Image<TPixel> ToContinuousImage<TPixel>(this Image image)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            if (image.GetConfiguration().PreferContiguousImageBuffers && image is Image<TPixel> rgba32Image)
            {
                return rgba32Image;
            }

            return image.CloneAs<TPixel>(ImageSharpHelper.Config);
        }

    }
}