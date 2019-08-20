using BaseLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace EnhancedTooltip
{
	public class EnhancedTooltipItem : GlobalItem
	{
		private static EnhancedTooltipConfig Config => EnhancedTooltip.Instance.GetConfig<EnhancedTooltipConfig>();
		private static int X, Y;

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (Config.FavoriteUseIcon)
			{
				tooltips.RemoveAll(tooltip => tooltip.mod == "Terraria" && tooltip.Name == "Favorite");
				tooltips.RemoveAll(tooltip => tooltip.mod == "Terraria" && tooltip.Name == "FavoriteDesc");
			}

			//bool hasAdditionalInfo = item.favorited || item.createTile > 0 || item.createWall > 0 || item.ammo > 0 || item.consumable || item.material || item.social || item.accessory || item.questItem || item.vanity || item.expert;

			//if (!ETPlayer.MoreInfo)
			//{
			//	tooltips.RemoveAll(x => x.mod == "Terraria" && x.Name.StartsWith("Favorite"));
			//	tooltips.RemoveAll(x => x.mod == "Terraria" && x.Name.StartsWith("Social"));
			//	tooltips.RemoveAll(x => x.mod == "Terraria" && x.Name == "Placeable");
			//	tooltips.RemoveAll(x => x.mod == "Terraria" && x.Name == "Consumable");
			//	tooltips.RemoveAll(x => x.mod == "Terraria" && x.Name == "Equipable");
			//	tooltips.RemoveAll(x => x.mod == "Terraria" && x.Name == "Material");
			//	tooltips.RemoveAll(x => x.mod == "Terraria" && x.Name == "Expert");
			//	tooltips.RemoveAll(x => x.mod == "Terraria" && x.Name == "Vanity");
			//	tooltips.RemoveAll(x => x.mod == "Terraria" && x.Name == "Quest");
			//	tooltips.RemoveAll(x => x.mod == "Terraria" && x.Name == "Ammo");

			//	if (hasAdditionalInfo)
			//		tooltips.Add(new TooltipLine(mod, "MoreInfo", $"Press [c/f2ff82:{GetHotkeyValue(mod.Name + ": Show more info")}] to display additional info"));
			//}

			//if (!ETPlayer.PrefixInfo)
			//{
			//	tooltips.RemoveAll(x => x.mod == "Terraria" && x.Name.StartsWith("Prefix"));
			//	if (item.prefix > 0)
			//		tooltips.Add(new TooltipLine(mod, "PrefixInfo", $"Press [c/83fcec:{GetHotkeyValue(mod.Name + ": Show prefix info")}] to display prefix info"));
			//}

			//if (!ETPlayer.EMMInfo)
			//{
			//	IEnumerable<TooltipLine> tooltipLines = tooltips.Where(x => x.Name.StartsWith("Modifier:"));
			//	if (tooltipLines.Any())
			//	{
			//		tooltips.RemoveAll(x => x.Name.StartsWith("Modifier:"));
			//		tooltips.Add(new TooltipLine(mod, "EMMInfo", $"Press [c/3bce01:{GetHotkeyValue(mod.Name + ": Show modifier info")}] to display modifier info"));
			//	}
			//}
		}

		//private static List<TwoColumnLine> twoColumnLines = new List<TwoColumnLine>();

		public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
		{
			if (!line.oneDropLogo)
			{
				if (line.mod == "Terraria" && line.Name == "ItemName") line.baseScale = new Vector2(Config.ItemNameScale);
				else if (line.Name.Contains("Tooltip")) line.baseScale = new Vector2(Config.TooltipTextScale);
				else line.baseScale = new Vector2(Config.OtherTextScale);

				float lineHeight = Main.fontMouseText.MeasureString(line.text).Y;
				yOffset = (int)(lineHeight * line.baseScale.Y - lineHeight);

				//TwoColumnLine twoColumnLine = GetLine(line, item);
				//twoColumnLine.position = new Vector2(line.X, line.Y);
				//twoColumnLine.Draw(Main.spriteBatch, box);
				//twoColumnLines.Add(twoColumnLine);
			}

			return true;
		}

		//public TwoColumnLine GetLine(DrawableTooltipLine line, Item item)
		//{
		//	switch (line.Name)
		//	{
		//		case "Damage":
		//			return DamageTooltip.GetLine(line, item);
		//		case "CritChance":
		//			int itemCrit = item.crit;
		//			Player player = Main.LocalPlayer;
		//			if (item.melee) itemCrit = player.meleeCrit - player.HeldItem.crit + itemCrit;
		//			else if (item.ranged) itemCrit = player.rangedCrit - player.HeldItem.crit + itemCrit;
		//			else if (item.magic) itemCrit = player.magicCrit - player.HeldItem.crit + itemCrit;
		//			return new TwoColumnLine("Critical strike chance:", $"{itemCrit}%", line.color, DoubleLerp(Color.Red, Color.Yellow, Color.Lime, item.crit / 100f), line.baseScale);
		//		case "Speed":
		//			return SpeedTooltip.GetLine(line, item);
		//		case "Knockback":
		//			return KnockbackTooltip.GetLine(line, item);
		//		case "FishingPower":
		//			return new TwoColumnLine("Fishing power:", $"{item.fishingPole}%", line.color, DoubleLerp(Color.Red, Color.Yellow, Color.Lime, item.fishingPole / (float)EnhancedTooltip.Instance.maxPowerRod), line.baseScale);
		//		case "BaitPower":
		//			return new TwoColumnLine("Bait power:", $"{item.bait}%", line.color, DoubleLerp(Color.Red, Color.Yellow, Color.Lime, item.bait / (float)EnhancedTooltip.Instance.maxPowerBait), line.baseScale);
		//		case "Defense":
		//			return new TwoColumnLine("Defense:", $"{item.defense}", line.color, DoubleLerp(Color.Red, Color.Yellow, Color.Lime, item.defense / (float)EnhancedTooltip.Instance.maxDefense), line.baseScale);
		//		case "PickPower":
		//			return new TwoColumnLine("Pickaxe power:", $"{item.pick}%", line.color, DoubleLerp(Color.Red, Color.Yellow, Color.Lime, item.pick / (float)EnhancedTooltip.Instance.maxPowerPick), line.baseScale);
		//		case "AxePower":
		//			return new TwoColumnLine("Axe power:", $"{item.axe}%", line.color, DoubleLerp(Color.Red, Color.Yellow, Color.Lime, item.axe / (float)EnhancedTooltip.Instance.maxPowerAxe), line.baseScale);
		//		case "HammerPower":
		//			return new TwoColumnLine("Hammer power:", $"{item.hammer}%", line.color, DoubleLerp(Color.Red, Color.Yellow, Color.Lime, item.hammer / (float)EnhancedTooltip.Instance.maxPowerHammer), line.baseScale);
		//		case "TileBoost":
		//			return new TwoColumnLine("Range:", $"{(item.tileBoost > 0 ? "+" : "")}{item.tileBoost}", line.color, item.tileBoost > 0 ? Color.Lime : Color.Red, line.baseScale);
		//		case "HealLife":
		//			return new TwoColumnLine("Heals: ", $"{item.healLife} life", line.color, Color.Lime, line.baseScale);
		//		case "HealMana":
		//			return new TwoColumnLine("Restores: ", $"{item.healMana} mana", line.color, Color.Lime, line.baseScale);
		//		case "UseMana":
		//			return new TwoColumnLine("Consumes: ", $"{item.mana} mana", line.color, Color.Lime, line.baseScale);
		//		case "BuffTime":
		//			if (item.type >= 71 && item.type <= 74) return null;
		//			return BuffTimeTooltip.GetLine(line, item);
		//		case "Price":
		//			return PriceTooltip.GetLine(line, item);
		//		case "MoreInfo":
		//			TwoColumnLine shiftLine = new TwoColumnLine(line.text, line.color, line.baseScale);
		//			shiftLine.measureableTextLeft = ChatManager.Regexes.Format.Replace(line.text, GetHotkeyValue(mod.Name + ": Show more info"));
		//			return shiftLine;
		//		case "PrefixInfo":
		//			TwoColumnLine controlLine = new TwoColumnLine(line.text, line.color, line.baseScale);
		//			controlLine.measureableTextLeft = ChatManager.Regexes.Format.Replace(line.text, GetHotkeyValue(mod.Name + ": Show prefix info"));
		//			return controlLine;
		//		default: return new TwoColumnLine(line.text, line.color, line.baseScale);
		//	}
		//}

		//public static void PreDrawTooltip(List<DrawableTooltipLine> lines, int x, int y)
		//{
		//	float width = 0f, height = 0f;
		//	foreach (Vector2 size in lines.Select(line => ChatManager.GetStringSize(Main.fontMouseText, line.text, line.baseScale, line.maxWidth)))
		//	{
		//		if (size.X > width) width = size.X;
		//		height += size.Y;
		//	}

		//	Main.spriteBatch.DrawPanel(new Rectangle(x - 8, y - 8, (int)(width + 16), (int)(height + 16)), EnhancedTooltip.Instance.GetConfig<EnhancedTooltipConfig>().TooltipPanelColor);
		//}

		public override bool PreDrawTooltip(Item item, ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y)
		{
			X = x;
			Y = y;
			return false;
		}

		public override void PostDrawTooltip(Item item, ReadOnlyCollection<DrawableTooltipLine> lines)
		{
			GetTooltipSize(item, lines, out float width, out float height);

			Main.spriteBatch.DrawPanel(new Rectangle(X - 8, Y - 8, (int)width, (int)height), Config.TooltipPanelColor);

			float firstLineHeight = Main.fontMouseText.MeasureString(lines.First().text).Y;

			Main.spriteBatch.Draw(ModContent.GetTexture("Terraria/UI/Divider"), new Vector2(X - 8f, Y + firstLineHeight), null, Color.White, 0f, Vector2.Zero, new Vector2(width / 8f, 1f), SpriteEffects.None, 0f);

			if (item.favorited && Config.FavoriteUseIcon) Main.spriteBatch.Draw(Main.cursorTextures[3], new Vector2(X + width - 24f, Y + firstLineHeight * 0.5f - 4f), null, Color.White, 0f, Main.cursorTextures[3].Size() * 0.5f, 1f, SpriteEffects.None, 0f);

			foreach (DrawableTooltipLine line in lines)
			{
				float offset = 0f;
				if (line.Name != "ItemName") offset = 8f;
				ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, line.font, line.text, new Vector2(line.X, line.Y + offset), line.overrideColor ?? line.color, line.rotation, line.origin, line.baseScale, line.maxWidth, line.spread);
			}
		}

		private static void GetTooltipSize(Item item, ReadOnlyCollection<DrawableTooltipLine> lines, out float width, out float height)
		{
			width = 0f;
			height = 0f;

			foreach (Vector2 size in lines.Select(line => ChatManager.GetStringSize(Main.fontMouseText, line.text, line.baseScale, line.maxWidth) * new Vector2(1f, line.baseScale.Y)))
			{
				if (size.X > width) width = size.X;
				height += size.Y;
			}

			width += 16f;
			height += 24f;

			if (item.favorited && Config.FavoriteUseIcon && width < Main.fontMouseText.MeasureString(lines.First().text).X + 24f) width += 24f;
		}
	}
}