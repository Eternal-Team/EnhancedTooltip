using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace EnhancedTooltip.Tooltip.Common
{
	public class PriceLine : BaseTooltipLine
	{
		public string Text;
		public Color Color;

		public PriceLine(Item item)
		{
			float dark = Main.mouseTextColor / 255f;
			int alpha = Main.mouseTextColor;

			int storeValue = item.GetStoreValue();
			if (item.shopSpecialCurrency != -1)
			{
				// tis weird code
				//CustomCurrencyManager.GetPriceText(item.shopSpecialCurrency, texts, ref lenght, storeValue);
				Color = new Color((byte)(255f * dark), (byte)(255f * dark), (byte)(255f * dark), alpha);
			}
			else if (storeValue > 0)
			{
				int value = storeValue * item.stack;
				if (!item.buy)
				{
					value = storeValue / 5;
					if (value < 1) value = 1;

					value *= item.stack;
				}

				if (value < 1) value = 1;

				int[] coinStacks = Utils.CoinsSplit(value);

				string text = "";
				for (int i = coinStacks.Length - 1; i >= 0; i--)
				{
					if (coinStacks[i] > 0) text += $"{coinStacks[i]} {Language.GetTextValue($"LegacyInterface.{18 - i}")} ";
				}

				Text = !item.buy ? $"{Language.GetTextValue("LegacyTooltip.49")} {text}" : $"{Language.GetTextValue("LegacyTooltip.50")} {text}";

				if (coinStacks[3] > 0) Color = new Color((byte)(220f * dark), (byte)(220f * dark), (byte)(198f * dark), alpha);
				else if (coinStacks[2] > 0) Color = new Color((byte)(224f * dark), (byte)(201f * dark), (byte)(92f * dark), alpha);
				else if (coinStacks[1] > 0) Color = new Color((byte)(181f * dark), (byte)(192f * dark), (byte)(193f * dark), alpha);
				else if (coinStacks[0] > 0) Color = new Color((byte)(246f * dark), (byte)(138f * dark), (byte)(96f * dark), alpha);
			}
			else if (item.type != 3817)
			{
				Text = Language.GetTextValue("LegacyTooltip.51");
				Color = new Color((byte)(120f * dark), (byte)(120f * dark), (byte)(120f * dark), alpha);
			}
		}

		public override Vector2 GetSize() => ChatManager.GetStringSize(Main.fontMouseText, Text, Vector2.One);

		public override void Draw(SpriteBatch spriteBatch, float maxWidth)
		{
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, Text, Position, Color, 0f, Vector2.Zero, Vector2.One);
		}
	}
}