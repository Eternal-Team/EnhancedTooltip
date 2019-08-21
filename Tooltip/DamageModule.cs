using BaseLibrary;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
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

			if (item.type == 3829 || item.type == 3830 || item.type == 3831) damage *= 3;

			string text;
			if (item.melee) text = Language.GetTextValue("LegacyTooltip.2");
			else if (item.ranged) text = Language.GetTextValue("LegacyTooltip.3");
			else if (item.magic) text = Language.GetTextValue("LegacyTooltip.4");
			else if (item.summon) text = Language.GetTextValue("LegacyTooltip.53");
			else if (item.thrown) text = Language.GetTextValue("LegacyTooltip.58");
			else text = " Damage";

			char startChar = text[1];
			text = text.Remove(0, 2);
			text = text.Insert(0, char.ToUpper(startChar).ToString());

			return new TwoColumnLine(line)
			{
				textLeft = text,
				textRight = Config.NumberStyles.FormatNumber(damage),
				colorRight = Utility.DoubleLerp(Color.Red, Color.Yellow, Color.LimeGreen, item.damage / GetMaxDamage(item))
			};
		}

		private static float GetMaxDamage(Item item)
		{
			float maxDamage = 1f;

			if (item.melee) maxDamage = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.MeleeDamage);
			if (item.ranged && item.ammo == AmmoID.None) maxDamage = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.RangedItemDamage);
			if (item.ranged && item.ammo != AmmoID.None) maxDamage = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.RangedAmmoDamage);
			if (item.magic) maxDamage = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.MagicDamage);
			if (item.summon) maxDamage = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.SummonDamage);
			if (item.thrown) maxDamage = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.ThrownDamage);

			return maxDamage;
		}
	}
}