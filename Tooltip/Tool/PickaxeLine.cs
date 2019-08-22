using Terraria;
using Terraria.Localization;

namespace EnhancedTooltip.Tooltip.Tool
{
	public class PickaxeLine : BaseSimpleLine
	{
		public override string Text => Item.pick + Language.GetTextValue("LegacyTooltip.26");
	}
}