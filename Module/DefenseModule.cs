using BaseLibrary;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EnhancedTooltip.Module
{
	internal class DefenseModule : BaseModule
	{
		internal override string Name => "Defense";

		internal override TwoColumnLine Create(Item item, DrawableTooltipLine line)
		{
			return new TwoColumnLine(line)
			{
				textLeft = "Defense",
				textRight = Config.NumberStyles.FormatNumber(item.defense),
				colorRight = Utility.DoubleLerp(Color.Red, Color.Yellow, Color.LimeGreen, item.defense / EnhancedTooltip.GetStat(EnhancedTooltip.Stat.Defense))
			};
		}
	}
}