using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EnhancedTooltip.Tooltip
{
	public class SpeedTooltip
	{
		public static TwoColumnLine GetLine(DrawableTooltipLine line, Item item)
		{
			string speedText;
			if (item.useTime <= 8) speedText = "Insanely fast";
			else if (item.useTime <= 20) speedText = "Very fast";
			else if (item.useTime <= 25) speedText = "Fast";
			else if (item.useTime <= 30) speedText = "Average";
			else if (item.useTime <= 35) speedText = "Slow";
			else if (item.useTime <= 45) speedText = "Very slow";
			else if (item.useTime <= 55) speedText = "Extremely slow";
			else speedText = "Snail";
			return new TwoColumnLine("Speed:", $"{speedText} ({item.useTime})", line.color, BaseLib.Utility.Utility.DoubleLerp(Color.Red, Color.Yellow, Color.Lime, 55f / (float)item.useTime), line.baseScale);
		}
	}
}