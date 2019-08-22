using BaseLibrary;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EnhancedTooltip.Module
{
	internal class BuffTimeModule : BaseModule
	{
		internal override string Name => "BuffTime";

		internal override TwoColumnLine Create(Item item, DrawableTooltipLine line)
		{
			TimeSpan span = TimeSpan.FromSeconds(item.buffTime / 60f);

			string text = "";
			if (span.Hours > 0) text += $"{span.Hours} hour{(span.Hours > 1 ? "s" : "")} ";
			if (span.Minutes > 0) text += $"{span.Minutes} minute{(span.Minutes > 1 ? "s" : "")} ";
			if (span.Seconds > 0) text += $"{span.Seconds} second{(span.Seconds > 1 ? "s" : "")} ";
			text = text.Trim();

			return new TwoColumnLine(line)
			{
				textLeft = "Buff duration",
				textRight = text,
				colorRight = Utility.DoubleLerp(Color.Red, Color.Yellow, Color.LimeGreen, item.buffTime / EnhancedTooltip.GetStat(EnhancedTooltip.Stat.BuffTime))
			};
		}
	}
}