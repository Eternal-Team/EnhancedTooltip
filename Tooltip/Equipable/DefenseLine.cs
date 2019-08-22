using Terraria;
using Terraria.Localization;

namespace EnhancedTooltip.Tooltip.Equipable
{
	public class DefenseLine : BaseSimpleLine
	{
		public override string Text => Item.defense + Language.GetTextValue("LegacyTooltip.25");
	}
}