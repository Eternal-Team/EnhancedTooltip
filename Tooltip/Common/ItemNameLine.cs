using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;
using Terraria;
using Terraria.UI.Chat;

namespace EnhancedTooltip.Tooltip.Common
{
	public class ItemNameLine : BaseTooltipLine
	{
		public string Text
		{
			get
			{
				if (Config.ShowMaxStack)
				{
					string text = Item.AffixName();
					if (Item.stack > 1) text = $"{text} ({Item.stack}/{Item.maxStack})";
					return text;
				}

				return Item.HoverName;
			}
		}

		public override Vector2 GetSize()
		{
			Vector2 size = ChatManager.GetStringSize(Main.fontMouseText, Text, Vector2.One);
			if (Item.favorited) size.X += 32f;
			return size;
		}

		public override int GetBottomMargin(ReadOnlyCollection<BaseTooltipLine> lines) => lines.Count > 1 ? 8 : 0;

		public override void Draw(SpriteBatch spriteBatch, float maxWidth)
		{
			Color color = Hooking.GetRarityColor(Item);
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, Text, Position, color, 0f, Vector2.Zero, Vector2.One);

			if (Item.favorited && Config.FavoriteUseIcon)
			{
				Main.spriteBatch.Draw(Main.cursorTextures[3], new Vector2(Position.X + maxWidth - 8f, Position.Y + GetSize().Y * 0.5f - 4f), null, Color.White, 0f, Main.cursorTextures[3].Size() * 0.5f, 1f, SpriteEffects.None, 0f);
			}
		}
	}
}