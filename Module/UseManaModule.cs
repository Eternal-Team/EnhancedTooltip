using BaseLibrary;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EnhancedTooltip.Module
{
	internal class UseManaModule : BaseModule
	{
		internal override string Name => "UseMana";

		internal override TwoColumnLine Create(Item item, DrawableTooltipLine line)
		{
			return new TwoColumnLine(line)
			{
				textLeft = "Consumes",
				textRight = item.mana + " mana",
				colorRight = Utility.DoubleLerp(Color.LimeGreen, Color.Yellow, Color.Red, item.mana / EnhancedTooltip.GetStat(EnhancedTooltip.Stat.UseMana))
			};
		}
	}
}