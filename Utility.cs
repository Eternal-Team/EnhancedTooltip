using System.Text;
using Microsoft.Xna.Framework;

namespace EnhancedTooltip
{
	public static class Utility
	{
		public static string CText(params object[] args)
		{
			StringBuilder sb = new StringBuilder();
			if (args[0] is Color) sb.Append($"[c/{TheOneLibrary.Utility.Utility.RGBToHex((Color)args[0])}:");
			for (int i = 0; i < args.Length; i++) if (!(args[i] is Color)) sb.Append(args[i]);
			if (args[0] is Color) sb.Append("]");
			return sb.ToString();
		}
	}
}