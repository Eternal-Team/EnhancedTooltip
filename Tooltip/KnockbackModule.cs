using BaseLibrary;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
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

			if (Config.TooltipUseValues)
			{
				return new TwoColumnLine(line)
				{
					textLeft = "Knockback",
					textRight = knockback.ToString("F1"),
					colorRight = Utility.DoubleLerp(Color.Red, Color.Yellow, Color.LimeGreen, knockback / EnhancedTooltip.GetStat("Knockback"))
				};
			}

			string text;
			if (knockback == 0f) text = Language.GetTextValue("LegacyTooltip.14");
			else if (knockback <= 1.5) text = Language.GetTextValue("LegacyTooltip.15");
			else if (knockback <= 3f) text = Language.GetTextValue("LegacyTooltip.16");
			else if (knockback <= 4f) text = Language.GetTextValue("LegacyTooltip.17");
			else if (knockback <= 6f) text = Language.GetTextValue("LegacyTooltip.18");
			else if (knockback <= 7f) text = Language.GetTextValue("LegacyTooltip.19");
			else if (knockback <= 9f) text = Language.GetTextValue("LegacyTooltip.20");
			else if (knockback <= 11f) text = Language.GetTextValue("LegacyTooltip.21");
			else text = Language.GetTextValue("LegacyTooltip.22");

			return new TwoColumnLine(line)
			{
				textLeft = text
			};
		}
	}
}