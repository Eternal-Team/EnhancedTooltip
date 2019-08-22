using BaseLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace EnhancedTooltip.Tooltip.Weapon
{
	public class DamageLine : BaseTooltipLine
	{
		public string TextLeft, TextRight;

		public DamageLine(Item item)
		{
			Item = item;

			if (Config.UseTwoColumnLines)
			{
				if (Item.melee) TextLeft = "Melee damage";
				else if (Item.ranged) TextLeft = "Ranged damage";
				else if (Item.magic) TextLeft = "Magic damage";
				else if (Item.summon) TextLeft = "Summon damage";
				else if (Item.thrown) TextLeft = "Throwing damage";
				else TextLeft = "Damage";

				int damage = Main.LocalPlayer.GetWeaponDamage(Item);

				if (Item.type == 3829 || Item.type == 3830 || Item.type == 3831) damage *= 3;

				TextRight = Config.NumberStyles.FormatNumber(damage);
			}
			else
			{
				int damage = Main.LocalPlayer.GetWeaponDamage(Item);

				if (Item.type == 3829 || Item.type == 3830 || Item.type == 3831) damage *= 3;

				if (Item.melee) TextLeft = Language.GetTextValue("LegacyTooltip.2");
				else if (Item.ranged) TextLeft = Language.GetTextValue("LegacyTooltip.3");
				else if (Item.magic) TextLeft = Language.GetTextValue("LegacyTooltip.4");
				else if (Item.thrown) TextLeft = Language.GetTextValue("LegacyTooltip.58");
				else if (Item.summon) TextLeft = Language.GetTextValue("LegacyTooltip.53");
				else TextLeft = Language.GetTextValue("LegacyTooltip.55");

				TextLeft = string.Concat(damage, TextLeft);
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
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, TextRight, Position + new Vector2(maxWidth, 0), Utility.DoubleLerp(Color.Red, Color.Yellow, Color.LimeGreen, Item.damage / GetMaxDamage()), 0f, new Vector2(size.X, 0), Vector2.One);
			}
		}

		private float GetMaxDamage()
		{
			float maxDamage = 1f;

			if (Item.melee) maxDamage = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.MeleeDamage);
			if (Item.ranged && Item.ammo == AmmoID.None) maxDamage = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.RangedItemDamage);
			if (Item.ranged && Item.ammo != AmmoID.None) maxDamage = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.RangedAmmoDamage);
			if (Item.magic) maxDamage = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.MagicDamage);
			if (Item.summon) maxDamage = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.SummonDamage);
			if (Item.thrown) maxDamage = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.ThrownDamage);

			return maxDamage;
		}
	}
}