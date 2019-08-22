using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace EnhancedTooltip.Tooltip.Weapon
{
	public class OneDropLogoLine : BaseTooltipLine
	{
		public override Vector2 GetSize() => new Vector2(19, 28);

		public override void Draw(SpriteBatch spriteBatch, float maxWidth)
		{
			for (int j = 0; j < 5; j++)
			{
				float logoX = Position.X;
				float logoY = Position.Y;
				if (j == 0) logoX--;
				else if (j == 1) logoX++;
				else if (j == 2) logoY--;
				else if (j == 3) logoY++;

				Main.spriteBatch.Draw(Main.oneDropLogo, new Vector2(logoX, logoY), null, j != 4 ? Color.Black : Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
	}
}