using Terraria;
using Terraria.Localization;

namespace EnhancedTooltip.Tooltip.Consumable
{
	public class HealManaLine : BaseSimpleLine
	{
		public override string Text => Language.GetTextValue("CommonItemTooltip.RestoresMana", Main.LocalPlayer.GetHealMana(Item));
	}
}