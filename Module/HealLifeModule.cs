﻿using BaseLibrary;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EnhancedTooltip.Module
{
	internal class HealLifeModule : BaseModule
	{
		internal override string Name => "HealLife";

		internal override TwoColumnLine Create(Item item, DrawableTooltipLine line)
		{
			return new TwoColumnLine(line)
			{
				textLeft = "Heals",
				textRight = item.healLife + " life",
				colorRight = Utility.DoubleLerp(Color.Red, Color.Yellow, Color.LimeGreen, item.healLife / EnhancedTooltip.GetStat(EnhancedTooltip.Stat.HealLife))
			};
		}
	}
}