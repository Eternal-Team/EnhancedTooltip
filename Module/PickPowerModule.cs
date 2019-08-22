using BaseLibrary;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EnhancedTooltip.Module
{
	internal class PickPowerModule : BaseModule
	{
		internal override string Name => "PickPower";

		internal override TwoColumnLine Create(Item item, DrawableTooltipLine line)
		{
			return new TwoColumnLine(line)
			{
				textLeft = "Pickaxe power",
				textRight = Config.NumberStyles.FormatNumber(item.pick),
				colorRight = Utility.DoubleLerp(Color.Red, Color.Yellow, Color.LimeGreen, item.pick / EnhancedTooltip.GetStat(EnhancedTooltip.Stat.PickaxePower))
			};
		}
	}
}