using System;
using UIKit;

namespace Occur.TextStyles.Touch
{
	public static class UIColorExtensions
	{
		/// <summary>
		/// Converts a UIColor value to a string hex value
		/// </summary>
		/// <returns>A string hex value</returns>
		/// <param name="color">Target UIColor</param>
		public static string ToHex (this UIColor color)
		{
			nfloat rFloat, gFloat, bFloat, aFloat;
			color.GetRGBA (out rFloat, out gFloat, out bFloat, out aFloat);

			int r, g, b;
			r = (int)(255.0 * rFloat);
			g = (int)(255.0 * gFloat);
			b = (int)(255.0 * bFloat);

			return string.Format ("#{0:X2}{1:X2}{2:X2}", r, g, b);
		}

		/// <summary>
		/// Creates a UIColor from a int hex value
		/// </summary>
		/// <returns>UIColor</returns>
		/// <param name="color">Extension UIColor reference</param>
		/// <param name="hexValue">Hex value as an int</param>
		public static UIColor FromHex (this UIColor color, int hexValue)
		{
			return UIColor.FromRGB (
				(((float)((hexValue & 0xFF0000) >> 16)) / 255.0f),
				(((float)((hexValue & 0xFF00) >> 8)) / 255.0f),
				(((float)(hexValue & 0xFF)) / 255.0f)
			);
		}

		/// <summary>
		/// Creates a UIColor from a string hex value
		/// </summary>
		/// <returns>UIColor</returns>
		/// <param name="color">Extension UIColor reference</param>
		/// <param name="hexValue">Hex value as an int</param>
		/// <param name="alpha">Alpha value of the color</param>
		public static UIColor FromHex (this UIColor color, string hexValue, float alpha = 1.0f)
		{
			var colorString = hexValue.Replace ("#", "");
			if (alpha > 1.0f) {
				alpha = 1.0f;
			} else if (alpha < 0.0f) {
				alpha = 0.0f;
			}

			float red, green, blue;

			switch (colorString.Length) {
			case 3: // #RGB
				{
					red = Convert.ToInt32 (string.Format ("{0}{0}", colorString.Substring (0, 1)), 16) / 255f;
					green = Convert.ToInt32 (string.Format ("{0}{0}", colorString.Substring (1, 1)), 16) / 255f;
					blue = Convert.ToInt32 (string.Format ("{0}{0}", colorString.Substring (2, 1)), 16) / 255f;
					return UIColor.FromRGBA (red, green, blue, alpha);
				}
			case 6: // #RRGGBB
				{
					red = Convert.ToInt32 (colorString.Substring (0, 2), 16) / 255f;
					green = Convert.ToInt32 (colorString.Substring (2, 2), 16) / 255f;
					blue = Convert.ToInt32 (colorString.Substring (4, 2), 16) / 255f;
					return UIColor.FromRGBA (red, green, blue, alpha);
				}   

			default :
				throw new ArgumentOutOfRangeException (string.Format ("Invalid color value {0} is invalid. It should be a hex value of the form #RBG, #RRGGBB", hexValue));
			}
		}
	}
}

