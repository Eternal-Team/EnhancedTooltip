using BaseLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace EnhancedTooltip.Tooltip.Weapon
{
	public class CritLine : BaseTooltipLine
	{
		public string TextLeft, TextRight;

		public CritLine(Item item)
		{
			Item = item;
			int critChance = GetCrit();

			if (Config.UseTwoColumnLines)
			{
				TextLeft = "Critical strike chance";
				TextRight = critChance + "%";
			}
			else
			{
				TextLeft = critChance + Language.GetTextValue("LegacyTooltip.5");
				TextRight = "";
			}
		}

		public override Vector2 GetSize()
		{
			Vector2 size = ChatManager.GetStringSize(Main.fontMouseText, TextLeft, Vector2.One);

			if (Config.UseTwoColumnLines)
			{
				Vector2 sizeRight = ChatManager.GetStringSize(Main.fontMouseText, TextRight, Vector2.One);
				size.X += sizeRight.X + 32f;
				if (sizeRight.Y > size.Y) size.Y = sizeRight.Y;
			}

			return size;
		}

		public override void Draw(SpriteBatch spriteBatch, float maxWidth)
		{
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, TextLeft, Position, Color.White, 0f, Vector2.Zero, Vector2.One);

			if (Config.UseTwoColumnLines)
			{
				Vector2 size = ChatManager.GetStringSize(Main.fontMouseText, TextRight, Vector2.One);
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, TextRight, Position + new Vector2(maxWidth, 0), Utility.DoubleLerp(Color.Red, Color.Yellow, Color.LimeGreen, Item.crit / GetMaxCrit()), 0f, new Vector2(size.X, 0), Vector2.One);
			}
		}

		public int GetCrit()
		{
			Player player = Main.LocalPlayer;
			int critChance = 0;

			if (Item.melee)
			{
				critChance = player.meleeCrit - player.HeldItem.crit + Item.crit;
				ItemLoader.GetWeaponCrit(Item, player, ref critChance);
				PlayerHooks.GetWeaponCrit(player, Item, ref critChance);
			}
			else if (Item.ranged)
			{
				critChance = player.rangedCrit - player.HeldItem.crit + Item.crit;
				ItemLoader.GetWeaponCrit(Item, player, ref critChance);
				PlayerHooks.GetWeaponCrit(player, Item, ref critChance);
			}
			else if (Item.magic)
			{
				critChance = player.magicCrit - player.HeldItem.crit + Item.crit;
				ItemLoader.GetWeaponCrit(Item, player, ref critChance);
				PlayerHooks.GetWeaponCrit(player, Item, ref critChance);
			}
			else if (Item.thrown)
			{
				critChance = player.thrownCrit - player.HeldItem.crit + Item.crit;
				ItemLoader.GetWeaponCrit(Item, player, ref critChance);
				PlayerHooks.GetWeaponCrit(player, Item, ref critChance);
			}
			else if (!Item.summon)
			{
				critChance = Item.crit;
				ItemLoader.GetWeaponCrit(Item, player, ref critChance);
				PlayerHooks.GetWeaponCrit(player, Item, ref critChance);
			}

			return critChance;
		}

		private float GetMaxCrit()
		{
			float maxCrit = 1f;

			if (Item.melee) maxCrit = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.MeleeCrit);
			if (Item.ranged) maxCrit = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.RangedCrit);
			if (Item.magic) maxCrit = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.MagicCrit);
			if (Item.thrown) maxCrit = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.ThrownCrit);

			return maxCrit;
		}
	}
}