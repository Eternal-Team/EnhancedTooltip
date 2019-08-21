using BaseLibrary;
using EnhancedTooltip.Tooltip;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace EnhancedTooltip
{
	// note: display available ammos for weapon?
	// note: icons for equipable, consumable, material...
	public class EnhancedTooltipItem : GlobalItem
	{
		private static EnhancedTooltipConfig Config => EnhancedTooltip.Instance.GetConfig<EnhancedTooltipConfig>();

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			// note: visual representation by coloring the header background?
			TooltipLine itemName = tooltips.FirstOrDefault(line => line.mod == "Terraria" && line.Name == "ItemName");
			if (itemName != null && Config.ShowMaxStack)
			{
				string text = item.AffixName();
				if (item.stack > 1) text = $"{text} ({item.stack}/{item.maxStack})";
				itemName.text = text;
			}

			if (Config.FavoriteUseIcon)
			{
				tooltips.RemoveAll(tooltip => tooltip.mod == "Terraria" && tooltip.Name == "Favorite");
				tooltips.RemoveAll(tooltip => tooltip.mod == "Terraria" && tooltip.Name == "FavoriteDesc");
			}

			if (item.modItem != null && Config.ShowModName) tooltips.Add(new TooltipLine(mod, "ModName", $"Added by {item.modItem.mod.DisplayName}"));

			for (int i = 0; i < tooltips.Count; i++)
			{
				TooltipLine line = tooltips[i];
				if (line.Name.Contains("Tooltip")) tooltips.MoveToEnd(i);
			}

			int index = tooltips.FindLastIndex(line => line.mod == "Terraria" && (line.Name == "Price" || line.Name == "SpecialPrice"));
			if (index != -1) tooltips.MoveToEnd(index);
		}
		
		public static bool PreDrawTooltip(Item item, ReadOnlyCollection<TwoColumnLine> lines, int x, int y, float width, float height)
		{
			foreach (TwoColumnLine line in lines)
			{
				if (lines.Any(l => l.Name.Contains("Tooltip")) && (line.Name.Contains("Tooltip") || line.Name == "Price" || line.Name == "SpecialPrice")) line.Y += 8;
			}

			Main.spriteBatch.DrawPanel(new Rectangle(x, y, (int)width, (int)height), Config.TooltipPanelColor);

			float firstLineHeight = lines[0].GetSize().Y;

			if (lines.Count > 1) Main.spriteBatch.Draw(ModContent.GetTexture("Terraria/UI/Divider"), new Vector2(x, y + firstLineHeight + 8f), null, Color.White, 0f, Vector2.Zero, new Vector2(width / 8f, 1f), SpriteEffects.None, 0f);

			if (item.favorited && Config.FavoriteUseIcon) Main.spriteBatch.Draw(Main.cursorTextures[3], new Vector2(x + width - 16f, y + firstLineHeight * 0.5f + 4f), null, Color.White, 0f, Main.cursorTextures[3].Size() * 0.5f, 1f, SpriteEffects.None, 0f);

			return true;

			//return ItemLoader.PreDrawTooltip(item, lines.Select(twoColumnLine => (TooltipLine)twoColumnLine.line).ToList().AsReadOnly(), ref x, ref y);
		}

		public static void PostDrawTooltip(Item item, ReadOnlyCollection<TwoColumnLine> lines, int x, int y, float width, float height)
		{
			TwoColumnLine firstTooltip = lines.FirstOrDefault(line => line.Name.Contains("Tooltip"));
			if (firstTooltip != null)
			{
				Main.spriteBatch.Draw(ModContent.GetTexture("Terraria/UI/Divider"), new Vector2(x, firstTooltip.Y), null, Color.White, 0f, Vector2.Zero, new Vector2(width / 8f, 1f), SpriteEffects.None, 0f);
			}
		}

		public static void ModifyTooltipMetrics(ReadOnlyCollection<TwoColumnLine> lines, ref int X, ref int Y, ref float width, ref float height)
		{
			X -= 8;
			Y -= 8;
			width += 16f;
			height += 16f + (lines.Any(line => line.Name.Contains("Tooltip")) ? 8f : 0f);
		}
	}
}