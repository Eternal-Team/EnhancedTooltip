using Terraria;
using Terraria.Localization;

namespace EnhancedTooltip.Tooltip.Tool
{
	public class AxeLine : BaseSimpleLine
	{
		public override string Text => Item.axe * 5 + Language.GetTextValue("LegacyTooltip.27");
	}
}