using BaseLibrary;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EnhancedTooltip.Module
{
	internal class HammerPowerModule : BaseModule
	{
		internal override string Name => "HammerPower";

		internal override TwoColumnLine Create(Item item, DrawableTooltipLine line)
		{
			return new TwoColumnLine(line)
			{
				textLeft = "Hammer power",
				textRight = Config.NumberStyles.FormatNumber(item.hammer),
				colorRight = Utility.DoubleLerp(Color.Red, Color.Yellow, Color.LimeGreen, item.hammer / EnhancedTooltip.GetStat(EnhancedTooltip.Stat.HammerPower))
			};
		}
	}
}