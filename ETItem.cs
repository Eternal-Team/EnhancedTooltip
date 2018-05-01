using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EnhancedTooltip.Tooltip;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using static TheOneLibrary.Utils.Utility;

namespace EnhancedTooltip
{
	public class ETItem : GlobalItem
	{
		public override bool InstancePerEntity => false;

		public override bool CloneNewInstances => false;

		public override GlobalItem Clone(Item item, Item itemClone)
		{
			ETItem clone = (ETItem)base.Clone();
			return clone;
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			TooltipLine itemName = tooltips.FirstOrDefault(x => x.Name == "ItemName");
			if (itemName != null && string.IsNullOrEmpty(itemName.text)) itemName.text = "MISSING ITEM NAME";
			else if (itemName == null) tooltips.Insert(0, new TooltipLine(mod, "ItemName", "null"));

			bool hasAdditionalInfo = item.favorited || item.createTile > 0 || item.createWall > 0 || item.ammo > 0 || item.consumable || item.material || item.social || item.accessory || item.questItem || item.vanity || item.expert;

			if (!ETPlayer.MoreInfo)
			{
				tooltips.RemoveAll(x => x.mod == "Terraria" && x.Name.StartsWith("Favorite"));
				tooltips.RemoveAll(x => x.mod == "Terraria" && x.Name.StartsWith("Social"));
				tooltips.RemoveAll(x => x.mod == "Terraria" && x.Name == "Placeable");
				tooltips.RemoveAll(x => x.mod == "Terraria" && x.Name == "Consumable");
				tooltips.RemoveAll(x => x.mod == "Terraria" && x.Name == "Equipable");
				tooltips.RemoveAll(x => x.mod == "Terraria" && x.Name == "Material");
				tooltips.RemoveAll(x => x.mod == "Terraria" && x.Name == "Expert");
				tooltips.RemoveAll(x => x.mod == "Terraria" && x.Name == "Vanity");
				tooltips.RemoveAll(x => x.mod == "Terraria" && x.Name == "Quest");
				tooltips.RemoveAll(x => x.mod == "Terraria" && x.Name == "Ammo");

				if (hasAdditionalInfo) tooltips.Add(new TooltipLine(mod, "MoreInfo", $"Press [c/f2ff82:{GetHotkeyValue(mod.Name + ": Show more info")}] to display additional info"));
			}

			if (!ETPlayer.PrefixInfo)
			{
				tooltips.RemoveAll(x => x.mod == "Terraria" && x.Name.StartsWith("Prefix"));
				if (item.prefix > 0) tooltips.Add(new TooltipLine(mod, "PrefixInfo", $"Press [c/83fcec:{GetHotkeyValue(mod.Name + ": Show prefix info")}] to display prefix info"));
			}
		}

		private static Rectangle box;
		private static List<TwoColumnLine> twoColumnLines = new List<TwoColumnLine>();

		public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
		{
			if (!line.oneDropLogo)
			{
				if (line.mod == "Terraria" && line.Name == "ItemName") line.baseScale = Vector2.One;
				else if (line.mod == "Terraria" && line.Name.StartsWith("Tooltip")) line.baseScale = new Vector2(0.8f);
				else line.baseScale = new Vector2(0.9f);

				TwoColumnLine twoColumnLine = GetLine(line, item);
				twoColumnLine.position = new Vector2(line.X, line.Y);
				twoColumnLine.Draw(Main.spriteBatch, box);
				twoColumnLines.Add(twoColumnLine);
			}

			return false;
		}

