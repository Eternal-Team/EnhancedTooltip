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
			if (item.useAnimation <= 8) speedText = "Insanely fast";
			else if (item.useAnimation <= 20) speedText = "Very fast";
			else if (item.useAnimation <= 25) speedText = "Fast";
			else if (item.useAnimation <= 30) speedText = "Average";
			else if (item.useAnimation <= 35) speedText = "Slow";
			else if (item.useAnimation <= 45) speedText = "Very slow";
			else if (item.useAnimation <= 55) speedText = "Extremely slow";
			else speedText = "Snail";
			return new TwoColumnLine("Speed:", $"{speedText} ({item.useTime})", line.color, TheOneLibrary.Utils.Utility.DoubleLerp(Color.Red, Color.Yellow, Color.Lime, 55f / item.useTime), line.baseScale);
		}
	}
}