using Terraria;
using Terraria.Localization;

namespace EnhancedTooltip.Tooltip.Tool
{
	public class HammerLine : BaseSimpleLine
	{
		public override string Text => Item.hammer + Language.GetTextValue("LegacyTooltip.28");
	}
}