		public TwoColumnLine GetLine(DrawableTooltipLine line, Item item)
		{
			switch (line.Name)
			{
				case "Damage":
					return DamageTooltip.GetLine(line, item);
				case "CritChance":
					int itemCrit = item.crit;
					Player player = Main.LocalPlayer;
					if (item.melee) itemCrit = player.meleeCrit - player.HeldItem.crit + itemCrit;
					else if (item.ranged) itemCrit = player.rangedCrit - player.HeldItem.crit + itemCrit;
					else if (item.magic) itemCrit = player.magicCrit - player.HeldItem.crit + itemCrit;
					return new TwoColumnLine("Critical strike chance:", $"{itemCrit}%", line.color, DoubleLerp(Color.Red, Color.Yellow, Color.Lime, item.crit / 100f), line.baseScale);
				case "Speed":
					return SpeedTooltip.GetLine(line, item);
				case "Knockback":
					return KnockbackTooltip.GetLine(line, item);
				case "FishingPower":
					return new TwoColumnLine("Fishing power:", $"{item.fishingPole}%", line.color, DoubleLerp(Color.Red, Color.Yellow, Color.Lime, item.fishingPole / (float)EnhancedTooltip.Instance.maxPowerRod), line.baseScale);
				case "BaitPower":
					return new TwoColumnLine("Bait power:", $"{item.bait}%", line.color, DoubleLerp(Color.Red, Color.Yellow, Color.Lime, item.bait / (float)EnhancedTooltip.Instance.maxPowerBait), line.baseScale);
				case "Defense":
					return new TwoColumnLine("Defense:", $"{item.defense}", line.color, DoubleLerp(Color.Red, Color.Yellow, Color.Lime, item.defense / (float)EnhancedTooltip.Instance.maxDefense), line.baseScale);
				case "PickPower":
					return new TwoColumnLine("Pickaxe power:", $"{item.pick}%", line.color, DoubleLerp(Color.Red, Color.Yellow, Color.Lime, item.pick / (float)EnhancedTooltip.Instance.maxPowerPick), line.baseScale);
				case "AxePower":
					return new TwoColumnLine("Axe power:", $"{item.axe}%", line.color, DoubleLerp(Color.Red, Color.Yellow, Color.Lime, item.axe / (float)EnhancedTooltip.Instance.maxPowerAxe), line.baseScale);
				case "HammerPower":
					return new TwoColumnLine("Hammer power:", $"{item.hammer}%", line.color, DoubleLerp(Color.Red, Color.Yellow, Color.Lime, item.hammer / (float)EnhancedTooltip.Instance.maxPowerHammer), line.baseScale);
				case "TileBoost":
					return new TwoColumnLine("Range:", $"{(item.tileBoost > 0 ? "+" : "")}{item.tileBoost}", line.color, item.tileBoost > 0 ? Color.Lime : Color.Red, line.baseScale);
				case "HealLife":
					return new TwoColumnLine("Heals: ", $"{item.healLife} life", line.color, Color.Lime, line.baseScale);
				case "HealMana":
					return new TwoColumnLine("Restores: ", $"{item.healMana} mana", line.color, Color.Lime, line.baseScale);
				case "UseMana":
					return new TwoColumnLine("Consumes: ", $"{item.mana} mana", line.color, Color.Lime, line.baseScale);
				case "BuffTime":
					if (item.type >= 71 && item.type <= 74) return null;
					return BuffTimeTooltip.GetLine(line, item);
				case "Price":
					return PriceTooltip.GetLine(line, item);
				case "MoreInfo":
					TwoColumnLine shiftLine = new TwoColumnLine(line.text, line.color, line.baseScale);
					shiftLine.measureableTextLeft = ChatManager.Regexes.Format.Replace(line.text, GetHotkeyValue(mod.Name + ": Show more info"));
					return shiftLine;
				case "PrefixInfo":
					TwoColumnLine controlLine = new TwoColumnLine(line.text, line.color, line.baseScale);
					controlLine.measureableTextLeft = ChatManager.Regexes.Format.Replace(line.text, GetHotkeyValue(mod.Name + ": Show prefix info"));
					return controlLine;
				default: return new TwoColumnLine(line.text, line.color, line.baseScale);
			}
		}

		public override bool PreDrawTooltip(Item item, ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y)
		{
			box.X = x - 12;
			box.Y = y - 12;

			if (twoColumnLines.Any())
			{
				float width = twoColumnLines.Select(z => Main.fontMouseText.MeasureString(ExtractText(z.textLeft)).X * z.scaleL.X + Main.fontMouseText.MeasureString(ExtractText(z.textRight)).X * z.scaleR.X).OrderByDescending(z => z).First();
				float height = twoColumnLines.Sum(z => MathHelper.Max(Main.fontMouseText.MeasureString(z.textLeft).Y, Main.fontMouseText.MeasureString(z.textRight).Y));

				box.Width = (int)width + 30;
				box.Height = (int)height + 20;

				Main.spriteBatch.DrawPanel(box, TheOneLibrary.TheOneLibrary.backgroundTexture, new Color(63, 82, 151) * 0.9f);
				Main.spriteBatch.DrawPanel(box, TheOneLibrary.TheOneLibrary.borderTexture, Color.Black);
			}

			twoColumnLines.Clear();

			return true;
		}
	}
}