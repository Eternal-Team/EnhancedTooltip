using Terraria;
using Terraria.Localization;

namespace EnhancedTooltip.Tooltip.Common
{
	public class PlaceableLine : BaseSimpleLine
	{
		public override string Text => Language.GetTextValue("LegacyTooltip.33");
	}
}