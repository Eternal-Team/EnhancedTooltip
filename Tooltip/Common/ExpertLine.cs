using Terraria;
using Terraria.Localization;

namespace EnhancedTooltip.Tooltip.Common
{
	public class ExpertLine : BaseSimpleLine
	{
		public override string Text=> Language.GetTextValue("GameUI.Expert");
	}
}