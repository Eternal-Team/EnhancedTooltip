using Terraria;
using Terraria.Localization;

namespace EnhancedTooltip.Tooltip.Tool
{
	public class FishingRodLine : BaseSimpleLine
	{
		public override string Text => Language.GetTextValue("GameUI.PrecentFishingPower", Item.fishingPole) + "\n" + Language.GetTextValue("GameUI.BaitRequired");
	}
}