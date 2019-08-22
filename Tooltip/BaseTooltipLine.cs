using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;
using Terraria;

namespace EnhancedTooltip.Tooltip
{
	public class BaseTooltipLine
	{
		public static EnhancedTooltipConfig Config => EnhancedTooltip.Instance.GetConfig<EnhancedTooltipConfig>();

		public Player Player;
		public Item Item;

		public Vector2 Position;

		public virtual Vector2 GetSize() => Vector2.Zero;

		public virtual int GetBottomMargin(ReadOnlyCollection<BaseTooltipLine> lines) => 0;

		public virtual int GetTopMargin(ReadOnlyCollection<BaseTooltipLine> lines) => 0;

		public virtual void Draw(SpriteBatch spriteBatch, float maxWidth)
		{
		}
	}
}