using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static BaseLib.Utility.Utility;

namespace EnhancedTooltip.Tooltip
{
	public class PriceTooltip
	{
		public static TwoColumnLine GetLine(DrawableTooltipLine line, Item item)
		{
			string valueText = "";
			if (Main.npcShop > 0 || item.value > 0)
			{
				int itemValue = item.value;
				if (!item.buy) itemValue /= 5;
				itemValue *= item.stack;

				int[] coins = Utils.CoinsSplit(itemValue);

				if (itemValue == 0) return new TwoColumnLine("Value: None", COLOR_NOCOIN, Vector2.One);

				if (coins[3] > 0) valueText += $" [c/{RGBToHex(COLOR_PLATINUM)}:{coins[3]}p]";
				if (coins[2] > 0) valueText += $" [c/{RGBToHex(COLOR_GOLD)}:{coins[2]}g]";
				if (coins[1] > 0) valueText += $" [c/{RGBToHex(COLOR_SILVER)}:{coins[1]}s]";
				if (coins[0] > 0) valueText += $" [c/{RGBToHex(COLOR_COPPER)}:{coins[0]}c]";
			}

			return new TwoColumnLine(item.buy ? "Buy" : "Sell" + " price: ", $"{valueText}", Color.White, line.baseScale);
		}
	}
}