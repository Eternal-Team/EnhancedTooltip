using Terraria;
using Terraria.Localization;

namespace EnhancedTooltip.Tooltip.Common
{
	public class DefenseLine : BaseSimpleLine
	{
		public override string Text => Item.defense + Language.GetTextValue("LegacyTooltip.25");
	}
}