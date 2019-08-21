using BaseLibrary;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EnhancedTooltip.Tooltip
{
	internal class KnockbackModule : BaseModule
	{
		internal override string Name => "Knockback";

		internal override TwoColumnLine Create(Item item, DrawableTooltipLine line)
		{
			Player player = Main.LocalPlayer;

			float knockback = item.knockBack;
			if (item.summon) knockback += player.minionKB;
			if (player.magicQuiver && item.useAmmo == AmmoID.Arrow || item.useAmmo == AmmoID.Stake) knockback = (int)(knockback * 1.1f);
			if (player.inventory[player.selectedItem].type == 3106 && item.type == 3106) knockback += knockback * (1f - player.stealth);

			ItemLoader.GetWeaponKnockback(item, player, ref knockback);
			PlayerHooks.GetWeaponKnockback(player, item, ref knockback);

			string knockbackText;
			if (knockback <= 0f) knockbackText = "No";
			else if (knockback <= 1.5f) knockbackText = "Extremely weak";
			else if (knockback <= 3f) knockbackText = "Very weak";
			else if (knockback <= 4f) knockbackText = "Weak";
			else if (knockback <= 6f) knockbackText = "Average";
			else if (knockback <= 7f) knockbackText = "Strong";
			else if (knockback <= 9f) knockbackText = "Very strong";
			else if (knockback <= 11f) knockbackText = "Extremely strong";
			else knockbackText = "Insane";

			return new TwoColumnLine(line)
			{
				textLeft = "Knockback",
				textRight = $"{knockbackText} ({knockback:F1})",
				colorRight = Utility.DoubleLerp(Color.Red, Color.Yellow, Color.LimeGreen, knockback / GetMaxKnockback(item))
			};
		}

		private static float GetMaxKnockback(Item item)
		{
			float maxKnockback = 1f;

			if (item.melee) maxKnockback = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.MeleeKnockback);
			if (item.ranged && item.ammo == AmmoID.None) maxKnockback = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.RangedItemKnockback);
			if (item.ranged && item.ammo != AmmoID.None) maxKnockback = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.RangedAmmoKnockback);
			if (item.magic) maxKnockback = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.MagicKnockback);
			if (item.summon) maxKnockback = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.SummonKnockback);
			if (item.thrown) maxKnockback = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.ThrownKnockback);

			return maxKnockback;
		}
	}
}