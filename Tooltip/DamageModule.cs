using BaseLibrary;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EnhancedTooltip.Tooltip
{
	internal class DamageModule : BaseModule
	{
		internal override string Name => "Damage";

		internal override TwoColumnLine Create(Item item, DrawableTooltipLine line)
		{
			int damage = Main.LocalPlayer.GetWeaponDamage(item);
			int maxDamage = 1;

			if (item.type == 3829 || item.type == 3830 || item.type == 3831) damage *= 3;

			string text;
			if (item.melee)
			{
				text = Language.GetTextValue("LegacyTooltip.2");
				maxDamage = EnhancedTooltip.GetStat("MeleeDamage");
			}
			else if (item.ranged)
			{
				text = Language.GetTextValue("LegacyTooltip.3");
				maxDamage = EnhancedTooltip.GetStat("RangedDamage");
			}
			else if (item.magic)
			{
				text = Language.GetTextValue("LegacyTooltip.4");
				maxDamage = EnhancedTooltip.GetStat("MagicDamage");
			}
			else if (item.summon)
			{
				text = Language.GetTextValue("LegacyTooltip.53");
				maxDamage = EnhancedTooltip.GetStat("SummonDamage");
			}
			else if (item.thrown)
			{
				text = Language.GetTextValue("LegacyTooltip.58");
				maxDamage = EnhancedTooltip.GetStat("ThrownDamage");
			}
			else text = " Damage";

			char startChar = text[1];
			text = text.Remove(0, 2);
			text = text.Insert(0, char.ToUpper(startChar).ToString());

			return new TwoColumnLine(line)
			{
				textLeft = text,
				textRight = Config.NumberStyles.FormatNumber(damage),
				colorRight = Utility.DoubleLerp(Color.Red, Color.Yellow, Color.LimeGreen, item.damage / (float)maxDamage)
			};
		}
	}
}