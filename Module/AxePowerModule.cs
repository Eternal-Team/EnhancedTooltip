using BaseLibrary;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EnhancedTooltip.Module
{
	internal class AxePowerModule : BaseModule
	{
		internal override string Name => "AxePower";

		internal override TwoColumnLine Create(Item item, DrawableTooltipLine line)
		{
			return new TwoColumnLine(line)
			{
				textLeft = "Axe power",
				textRight = Config.NumberStyles.FormatNumber(item.axe * 5),
				colorRight = Utility.DoubleLerp(Color.Red, Color.Yellow, Color.LimeGreen, item.axe / EnhancedTooltip.GetStat(EnhancedTooltip.Stat.AxePower))
			};
		}
	}
}