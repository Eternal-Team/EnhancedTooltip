using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EnhancedTooltip.Tooltip
{
	public class DamageTooltip
	{
		public static TwoColumnLine GetLine(DrawableTooltipLine line, Item item)
		{
			float itemDamage = item.damage;
			string textDamage = "damage:";
			if (item.melee)
			{
				textDamage = textDamage.Insert(0, "Melee ");
				itemDamage *= Main.LocalPlayer.meleeDamage;
			}
			else if (item.ranged)
			{
				textDamage = textDamage.Insert(0, "Ranged ");
				itemDamage *= Main.LocalPlayer.rangedDamage;
			}
			else if (item.magic)
			{
				textDamage = textDamage.Insert(0, "Magic ");
				itemDamage *= Main.LocalPlayer.magicDamage;
			}
			else if (item.summon)
			{
				textDamage = textDamage.Insert(0, "Summon ");
				itemDamage *= Main.LocalPlayer.minionDamage;
			}
			else if (item.thrown)
			{
				textDamage = textDamage.Insert(0, "Throwing ");
				itemDamage *= Main.LocalPlayer.thrownDamage;
			}
			return new TwoColumnLine(textDamage, ((int)itemDamage).ToString(), line.color, Utility.DoubleLerp(Color.Red, Color.Yellow, Color.Lime, (float)item.damage / (float)EnhancedTooltip.Instance.maxDamage), line.baseScale);
		}
	}
}