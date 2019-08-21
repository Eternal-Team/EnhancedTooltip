using BaseLibrary;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EnhancedTooltip.Tooltip
{
	internal class TileBoostModule : BaseModule
	{
		internal override string Name => "TileBoost";

		internal override TwoColumnLine Create(Item item, DrawableTooltipLine line)
		{
			return new TwoColumnLine(line)
			{
				textLeft = "Range",
				textRight = (item.tileBoost > 0 ? "+" : "") + item.tileBoost,
				colorRight = Utility.DoubleLerp(Color.Red, Color.Yellow, Color.LimeGreen, (float)item.tileBoost / EnhancedTooltip.GetStat(EnhancedTooltip.Stat.TileBoost))
			};
		}
	}
}