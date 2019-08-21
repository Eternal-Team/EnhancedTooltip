using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace EnhancedTooltip.Tooltip
{
	public class TwoColumnLine
	{
		public readonly DrawableTooltipLine line;

		public string Name => line.Name;

		public string Mod => line.mod;

		public string textLeft, textRight;

		public Color colorLeft, colorRight = Color.White;

		public Vector2 scaleLeft, scaleRight = Vector2.One;

		public int X
		{
			get => line.X;
			set => line.X = value;
		}

		public int Y
		{
			get => line.Y;
			set => line.Y = value;
		}

		public float Rotation => line.rotation;

		public Vector2 Origin => line.origin;

		public float Spread => line.spread;

		public float MaxWidth => line.maxWidth;

		public DynamicSpriteFont Font => line.font;

		public bool OneDropLogo => line.oneDropLogo;

		public Vector2 GetSize()
		{
			Vector2 size = Font.MeasureString(textLeft) * scaleLeft;
			if (!string.IsNullOrWhiteSpace(textRight))
			{
				Vector2 sizeRight = Font.MeasureString(textRight) * scaleRight;
				size.X += sizeRight.X + 32f;
				if (sizeRight.Y > size.Y) size.Y = sizeRight.Y;
			}

			return size + new Vector2(line.Name == "ItemName" && Main.HoverItem.favorited ? 32f : 0f, 0f);
		}

		public TwoColumnLine(DrawableTooltipLine line)
		{
			this.line = line;
			colorLeft = line.overrideColor ?? line.color;

			if (!(Mod == "Terraria" && Name == "ItemName")) line.Y += 8;

			if (!OneDropLogo)
			{
				if (Mod == "Terraria" && Name == "ItemName") scaleLeft = new Vector2(EnhancedTooltip.Instance.GetConfig<EnhancedTooltipConfig>().ItemNameScale);
				else if (Name.Contains("Tooltip")) scaleLeft = new Vector2(EnhancedTooltip.Instance.GetConfig<EnhancedTooltipConfig>().TooltipTextScale);
				else scaleLeft = new Vector2(EnhancedTooltip.Instance.GetConfig<EnhancedTooltipConfig>().OtherTextScale);
			}
		}

		public static TwoColumnLine CreateFromDrawableTooltipLine(Item item, DrawableTooltipLine line)
		{
			if (EnhancedTooltip.Instance.GetConfig<EnhancedTooltipConfig>().UseTwoColumnLines && ModuleManager.Tooltips.ContainsKey(line.Name)) return ModuleManager.Tooltips[line.Name].Create(item, line);

			return new TwoColumnLine(line)
			{
				textLeft = line.text
			};
		}

		public void Draw(SpriteBatch spriteBatch, float width)
		{
			Vector2 position = new Vector2(X + 8f, Y + 8f);

			if (!string.IsNullOrWhiteSpace(textLeft))
			{
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Font, textLeft, position, colorLeft, Rotation, Origin, scaleLeft, MaxWidth, Spread);
			}

			if (!string.IsNullOrWhiteSpace(textRight))
			{
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Font, textRight, position + new Vector2(width - 16f, 0), colorRight, Rotation, new Vector2(Font.MeasureString(textRight).X, 0), scaleRight, MaxWidth, Spread);
			}
		}
	}
}