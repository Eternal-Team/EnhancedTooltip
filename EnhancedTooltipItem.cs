using BaseLibrary;
using EnhancedTooltip.Tooltip;
using EnhancedTooltip.Tooltip.Common;
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

		public static void ModifyTooltips(Item item, List<BaseTooltipLine> tooltips)
		{
			if (Config.FavoriteUseIcon) tooltips.RemoveAll(tooltip => tooltip is FavoriteLine);

			if (item.modItem != null && Config.ShowModName) tooltips.Add(new TextLine { Text = $"Added by {item.modItem.mod.DisplayName}" });

			for (int i = 0; i < tooltips.Count; i++)
			{
				if (tooltips[i] is Tooltip.Common.TooltipLine) tooltips.MoveToEnd(i);
			}

			//int index = tooltips.FindLastIndex(line => line is PriceLine);
			//if (index != -1) tooltips.MoveToEnd(index);
		}

		public static bool PreDrawTooltip(Item item, ReadOnlyCollection<BaseTooltipLine> lines, int x, int y, float width, float height)
		{
			Main.spriteBatch.DrawPanel(new Rectangle(x, y, (int)width, (int)height), Config.TooltipPanelColor);

			for (int i = 0; i < lines.Count; i++)
			{
				if (i > 0) lines[i].Position.Y += 8f;
			}

			return true;
		}

		public static void PostDrawTooltip(Item item, ReadOnlyCollection<BaseTooltipLine> lines, int x, int y, float width, float height)
		{
			BaseTooltipLine itemName = lines.FirstOrDefault(line => line is ItemNameLine);
			if (itemName != null && lines.Count > 1)
			{
				Main.spriteBatch.Draw(ModContent.GetTexture("Terraria/UI/Divider"), new Vector2(x, y + itemName.GetSize().Y + 8f), null, Color.White, 0f, Vector2.Zero, new Vector2(width / 8f, 1f), SpriteEffects.None, 0f);
			}
		}

		public static void ModifyTooltipMetrics(ReadOnlyCollection<BaseTooltipLine> lines, ref int X, ref int Y, ref float width, ref float height)
		{
			X -= 8;
			Y -= 8;
			width += 16f;
			height += 16f + (lines.Any(line => line is Tooltip.Common.TooltipLine) ? 8f : 0f);
		}
	}
}