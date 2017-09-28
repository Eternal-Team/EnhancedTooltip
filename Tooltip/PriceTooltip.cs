using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EnhancedTooltip.Tooltip
{
	public class PriceTooltip
	{
		public static readonly Color
			COLOR_PLATINUM = new Color(220, 220, 198),
			COLOR_GOLD = new Color(224, 201, 92),
			COLOR_SILVER = new Color(181, 192, 193),
			COLOR_COPPER = new Color(246, 138, 96),
			COLOR_NOCOIN = new Color(120, 120, 120);

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

				if (coins[3] > 0) valueText += $" [c/{Utility.RGBToHex(COLOR_PLATINUM)}:{coins[3]}p ]";
				if (coins[2] > 0) valueText += $" [c/{Utility.RGBToHex(COLOR_GOLD)}:{coins[2]}g ]";
				if (coins[1] > 0) valueText += $" [c/{Utility.RGBToHex(COLOR_SILVER)}:{coins[1]}s ]";
				if (coins[0] > 0) valueText += $" [c/{Utility.RGBToHex(COLOR_COPPER)}:{coins[0]}c ]";
			}

			return new TwoColumnLine(item.buy ? "Buy" : "Sell" + " price: ", $"{ valueText }", Color.White, line.baseScale);
		}
	}
}