//using Microsoft.Xna.Framework;
//using Terraria;
//using Terraria.ModLoader;

//namespace EnhancedTooltip.Tooltip
//{
//	public class BuffTimeTooltip
//	{
//		public static TwoColumnLine GetLine(DrawableTooltipLine line, Item item)
//		{
//			string buffText = "";

//			if (item.buffTime > 0) buffText = $"{(item.buffTime > 60 ? item.buffTime / 3600 : item.buffTime / 60)} {(item.buffTime > 3600 ? "minute" : "second")}{(item.buffTime >= (item.buffTime > 60 * 60 ? 2 * 60 * 60 : 2 * 60) ? "s" : "")}";

//			return new TwoColumnLine("Duration: ", buffText, line.color, Color.Lime, line.baseScale);
//		}
//	}
//}

