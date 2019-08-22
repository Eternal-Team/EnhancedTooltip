using Terraria;
using Terraria.Localization;

namespace EnhancedTooltip.Tooltip.Consumable
{
	public class HealLifeLine : BaseSimpleLine
	{
		public override string Text => Language.GetTextValue("CommonItemTooltip.RestoresLife", Main.LocalPlayer.GetHealLife(Item));
	}
}