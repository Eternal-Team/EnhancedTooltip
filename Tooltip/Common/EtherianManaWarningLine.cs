using Terraria;
using Terraria.Localization;

namespace EnhancedTooltip.Tooltip.Common
{
	public class EtherianManaWarningLine : BaseSimpleLine
	{
		public override string Text => Language.GetTextValue("LegacyMisc.104");
	}
}