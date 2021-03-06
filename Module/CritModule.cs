﻿using BaseLibrary;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EnhancedTooltip.Module
{
	internal class CritModule : BaseModule
	{
		internal override string Name => "CritChance";

		internal override TwoColumnLine Create(Item item, DrawableTooltipLine line)
		{
			Player player = Main.LocalPlayer;
			int critChance = 0;

			if (item.melee)
			{
				critChance = player.meleeCrit - player.HeldItem.crit + item.crit;
				ItemLoader.GetWeaponCrit(item, player, ref critChance);
				PlayerHooks.GetWeaponCrit(player, item, ref critChance);
			}
			else if (item.ranged)
			{
				critChance = player.rangedCrit - player.HeldItem.crit + item.crit;
				ItemLoader.GetWeaponCrit(item, player, ref critChance);
				PlayerHooks.GetWeaponCrit(player, item, ref critChance);
			}
			else if (item.magic)
			{
				critChance = player.magicCrit - player.HeldItem.crit + item.crit;
				ItemLoader.GetWeaponCrit(item, player, ref critChance);
				PlayerHooks.GetWeaponCrit(player, item, ref critChance);
			}
			else if (item.thrown)
			{
				critChance = player.thrownCrit - player.HeldItem.crit + item.crit;
				ItemLoader.GetWeaponCrit(item, player, ref critChance);
				PlayerHooks.GetWeaponCrit(player, item, ref critChance);
			}
			else if (!item.summon)
			{
				critChance = item.crit;
				ItemLoader.GetWeaponCrit(item, player, ref critChance);
				PlayerHooks.GetWeaponCrit(player, item, ref critChance);
			}

			return new TwoColumnLine(line)
			{
				textLeft = "Critical strike chance",
				textRight = critChance + "%",
				colorRight = Utility.DoubleLerp(Color.Red, Color.Yellow, Color.LimeGreen, critChance / GetMaxCrit(item))
			};
		}

		private static float GetMaxCrit(Item item)
		{
			float maxCrit = 1f;

			if (item.melee) maxCrit = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.MeleeCrit);
			if (item.ranged) maxCrit = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.RangedCrit);
			if (item.magic) maxCrit = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.MagicCrit);
			if (item.thrown) maxCrit = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.ThrownCrit);

			return maxCrit;
		}
	}
}