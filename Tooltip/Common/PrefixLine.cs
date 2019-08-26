using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace EnhancedTooltip.Tooltip.Common
{
	public class PrefixLine : BaseTooltipLine
	{
		public List<string> Text = new List<string>();
		public List<Color> Color = new List<Color>();

		public PrefixLine(Item item)
		{
			if (Main.cpItem == null || Main.cpItem.netID != item.netID)
			{
				Main.cpItem = new Item();
				Main.cpItem.netDefaults(item.netID);
			}

			float dark = Main.mouseTextColor / 255f;
			byte a = Main.mouseTextColor;

			Color badColor = new Color((byte)(190f * dark), (byte)(120f * dark), (byte)(120f * dark), a);
			Color goodColor = new Color((byte)(120f * dark), (byte)(190f * dark), (byte)(120f * dark), a);

			if (Main.cpItem.damage != item.damage)
			{
				double num12 = item.damage - (float)Main.cpItem.damage;
				num12 = num12 / Main.cpItem.damage * 100.0;
				num12 = Math.Round(num12);
				Text.Add(num12 > 0.0 ? "+" + num12 + Language.GetTextValue("LegacyTooltip.39") : num12 + Language.GetTextValue("LegacyTooltip.39"));
				Color.Add(num12 < 0.0 ? badColor : goodColor);
			}

			if (Main.cpItem.useAnimation != item.useAnimation)
			{
				double num13 = item.useAnimation - (float)Main.cpItem.useAnimation;
				num13 = num13 / Main.cpItem.useAnimation * 100.0;
				num13 = Math.Round(num13);
				num13 *= -1.0;
				Text.Add(num13 > 0.0 ? "+" + num13 + Language.GetTextValue("LegacyTooltip.40") : num13 + Language.GetTextValue("LegacyTooltip.40"));
				Color.Add(num13 < 0.0 ? badColor : goodColor);
			}

			if (Main.cpItem.crit != item.crit)
			{
				double num14 = item.crit - (float)Main.cpItem.crit;
				Text.Add(num14 > 0.0 ? "+" + num14 + Language.GetTextValue("LegacyTooltip.41") : num14 + Language.GetTextValue("LegacyTooltip.41"));
				Color.Add(num14 < 0.0 ? badColor : goodColor);
			}

			if (Main.cpItem.mana != item.mana)
			{
				double num15 = item.mana - (float)Main.cpItem.mana;
				num15 = num15 / Main.cpItem.mana * 100.0;
				num15 = Math.Round(num15);
				Text.Add(num15 > 0.0 ? "+" + num15 + Language.GetTextValue("LegacyTooltip.42") : num15 + Language.GetTextValue("LegacyTooltip.42"));
				Color.Add(num15 > 0.0 ? badColor : goodColor);
			}

			if (Main.cpItem.scale != item.scale)
			{
				double num16 = item.scale - Main.cpItem.scale;
				num16 = num16 / Main.cpItem.scale * 100.0;
				num16 = Math.Round(num16);
				Text.Add(num16 > 0.0 ? "+" + num16 + Language.GetTextValue("LegacyTooltip.43") : num16 + Language.GetTextValue("LegacyTooltip.43"));
				Color.Add(num16 < 0.0 ? badColor : goodColor);
			}

			if (Main.cpItem.shootSpeed != item.shootSpeed)
			{
				double num17 = item.shootSpeed - Main.cpItem.shootSpeed;
				num17 = num17 / Main.cpItem.shootSpeed * 100.0;
				num17 = Math.Round(num17);
				Text.Add(num17 > 0.0 ? "+" + num17 + Language.GetTextValue("LegacyTooltip.44") : num17 + Language.GetTextValue("LegacyTooltip.44"));
				Color.Add(num17 < 0.0 ? badColor : goodColor);
			}

			float knockback = item.knockBack;
			if (item.summon) knockback += Main.LocalPlayer.minionKB;
			if (Main.LocalPlayer.magicQuiver && item.useAmmo == AmmoID.Arrow || item.useAmmo == AmmoID.Stake) knockback = (int)(knockback * 1.1f);
			if (Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].type == 3106 && item.type == 3106) knockback += knockback * (1f - Main.LocalPlayer.stealth);

			ItemLoader.GetWeaponKnockback(item, Main.LocalPlayer, ref knockback);
			PlayerHooks.GetWeaponKnockback(Main.LocalPlayer, item, ref knockback);
			if (Main.cpItem.knockBack != knockback)
			{
				double num18 = knockback - Main.cpItem.knockBack;
				num18 = num18 / Main.cpItem.knockBack * 100.0;
				num18 = Math.Round(num18);
				Text.Add(num18 > 0.0 ? "+" + num18 + Language.GetTextValue("LegacyTooltip.45") : num18 + Language.GetTextValue("LegacyTooltip.45"));
				Color.Add(num18 < 0.0 ? badColor : goodColor);
			}

			if (item.prefix == 62)
			{
				Text.Add("+1" + Language.GetTextValue("LegacyTooltip.25"));
				Color.Add(goodColor);
			}

			if (item.prefix == 63)
			{
				Text.Add("+2" + Language.GetTextValue("LegacyTooltip.25"));
				Color.Add(goodColor);
			}

			if (item.prefix == 64)
			{
				Text.Add("+3" + Language.GetTextValue("LegacyTooltip.25"));
				Color.Add(goodColor);
			}

			if (item.prefix == 65)
			{
				Text.Add("+4" + Language.GetTextValue("LegacyTooltip.25"));
				Color.Add(goodColor);
			}

			if (item.prefix == 66)
			{
				Text.Add("+20 " + Language.GetTextValue("LegacyTooltip.31"));
				Color.Add(goodColor);
			}

			if (item.prefix == 67)
			{
				Text.Add("+2" + Language.GetTextValue("LegacyTooltip.5"));
				Color.Add(goodColor);
			}

			if (item.prefix == 68)
			{
				Text.Add("+4" + Language.GetTextValue("LegacyTooltip.5"));
				Color.Add(goodColor);
			}

			if (item.prefix == 69)
			{
				Text.Add("+1" + Language.GetTextValue("LegacyTooltip.39"));
				Color.Add(goodColor);
			}

			if (item.prefix == 70)
			{
				Text.Add("+2" + Language.GetTextValue("LegacyTooltip.39"));
				Color.Add(goodColor);
			}

			if (item.prefix == 71)
			{
				Text.Add("+3" + Language.GetTextValue("LegacyTooltip.39"));
				Color.Add(goodColor);
			}

			if (item.prefix == 72)
			{
				Text.Add("+4" + Language.GetTextValue("LegacyTooltip.39"));
				Color.Add(goodColor);
			}

			if (item.prefix == 73)
			{
				Text.Add("+1" + Language.GetTextValue("LegacyTooltip.46"));
				Color.Add(goodColor);
			}

			if (item.prefix == 74)
			{
				Text.Add("+2" + Language.GetTextValue("LegacyTooltip.46"));
				Color.Add(goodColor);
			}

			if (item.prefix == 75)
			{
				Text.Add("+3" + Language.GetTextValue("LegacyTooltip.46"));
				Color.Add(goodColor);
			}

			if (item.prefix == 76)
			{
				Text.Add("+4" + Language.GetTextValue("LegacyTooltip.46"));
				Color.Add(goodColor);
			}

			if (item.prefix == 77)
			{
				Text.Add("+1" + Language.GetTextValue("LegacyTooltip.47"));
				Color.Add(goodColor);
			}

			if (item.prefix == 78)
			{
				Text.Add("+2" + Language.GetTextValue("LegacyTooltip.47"));
				Color.Add(goodColor);
			}

			if (item.prefix == 79)
			{
				Text.Add("+3" + Language.GetTextValue("LegacyTooltip.47"));
				Color.Add(goodColor);
			}

			if (item.prefix == 80)
			{
				Text.Add("+4" + Language.GetTextValue("LegacyTooltip.47"));
				Color.Add(goodColor);
			}
		}

		public override Vector2 GetSize()
		{
			Vector2 size = Vector2.Zero;
			foreach (Vector2 measured in Text.Select(s => ChatManager.GetStringSize(Main.fontMouseText, s, Vector2.One)))
			{
				if (measured.X > size.X) size.X = measured.X;
				size.Y += measured.Y;
			}

			return size;
		}

		public override void Draw(SpriteBatch spriteBatch, float maxWidth)
		{
			float Y = Position.Y;
			for (var i = 0; i < Text.Count; i++)
			{
				string text = Text[i];
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, text, new Vector2(Position.X, Y), Color[i], 0f, Vector2.Zero, Vector2.One);
				Y += ChatManager.GetStringSize(Main.fontMouseText, text, Vector2.One).Y;
			}
		}
	}
}