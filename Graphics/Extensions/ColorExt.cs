using System;
using OpenToolkit.Mathematics;

namespace System.Drawing
{
	/// <summary>
	/// Extension class for <see cref="System.Drawing.Color"/>.
	/// </summary>
	public static class ColorExt
	{
		/// <summary>
		/// Converts the <see cref="System.Drawing.Color"/> to a <see cref="Vector4"/>.
		/// </summary>
		/// <param name="color">The <see cref="System.Drawing.Color"/> to convert.</param>
		/// <returns>The resulting (r,g,b,a) <see cref="Vector4"/>.</returns>
		public static Vector4 ToVector4(this Color color)
		{
			return new Vector4 (color.R/255f, color.G/255f, color.B/255f, color.A/255f);
		}

		/// <summary>
		/// Converts the <see cref="System.Drawing.Color"/> to a <see cref="Vector3"/>.
		/// </summary>
		/// <param name="color">The <see cref="System.Drawing.Color"/> to convert.</param>
		/// <returns>The resulting (r,g,b) <see cref="Vector3"/>.</returns>
		public static Vector3 ToVector3(this Color color)
		{
			return new Vector3 (color.R/255f, color.G/255f, color.B/255f);
		}
	}
}

