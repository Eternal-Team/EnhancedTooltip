using BaseLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace EnhancedTooltip.Tooltip.Weapon
{
	public class SpeedLine : BaseTooltipLine
	{
		public string TextLeft, TextRight;

		public SpeedLine(Item item)
		{
			Item = item;

			if (Config.UseTwoColumnLines)
			{
				TextLeft = "Speed";

				if (item.useAnimation <= 8) TextRight = "Insanely fast";
				else if (item.useAnimation <= 20) TextRight = "Very fast";
				else if (item.useAnimation <= 25) TextRight = "Fast";
				else if (item.useAnimation <= 30) TextRight = "Average";
				else if (item.useAnimation <= 35) TextRight = "Slow";
				else if (item.useAnimation <= 45) TextRight = "Very slow";
				else if (item.useAnimation <= 55) TextRight = "Extremely slow";
				else TextRight = "Snail";

				TextRight += $" ({item.useAnimation})";
			}
			else
			{
				if (item.useAnimation <= 8) TextLeft = Language.GetTextValue("LegacyTooltip.6");
				else if (item.useAnimation <= 20) TextLeft = Language.GetTextValue("LegacyTooltip.7");
				else if (item.useAnimation <= 25) TextLeft = Language.GetTextValue("LegacyTooltip.8");
				else if (item.useAnimation <= 30) TextLeft = Language.GetTextValue("LegacyTooltip.9");
				else if (item.useAnimation <= 35) TextLeft = Language.GetTextValue("LegacyTooltip.10");
				else if (item.useAnimation <= 45) TextLeft = Language.GetTextValue("LegacyTooltip.11");
				else if (item.useAnimation <= 55) TextLeft = Language.GetTextValue("LegacyTooltip.12");
				else TextLeft = Language.GetTextValue("LegacyTooltip.13");

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
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, TextRight, Position + new Vector2(maxWidth, 0), Utility.DoubleLerp(Color.LimeGreen, Color.Yellow, Color.Red, Item.useAnimation / GetMaxSpeed()), 0f, new Vector2(size.X, 0), Vector2.One);
			}
		}

		private float GetMaxSpeed()
		{
			float maxSpeed = 1f;

			if (Item.melee) maxSpeed = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.MeleeSpeed);
			if (Item.ranged && Item.ammo == AmmoID.None) maxSpeed = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.RangedItemSpeed);
			if (Item.ranged && Item.ammo != AmmoID.None) maxSpeed = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.RangedAmmoSpeed);
			if (Item.magic) maxSpeed = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.MagicSpeed);
			if (Item.summon) maxSpeed = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.SummonSpeed);
			if (Item.thrown) maxSpeed = EnhancedTooltip.GetStat(EnhancedTooltip.Stat.ThrownSpeed);

			return maxSpeed;
		}
	}
}