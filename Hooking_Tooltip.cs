using BaseLibrary;
using EnhancedTooltip.Tooltip;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EnhancedTooltip
{
	internal static partial class Hooking
	{
		private static FieldInfo tooltipDistanceInfo;

		private static void DrawTooltip(byte diff, int X, int Y)
		{
			if (tooltipDistanceInfo == null) tooltipDistanceInfo = typeof(Main).GetField("toolTipDistance", Utility.defaultFlags);

			Player player = Main.LocalPlayer;
			Item item = Main.HoverItem;

			GetTooltips(player, item, out int lenght, out int oneDropLogoIndex, out Color color, out string[] tooltipNames, out string[] texts, out bool[] modifiers, out bool[] badModifiers);

			List<TooltipLine> lines = ItemLoader.ModifyTooltips(item, ref lenght, tooltipNames, ref texts, ref modifiers, ref badModifiers, ref oneDropLogoIndex, out var overrideColor);

			List<DrawableTooltipLine> drawableLines = lines.Select((x, i) => new DrawableTooltipLine(x, i, 0, 0, Color.White)).ToList();

			List<TwoColumnLine> twoColumnLines = drawableLines.Select(line => TwoColumnLine.CreateFromDrawableTooltipLine(item, line)).ToList();

			Vector2 size = Vector2.Zero;
			foreach (Vector2 vector in twoColumnLines.Select(line => line.GetSize()))
			{
				if (vector.X > size.X) size.X = vector.X;
				size.Y += vector.Y;
			}

			int tooltipDistance = (int)(tooltipDistanceInfo?.GetValue(null) ?? 0);
			X += tooltipDistance;
			Y += tooltipDistance;

			EnhancedTooltipItem.ModifyTooltipMetrics(ref X, ref Y, ref size.X, ref size.Y);

			if (X + size.X + 4f > Main.screenWidth) X = (int)(Main.screenWidth - size.X - 4f);
			if (Y + size.Y + 4f > Main.screenHeight) Y = (int)(Main.screenHeight - size.Y - 4f);

			// note: shouldn't continue drawing at all if returns false
			bool globalCanDraw = EnhancedTooltipItem.PreDrawTooltip(item, twoColumnLines.AsReadOnly(), X, Y, size.X, size.Y);

			float dark = Main.mouseTextColor / 255f;
			int alpha = Main.mouseTextColor;

			int yOffset = 0;
			for (int i = 0; i < twoColumnLines.Count; i++)
			{
				TwoColumnLine line = twoColumnLines[i];

				Color drawColor;
				line.X += X;
				line.Y += Y;

				if (line.OneDropLogo)
				{
					line.colorLeft = new Color(dark, dark, dark, dark);

					if (ItemLoader.PreDrawTooltipLine(item, line.line, ref yOffset) && globalCanDraw)
					{
						for (int j = 0; j < 5; j++)
						{
							int logoX = line.X;
							int logoY = line.Y;
							if (j == 0) logoX--;
							else if (j == 1) logoX++;
							else if (j == 2) logoY--;
							else if (j == 3) logoY++;

							drawColor = line.colorLeft;
							Main.spriteBatch.Draw(Main.oneDropLogo, new Vector2(logoX + 8f, logoY + 8f), null, j != 4 ? Color.Black : drawColor, line.Rotation, line.Origin, (line.scaleLeft.X + line.scaleLeft.Y) / 2f, SpriteEffects.None, 0f);
						}

						ItemLoader.PostDrawTooltipLine(item, line.line);
					}
				}
				else
				{
					Color baseColor = new Color(dark, dark, dark, dark);
					if (i == 0)
					{
						if (diff == 1) baseColor = new Color((byte)(Main.mcColor.R * dark), (byte)(Main.mcColor.G * dark), (byte)(Main.mcColor.B * dark), alpha);
						else if (diff == 2) baseColor = new Color((byte)(Main.hcColor.R * dark), (byte)(Main.hcColor.G * dark), (byte)(Main.hcColor.B * dark), alpha);
						else baseColor = GetRarityColor(item);
					}
					else if (modifiers[i]) baseColor = badModifiers[i] ? new Color((byte)(190f * dark), (byte)(120f * dark), (byte)(120f * dark), alpha) : new Color((byte)(120f * dark), (byte)(190f * dark), (byte)(120f * dark), alpha);
					else if (line.line.mod.Equals("Terraria") && line.line.Name.Equals("Price")) baseColor = color;

					line.colorLeft = baseColor;

					drawColor = baseColor;
					if (overrideColor[i].HasValue)
					{
						drawColor = overrideColor[i].Value * dark;
						line.colorLeft = drawColor;
					}

					if (ItemLoader.PreDrawTooltipLine(item, line.line, ref yOffset) && globalCanDraw) line.Draw(Main.spriteBatch, size.X);

					ItemLoader.PostDrawTooltipLine(item, line.line);
				}

				Y += (int)(line.GetSize().Y + yOffset);
			}

			ItemLoader.PostDrawTooltip(item, drawableLines.AsReadOnly());
		}

		private static void GetTooltips(Player player, Item item, out int lenght, out int oneDropLogoIndex, out Color color, out string[] tooltipNames, out string[] texts, out bool[] modifiers, out bool[] badModifiers)
		{
			color = new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor);
			oneDropLogoIndex = -1;

			float knockBack = item.knockBack;
			float knockbackMultiplier = 1f;

			if (item.melee && player.kbGlove) knockbackMultiplier += 1f;
			if (player.kbBuff) knockbackMultiplier += 0.5f;
			if (knockbackMultiplier != 1f) item.knockBack *= knockbackMultiplier;

			if (item.ranged && player.shroomiteStealth) item.knockBack *= 1f + (1f - player.stealth) * 0.5f;

			int numLines = 20 + item.ToolTip?.Lines ?? 0;
			lenght = 1;

			tooltipNames = new string[numLines];
			texts = new string[numLines];
			modifiers = new bool[numLines];
			badModifiers = new bool[numLines];

			for (int i = 0; i < numLines; i++)
			{
				modifiers[i] = false;
				badModifiers[i] = false;
			}

			texts[0] = item.HoverName;
			tooltipNames[0] = "ItemName";
			if (item.favorited)
			{
				texts[lenght++] = Language.GetTextValue("LegacyTooltip.56");
				tooltipNames[lenght - 1] = "Favorite";
				texts[lenght++] = Language.GetTextValue("LegacyTooltip.57");
				tooltipNames[lenght - 1] = "FavoriteDesc";
			}

			if (item.social)
			{
				texts[lenght] = Language.GetTextValue("LegacyTooltip.0");
				tooltipNames[lenght] = "Social";
				lenght++;
				texts[lenght] = Language.GetTextValue("LegacyTooltip.1");
				tooltipNames[lenght] = "SocialDesc";
				lenght++;
			}
			else
			{
				if (item.damage > 0 && (!item.notAmmo || item.useStyle > 0) && (item.type < 71 || item.type > 74 || player.HasItem(905)))
				{
					LocalizedText tip;
					if (item.melee) tip = Language.GetText("LegacyTooltip.2");
					else if (item.ranged) tip = Language.GetText("LegacyTooltip.3");
					else if (item.magic) tip = Language.GetText("LegacyTooltip.4");
					else if (item.thrown) tip = Language.GetText("LegacyTooltip.58");
					else if (item.summon) tip = Language.GetText("LegacyTooltip.53");
					else tip = Language.GetText("LegacyTooltip.55");
					int damage = player.GetWeaponDamage(item);
					if (item.type == 3829 || item.type == 3830 || item.type == 3831) damage *= 3;
					texts[lenght] = string.Concat(damage, tip.Value);

					tooltipNames[lenght] = "Damage";
					lenght++;
					if (item.melee)
					{
						int num7 = player.meleeCrit - player.HeldItem.crit + item.crit;
						ItemLoader.GetWeaponCrit(item, player, ref num7);
						PlayerHooks.GetWeaponCrit(player, item, ref num7);
						texts[lenght] = num7 + Language.GetTextValue("LegacyTooltip.5");
						tooltipNames[lenght] = "CritChance";
						lenght++;
					}
					else if (item.ranged)
					{
						int num8 = player.rangedCrit - player.HeldItem.crit + item.crit;
						ItemLoader.GetWeaponCrit(item, player, ref num8);
						PlayerHooks.GetWeaponCrit(player, item, ref num8);
						texts[lenght] = num8 + Language.GetTextValue("LegacyTooltip.5");
						tooltipNames[lenght] = "CritChance";
						lenght++;
					}
					else if (item.magic)
					{
						int num9 = player.magicCrit - player.HeldItem.crit + item.crit;
						ItemLoader.GetWeaponCrit(item, player, ref num9);
						PlayerHooks.GetWeaponCrit(player, item, ref num9);
						texts[lenght] = num9 + Language.GetTextValue("LegacyTooltip.5");
						tooltipNames[lenght] = "CritChance";
						lenght++;
					}
					else if (item.thrown)
					{
						int num10 = player.thrownCrit - player.HeldItem.crit + item.crit;
						ItemLoader.GetWeaponCrit(item, player, ref num10);
						PlayerHooks.GetWeaponCrit(player, item, ref num10);
						texts[lenght] = num10 + Language.GetTextValue("LegacyTooltip.5");
						tooltipNames[lenght] = "CritChance";
						lenght++;
					}
					else if (!item.summon) // crit tooltip for fully custom classes
					{
						int crit = item.crit;
						ItemLoader.GetWeaponCrit(item, player, ref crit);
						PlayerHooks.GetWeaponCrit(player, item, ref crit);
						texts[lenght] = crit + Language.GetTextValue("LegacyTooltip.5");
						tooltipNames[lenght] = "CritChance";
						lenght++;
					}

					if (item.useStyle > 0 && !item.summon)
					{
						if (item.useAnimation <= 8)
						{
							texts[lenght] = Language.GetTextValue("LegacyTooltip.6");
						}
						else if (item.useAnimation <= 20)
						{
							texts[lenght] = Language.GetTextValue("LegacyTooltip.7");
						}
						else if (item.useAnimation <= 25)
						{
							texts[lenght] = Language.GetTextValue("LegacyTooltip.8");
						}
						else if (item.useAnimation <= 30)
						{
							texts[lenght] = Language.GetTextValue("LegacyTooltip.9");
						}
						else if (item.useAnimation <= 35)
						{
							texts[lenght] = Language.GetTextValue("LegacyTooltip.10");
						}
						else if (item.useAnimation <= 45)
						{
							texts[lenght] = Language.GetTextValue("LegacyTooltip.11");
						}
						else if (item.useAnimation <= 55)
						{
							texts[lenght] = Language.GetTextValue("LegacyTooltip.12");
						}
						else
						{
							texts[lenght] = Language.GetTextValue("LegacyTooltip.13");
						}

						tooltipNames[lenght] = "Speed";
						lenght++;
					}

					float num11 = item.knockBack;
					if (item.summon)
					{
						num11 += player.minionKB;
					}

					if (player.magicQuiver && item.useAmmo == AmmoID.Arrow || item.useAmmo == AmmoID.Stake)
					{
						num11 = (int)(num11 * 1.1f);
					}

					if (player.inventory[player.selectedItem].type == 3106 && item.type == 3106)
					{
						num11 += num11 * (1f - player.stealth);
					}

					ItemLoader.GetWeaponKnockback(item, player, ref num11);
					PlayerHooks.GetWeaponKnockback(player, item, ref num11);
					if (num11 == 0f)
					{
						texts[lenght] = Language.GetTextValue("LegacyTooltip.14");
					}
					else if (num11 <= 1.5)
					{
						texts[lenght] = Language.GetTextValue("LegacyTooltip.15");
					}
					else if (num11 <= 3f)
					{
						texts[lenght] = Language.GetTextValue("LegacyTooltip.16");
					}
					else if (num11 <= 4f)
					{
						texts[lenght] = Language.GetTextValue("LegacyTooltip.17");
					}
					else if (num11 <= 6f)
					{
						texts[lenght] = Language.GetTextValue("LegacyTooltip.18");
					}
					else if (num11 <= 7f)
					{
						texts[lenght] = Language.GetTextValue("LegacyTooltip.19");
					}
					else if (num11 <= 9f)
					{
						texts[lenght] = Language.GetTextValue("LegacyTooltip.20");
					}
					else if (num11 <= 11f)
					{
						texts[lenght] = Language.GetTextValue("LegacyTooltip.21");
					}
					else
					{
						texts[lenght] = Language.GetTextValue("LegacyTooltip.22");
					}

					tooltipNames[lenght] = "Knockback";
					lenght++;
				}

				if (item.fishingPole > 0)
				{
					texts[lenght] = Language.GetTextValue("GameUI.PrecentFishingPower", item.fishingPole);
					tooltipNames[lenght] = "FishingPower";
					lenght++;
					texts[lenght] = Language.GetTextValue("GameUI.BaitRequired");
					tooltipNames[lenght] = "NeedsBait";
					lenght++;
				}

				if (item.bait > 0)
				{
					texts[lenght] = Language.GetTextValue("GameUI.BaitPower", item.bait);
					tooltipNames[lenght] = "BaitPower";
					lenght++;
				}

				if (item.headSlot > 0 || item.bodySlot > 0 || item.legSlot > 0 || item.accessory || Main.projHook[item.shoot] || item.mountType != -1 || item.buffType > 0 && (Main.lightPet[item.buffType] || Main.vanityPet[item.buffType]))
				{
					texts[lenght] = Language.GetTextValue("LegacyTooltip.23");
					tooltipNames[lenght] = "Equipable";
					lenght++;
				}

				if (item.tileWand > 0)
				{
					texts[lenght] = Language.GetTextValue("LegacyTooltip.52") + Lang.GetItemNameValue(item.tileWand);
					tooltipNames[lenght] = "WandConsumes";
					lenght++;
				}

				if (item.questItem)
				{
					texts[lenght] = Language.GetTextValue("LegacyInterface.65");
					tooltipNames[lenght] = "Quest";
					lenght++;
				}

				if (item.vanity)
				{
					texts[lenght] = Language.GetTextValue("LegacyTooltip.24");
					tooltipNames[lenght] = "Vanity";
					lenght++;
				}

				if (item.defense > 0)
				{
					texts[lenght] = item.defense + Language.GetTextValue("LegacyTooltip.25");
					tooltipNames[lenght] = "Defense";
					lenght++;
				}

				if (item.pick > 0)
				{
					texts[lenght] = item.pick + Language.GetTextValue("LegacyTooltip.26");
					tooltipNames[lenght] = "PickPower";
					lenght++;
				}

				if (item.axe > 0)
				{
					texts[lenght] = item.axe * 5 + Language.GetTextValue("LegacyTooltip.27");
					tooltipNames[lenght] = "AxePower";
					lenght++;
				}

				if (item.hammer > 0)
				{
					texts[lenght] = item.hammer + Language.GetTextValue("LegacyTooltip.28");
					tooltipNames[lenght] = "HammerPower";
					lenght++;
				}

				if (item.tileBoost != 0)
				{
					int tileBoost = item.tileBoost;
					if (tileBoost > 0)
					{
						texts[lenght] = "+" + tileBoost + Language.GetTextValue("LegacyTooltip.54");
					}
					else
					{
						texts[lenght] = tileBoost + Language.GetTextValue("LegacyTooltip.54");
					}

					tooltipNames[lenght] = "TileBoost";
					lenght++;
				}

				if (item.healLife > 0)
				{
					texts[lenght] = Language.GetTextValue("CommonItemTooltip.RestoresLife", player.GetHealLife(item));
					tooltipNames[lenght] = "HealLife";
					lenght++;
				}

				if (item.healMana > 0)
				{
					texts[lenght] = Language.GetTextValue("CommonItemTooltip.RestoresMana", player.GetHealMana(item));
					tooltipNames[lenght] = "HealMana";
					lenght++;
				}

				if (item.mana > 0 && (item.type != 127 || !player.spaceGun))
				{
					texts[lenght] = Language.GetTextValue("CommonItemTooltip.UsesMana", player.GetManaCost(item));
					tooltipNames[lenght] = "UseMana";
					lenght++;
				}

				if (item.createWall > 0 || item.createTile > -1)
				{
					if (item.type != 213 && item.tileWand < 1)
					{
						texts[lenght] = Language.GetTextValue("LegacyTooltip.33");
						tooltipNames[lenght] = "Placeable";
						lenght++;
					}
				}
				else if (item.ammo > 0 && !item.notAmmo)
				{
					texts[lenght] = Language.GetTextValue("LegacyTooltip.34");
					tooltipNames[lenght] = "Ammo";
					lenght++;
				}
				else if (item.consumable)
				{
					texts[lenght] = Language.GetTextValue("LegacyTooltip.35");
					tooltipNames[lenght] = "Consumable";
					lenght++;
				}

				if (item.material)
				{
					texts[lenght] = Language.GetTextValue("LegacyTooltip.36");
					tooltipNames[lenght] = "Material";
					lenght++;
				}

				if (item.ToolTip != null)
				{
					for (int j = 0; j < item.ToolTip.Lines; j++)
					{
						if (j == 0 && item.type >= 1533 && item.type <= 1537 && !NPC.downedPlantBoss)
						{
							texts[lenght] = Language.GetTextValue("LegacyTooltip.59");
							tooltipNames[lenght] = "Tooltip" + j;
							lenght++;
						}
						else
						{
							texts[lenght] = item.ToolTip.GetLine(j);
							tooltipNames[lenght] = "Tooltip" + j;
							lenght++;
						}
					}
				}

				if ((item.type == 3818 || item.type == 3819 || item.type == 3820 || item.type == 3824 || item.type == 3825 || item.type == 3826 || item.type == 3829 || item.type == 3830 || item.type == 3831 || item.type == 3832 || item.type == 3833 || item.type == 3834) && !player.downedDD2EventAnyDifficulty)
				{
					texts[lenght] = Language.GetTextValue("LegacyMisc.104");
					tooltipNames[lenght] = "EtherianManaWarning";
					lenght++;
				}

				if (item.buffType == 26 && Main.expertMode)
				{
					texts[lenght] = Language.GetTextValue("LegacyMisc.40");
					tooltipNames[lenght] = "WellFedExpert";
					lenght++;
				}

				if (item.buffTime > 0)
				{
					var textValue = item.buffTime / 60 >= 60 ? Language.GetTextValue("CommonItemTooltip.MinuteDuration", Math.Round(item.buffTime / 3600f)) : Language.GetTextValue("CommonItemTooltip.SecondDuration", Math.Round(item.buffTime / 60f));

					texts[lenght] = textValue;
					tooltipNames[lenght] = "BuffTime";
					lenght++;
				}

				if (item.type == 3262 || item.type == 3282 || item.type == 3283 || item.type == 3284 || item.type == 3285 || item.type == 3286 || item.type == 3316 || item.type == 3315 || item.type == 3317 || item.type == 3291 || item.type == 3389)
				{
					texts[lenght] = " ";
					oneDropLogoIndex = lenght;
					tooltipNames[lenght] = "OneDropLogo";
					lenght++;
				}

				if (item.prefix > 0)
				{
					if (Main.cpItem == null || Main.cpItem.netID != item.netID)
					{
						Main.cpItem = new Item();
						Main.cpItem.netDefaults(item.netID);
					}

					if (Main.cpItem.damage != item.damage)
					{
						double num12 = item.damage - (float)Main.cpItem.damage;
						num12 = num12 / Main.cpItem.damage * 100.0;
						num12 = Math.Round(num12);
						texts[lenght] = num12 > 0.0 ? "+" + num12 + Language.GetTextValue("LegacyTooltip.39") : num12 + Language.GetTextValue("LegacyTooltip.39");

						if (num12 < 0.0)
						{
							badModifiers[lenght] = true;
						}

						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixDamage";
						lenght++;
					}

					if (Main.cpItem.useAnimation != item.useAnimation)
					{
						double num13 = item.useAnimation - (float)Main.cpItem.useAnimation;
						num13 = num13 / Main.cpItem.useAnimation * 100.0;
						num13 = Math.Round(num13);
						num13 *= -1.0;
						texts[lenght] = num13 > 0.0 ? "+" + num13 + Language.GetTextValue("LegacyTooltip.40") : num13 + Language.GetTextValue("LegacyTooltip.40");

						if (num13 < 0.0)
						{
							badModifiers[lenght] = true;
						}

						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixSpeed";
						lenght++;
					}

					if (Main.cpItem.crit != item.crit)
					{
						double num14 = item.crit - (float)Main.cpItem.crit;
						texts[lenght] = num14 > 0.0 ? "+" + num14 + Language.GetTextValue("LegacyTooltip.41") : num14 + Language.GetTextValue("LegacyTooltip.41");

						if (num14 < 0.0)
						{
							badModifiers[lenght] = true;
						}

						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixCritChance";
						lenght++;
					}

					if (Main.cpItem.mana != item.mana)
					{
						double num15 = item.mana - (float)Main.cpItem.mana;
						num15 = num15 / Main.cpItem.mana * 100.0;
						num15 = Math.Round(num15);
						if (num15 > 0.0)
						{
							texts[lenght] = "+" + num15 + Language.GetTextValue("LegacyTooltip.42");
						}
						else
						{
							texts[lenght] = num15 + Language.GetTextValue("LegacyTooltip.42");
						}

						if (num15 > 0.0)
						{
							badModifiers[lenght] = true;
						}

						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixUseMana";
						lenght++;
					}

					if (Main.cpItem.scale != item.scale)
					{
						double num16 = item.scale - Main.cpItem.scale;
						num16 = num16 / Main.cpItem.scale * 100.0;
						num16 = Math.Round(num16);
						if (num16 > 0.0)
						{
							texts[lenght] = "+" + num16 + Language.GetTextValue("LegacyTooltip.43");
						}
						else
						{
							texts[lenght] = num16 + Language.GetTextValue("LegacyTooltip.43");
						}

						if (num16 < 0.0)
						{
							badModifiers[lenght] = true;
						}

						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixSize";
						lenght++;
					}

					if (Main.cpItem.shootSpeed != item.shootSpeed)
					{
						double num17 = item.shootSpeed - Main.cpItem.shootSpeed;
						num17 = num17 / Main.cpItem.shootSpeed * 100.0;
						num17 = Math.Round(num17);
						texts[lenght] = num17 > 0.0 ? "+" + num17 + Language.GetTextValue("LegacyTooltip.44") : num17 + Language.GetTextValue("LegacyTooltip.44");

						if (num17 < 0.0) badModifiers[lenght] = true;

						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixShootSpeed";
						lenght++;
					}

					if (Main.cpItem.knockBack != knockBack)
					{
						double num18 = knockBack - Main.cpItem.knockBack;
						num18 = num18 / Main.cpItem.knockBack * 100.0;
						num18 = Math.Round(num18);
						texts[lenght] = num18 > 0.0 ? "+" + num18 + Language.GetTextValue("LegacyTooltip.45") : num18 + Language.GetTextValue("LegacyTooltip.45");

						if (num18 < 0.0) badModifiers[lenght] = true;

						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixKnockback";
						lenght++;
					}

					if (item.prefix == 62)
					{
						texts[lenght] = "+1" + Language.GetTextValue("LegacyTooltip.25");
						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixAccDefense";
						lenght++;
					}

					if (item.prefix == 63)
					{
						texts[lenght] = "+2" + Language.GetTextValue("LegacyTooltip.25");
						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixAccDefense";
						lenght++;
					}

					if (item.prefix == 64)
					{
						texts[lenght] = "+3" + Language.GetTextValue("LegacyTooltip.25");
						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixAccDefense";
						lenght++;
					}

					if (item.prefix == 65)
					{
						texts[lenght] = "+4" + Language.GetTextValue("LegacyTooltip.25");
						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixAccDefense";
						lenght++;
					}

					if (item.prefix == 66)
					{
						texts[lenght] = "+20 " + Language.GetTextValue("LegacyTooltip.31");
						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixAccMaxMana";
						lenght++;
					}

					if (item.prefix == 67)
					{
						texts[lenght] = "+2" + Language.GetTextValue("LegacyTooltip.5");
						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixAccCritChance";
						lenght++;
					}

					if (item.prefix == 68)
					{
						texts[lenght] = "+4" + Language.GetTextValue("LegacyTooltip.5");
						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixAccCritChance";
						lenght++;
					}

					if (item.prefix == 69)
					{
						texts[lenght] = "+1" + Language.GetTextValue("LegacyTooltip.39");
						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixAccDamage";
						lenght++;
					}

					if (item.prefix == 70)
					{
						texts[lenght] = "+2" + Language.GetTextValue("LegacyTooltip.39");
						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixAccDamage";
						lenght++;
					}

					if (item.prefix == 71)
					{
						texts[lenght] = "+3" + Language.GetTextValue("LegacyTooltip.39");
						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixAccDamage";
						lenght++;
					}

					if (item.prefix == 72)
					{
						texts[lenght] = "+4" + Language.GetTextValue("LegacyTooltip.39");
						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixAccDamage";
						lenght++;
					}

					if (item.prefix == 73)
					{
						texts[lenght] = "+1" + Language.GetTextValue("LegacyTooltip.46");
						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixAccMoveSpeed";
						lenght++;
					}

					if (item.prefix == 74)
					{
						texts[lenght] = "+2" + Language.GetTextValue("LegacyTooltip.46");
						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixAccMoveSpeed";
						lenght++;
					}

					if (item.prefix == 75)
					{
						texts[lenght] = "+3" + Language.GetTextValue("LegacyTooltip.46");
						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixAccMoveSpeed";
						lenght++;
					}

					if (item.prefix == 76)
					{
						texts[lenght] = "+4" + Language.GetTextValue("LegacyTooltip.46");
						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixAccMoveSpeed";
						lenght++;
					}

					if (item.prefix == 77)
					{
						texts[lenght] = "+1" + Language.GetTextValue("LegacyTooltip.47");
						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixAccMeleeSpeed";
						lenght++;
					}

					if (item.prefix == 78)
					{
						texts[lenght] = "+2" + Language.GetTextValue("LegacyTooltip.47");
						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixAccMeleeSpeed";
						lenght++;
					}

					if (item.prefix == 79)
					{
						texts[lenght] = "+3" + Language.GetTextValue("LegacyTooltip.47");
						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixAccMeleeSpeed";
						lenght++;
					}

					if (item.prefix == 80)
					{
						texts[lenght] = "+4" + Language.GetTextValue("LegacyTooltip.47");
						modifiers[lenght] = true;
						tooltipNames[lenght] = "PrefixAccMeleeSpeed";
						lenght++;
					}
				}

				if (item.wornArmor && player.setBonus != "")
				{
					texts[lenght] = Language.GetTextValue("LegacyTooltip.48") + " " + player.setBonus;
					tooltipNames[lenght] = "SetBonus";
					lenght++;
				}
			}

			if (item.expert)
			{
				texts[lenght] = Language.GetTextValue("GameUI.Expert");
				tooltipNames[lenght] = "Expert";
				lenght++;
			}

			float dark = Main.mouseTextColor / 255f;
			int alpha = Main.mouseTextColor;
			if (Main.npcShop > 0)
			{
				int storeValue = item.GetStoreValue();
				if (item.shopSpecialCurrency != -1)
				{
					tooltipNames[lenght] = "SpecialPrice";
					CustomCurrencyManager.GetPriceText(item.shopSpecialCurrency, texts, ref lenght, storeValue);
					color = new Color((byte)(255f * dark), (byte)(255f * dark), (byte)(255f * dark), alpha);
				}
				else if (item.GetStoreValue() > 0)
				{
					string text = "";
					int num21 = 0;
					int num22 = 0;
					int num23 = 0;
					int num24 = 0;
					int value = storeValue * item.stack;
					if (!item.buy)
					{
						value = storeValue / 5;
						if (value < 1) value = 1;

						value *= item.stack;
					}

					if (value < 1)
					{
						value = 1;
					}

					// note: use this
					int[] coinStacks = Utils.CoinsSplit(value);

					if (value >= 1000000)
					{
						num21 = value / 1000000;
						value -= num21 * 1000000;
					}

					if (value >= 10000)
					{
						num22 = value / 10000;
						value -= num22 * 10000;
					}

					if (value >= 100)
					{
						num23 = value / 100;
						value -= num23 * 100;
					}

					if (value >= 1)
					{
						num24 = value;
					}

					if (num21 > 0)
					{
						text = $"{text}{num21} {Language.GetTextValue("LegacyInterface.15")} ";
					}

					if (num22 > 0)
					{
						text = $"{text}{num22} {Language.GetTextValue("LegacyInterface.16")} ";
					}

					if (num23 > 0)
					{
						text = $"{text}{num23} {Language.GetTextValue("LegacyInterface.17")} ";
					}

					if (num24 > 0)
					{
						text = $"{text}{num24} {Language.GetTextValue("LegacyInterface.18")} ";
					}

					if (!item.buy)
					{
						texts[lenght] = Language.GetTextValue("LegacyTooltip.49") + " " + text;
					}
					else
					{
						texts[lenght] = Language.GetTextValue("LegacyTooltip.50") + " " + text;
					}

					tooltipNames[lenght] = "Price";
					lenght++;
					if (num21 > 0)
					{
						color = new Color((byte)(220f * dark), (byte)(220f * dark), (byte)(198f * dark), alpha);
					}
					else if (num22 > 0)
					{
						color = new Color((byte)(224f * dark), (byte)(201f * dark), (byte)(92f * dark), alpha);
					}
					else if (num23 > 0)
					{
						color = new Color((byte)(181f * dark), (byte)(192f * dark), (byte)(193f * dark), alpha);
					}
					else if (num24 > 0)
					{
						color = new Color((byte)(246f * dark), (byte)(138f * dark), (byte)(96f * dark), alpha);
					}
				}
				else if (item.type != 3817)
				{
					texts[lenght] = Language.GetTextValue("LegacyTooltip.51");
					tooltipNames[lenght] = "Price";
					lenght++;
					color = new Color((byte)(120f * dark), (byte)(120f * dark), (byte)(120f * dark), alpha);
				}
			}
		}
	}
}