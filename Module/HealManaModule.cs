﻿using BaseLibrary;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EnhancedTooltip.Module
{
	internal class HealManaModule : BaseModule
	{
		internal override string Name => "HealMana";

		internal override TwoColumnLine Create(Item item, DrawableTooltipLine line)
		{
			return new TwoColumnLine(line)
			{
				textLeft = "Restores",
				textRight = item.healMana + " mana",
				colorRight = Utility.DoubleLerp(Color.Red, Color.Yellow, Color.LimeGreen, item.healMana / EnhancedTooltip.GetStat(EnhancedTooltip.Stat.HealMana))
			};
		}
	}
}