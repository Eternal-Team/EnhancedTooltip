using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI.Chat;

namespace EnhancedTooltip.Tooltip
{
	public class TwoColumnLine
	{
		public string textLeft, textRight;
		public string measureableTextLeft;
		public Color colorLeft, colorRight;

		public Vector2 scaleL, scaleR;

		public Vector2 position = Vector2.Zero;

		public TwoColumnLine(string text) : this(text, "", Color.White, Color.White, Vector2.One, Vector2.One)
		{
		}

		public TwoColumnLine(string text, Vector2 scale) : this(text, "", Color.White, Color.White, scale, scale)
		{
		}

		public TwoColumnLine(string text, Color c, Vector2 scale = default(Vector2)) : this(text, "", c, c, scale, scale)
		{
		}

		public TwoColumnLine(string textLeft, string textRight) : this(textLeft, textRight, Color.White, Color.White, Vector2.One, Vector2.One)
		{
		}

		public TwoColumnLine(string textLeft, string textRight, Vector2 scale) : this(textLeft, textRight, Color.White, Color.White, scale, scale)
		{
		}

		public TwoColumnLine(string textLeft, string textRight, Vector2 scaleL, Vector2 scaleR) : this(textLeft, textRight, Color.White, Color.White, scaleL, scaleR)
		{
		}

		public TwoColumnLine(string textLeft, string textRight, Color color) : this(textLeft, textRight, color, color, Vector2.One, Vector2.One)
		{
		}

		public TwoColumnLine(string textLeft, string textRight, Color color, Vector2 scale) : this(textLeft, textRight, color, color, scale, scale)
		{
		}

		public TwoColumnLine(string textLeft, string textRight, Color color, Vector2 scaleL, Vector2 scaleR) : this(textLeft, textRight, color, color, scaleL, scaleR)
		{
		}

		public TwoColumnLine(string textLeft, string textRight, Color colorLeft, Color colorRight) : this(textLeft, textRight, colorLeft, colorRight, Vector2.One, Vector2.One)
		{
		}

		public TwoColumnLine(string textLeft, string textRight, Color colorLeft, Color colorRight, Vector2 scale) : this(textLeft, textRight, colorLeft, colorRight, scale, scale)
		{
		}

		public TwoColumnLine(string textLeft, string textRight, Color colorLeft, Color colorRight, Vector2 scaleL, Vector2 scaleR)
		{
			this.textLeft = textLeft;
			this.textRight = textRight;
			this.colorLeft = colorLeft;
			this.colorRight = colorRight;
			this.scaleL = scaleL;
			this.scaleR = scaleR;
		}

		public void Draw(SpriteBatch sb, Rectangle box)
		{
			position.X = (int)position.X;
			position.Y = (int)position.Y;

			if (string.IsNullOrEmpty(measureableTextLeft)) measureableTextLeft = textLeft;

			Vector2 sizeL = Main.fontMouseText.MeasureString(measureableTextLeft) * scaleL;
			Vector2 sizeR = Main.fontMouseText.MeasureString(textRight) * scaleR;

			ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, textLeft, position, colorLeft, 0f, Vector2.Zero, scaleL);
			ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, textRight, position + new Vector2(box.Width - 24 - sizeR.X, 0), colorRight, 0f, Vector2.Zero, scaleR);

			if (box.Width < sizeL.X + sizeR.X + 32) box.Width = (int)(sizeL.X + sizeR.X + 32);
		}
	}
}