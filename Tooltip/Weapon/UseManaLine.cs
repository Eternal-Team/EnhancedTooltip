using Terraria;
using Terraria.Localization;

namespace EnhancedTooltip.Tooltip.Weapon
{
	public class UseManaLine : BaseSimpleLine
	{
		public override string Text => Language.GetTextValue("CommonItemTooltip.UsesMana", Main.LocalPlayer.GetManaCost(Item));
	}
}