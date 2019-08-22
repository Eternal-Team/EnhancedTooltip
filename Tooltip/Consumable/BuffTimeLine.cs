using System;
using Terraria;
using Terraria.Localization;

namespace EnhancedTooltip.Tooltip.Consumable
{
	public class BuffTimeLine : BaseSimpleLine
	{
		public override string Text => Item.buffTime / 60 >= 60 ? Language.GetTextValue("CommonItemTooltip.MinuteDuration", Math.Round(Item.buffTime / 3600f)) : Language.GetTextValue("CommonItemTooltip.SecondDuration", Math.Round(Item.buffTime / 60f));
	}
}