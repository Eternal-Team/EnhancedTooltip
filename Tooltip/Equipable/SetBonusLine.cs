﻿using Terraria;
using Terraria.Localization;

namespace EnhancedTooltip.Tooltip.Equipable
{
	public class SetBonusLine : BaseSimpleLine
	{
		public override string Text => $"{Language.GetTextValue("LegacyTooltip.48")} {Player.setBonus}";
	}
}