using Terraria;
using Terraria.Localization;

namespace EnhancedTooltip.Tooltip.Tool
{
	public class BaitLine : BaseSimpleLine
	{
		public override string Text => Language.GetTextValue("GameUI.BaitPower", Item.bait);
	}
}