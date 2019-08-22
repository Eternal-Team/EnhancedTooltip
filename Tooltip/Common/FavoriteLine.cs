using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace EnhancedTooltip.Tooltip.Common
{
	public class FavoriteLine : BaseTooltipLine
	{
		public string Text;

		public FavoriteLine()
		{
			Text = Language.GetTextValue("LegacyTooltip.56") + "\n" + Language.GetTextValue("LegacyTooltip.57");
		}

		public override Vector2 GetSize() => ChatManager.GetStringSize(Main.fontMouseText, Text, Vector2.One);

		public override void Draw(SpriteBatch spriteBatch, float maxWidth)
		{
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, Text, Position, Color.White, 0f, Vector2.Zero, Vector2.One);
		}
	}
}