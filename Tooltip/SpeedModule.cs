using BaseLibrary;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EnhancedTooltip.Tooltip
{
	internal class SpeedModule : BaseModule
	{
		internal override string Name => "Speed";

		internal override TwoColumnLine Create(Item item, DrawableTooltipLine line)
		{
			string speedText;
			if (item.useAnimation <= 8) speedText = "Insanely fast";
			else if (item.useAnimation <= 20) speedText = "Very fast";
			else if (item.useAnimation <= 25) speedText = "Fast";
			else if (item.useAnimation <= 30) speedText = "Average";
			else if (item.useAnimation <= 35) speedText = "Slow";
			else if (item.useAnimation <= 45) speedText = "Very slow";
			else if (item.useAnimation <= 55) speedText = "Extremely slow";
			else speedText = "Snail";

			return new TwoColumnLine(line)
			{
				textLeft = "Speed",
				textRight = $"{speedText} ({item.useAnimation})",
				colorRight = Utility.DoubleLerp(Color.LimeGreen, Color.Yellow, Color.Red, item.useAnimation / GetMaxSpeed(item))
			};
		}

		private static float GetMaxSpeed(Item item)
		{
			float maxSpeed = 1f;

			if (item.melee) maxSpeed = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.MeleeSpeed);
			if (item.ranged && item.ammo == AmmoID.None) maxSpeed = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.RangedItemSpeed);
			if (item.ranged && item.ammo != AmmoID.None) maxSpeed = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.RangedAmmoSpeed);
			if (item.magic) maxSpeed = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.MagicSpeed);
			if (item.summon) maxSpeed = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.SummonSpeed);
			if (item.thrown) maxSpeed = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.ThrownSpeed);

			return maxSpeed;
		}
	}
}