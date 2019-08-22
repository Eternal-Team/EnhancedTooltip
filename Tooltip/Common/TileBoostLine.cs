using Terraria;
using Terraria.Localization;

namespace EnhancedTooltip.Tooltip.Common
{
	public class TileBoostLine : BaseSimpleLine
	{
		public override string Text => Item.tileBoost > 0 ? "+" + Item.tileBoost + Language.GetTextValue("LegacyTooltip.54") : Item.tileBoost + Language.GetTextValue("LegacyTooltip.54");
	}
}