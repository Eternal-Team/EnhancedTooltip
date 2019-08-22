using Terraria;
using Terraria.Localization;

namespace EnhancedTooltip.Tooltip.Tool
{
	public class TileWandLine : BaseSimpleLine
	{
		public override string Text => Language.GetTextValue("LegacyTooltip.52") + Lang.GetItemNameValue(Item.tileWand);
	}
}