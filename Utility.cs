using System.Text;
using Microsoft.Xna.Framework;

namespace EnhancedTooltip
{
	public static class Utility
	{
		public static string RGBToHex(Color color) => color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");

		public static string CText(params object[] args)
		{
			StringBuilder sb = new StringBuilder();
			if (args[0] is Color) sb.Append($"[c/{RGBToHex((Color)args[0])}:");
			for (int i = 0; i < args.Length; i++) if (!(args[i] is Color)) sb.Append(args[i]);
			if (args[0] is Color) sb.Append("]");
			return sb.ToString();
		}

		public static Color DoubleLerp(Color c1, Color c2, Color c3, float step) => step < .5f ? Color.Lerp(c1, c2, step * 2f) : Color.Lerp(c2, c3, (step - .5f) * 2f);
	}
}