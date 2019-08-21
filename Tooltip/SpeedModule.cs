using BaseLibrary;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EnhancedTooltip.Tooltip
{
	internal class SpeedModule : BaseModule
	{
		internal override string Name => "Speed";

		internal override TwoColumnLine Create(Item item, DrawableTooltipLine line)
		{
			if (Config.TooltipUseValues)
			{
				return new TwoColumnLine(line)
				{
					textLeft = "Speed",
					textRight = item.useAnimation.ToString(),
					colorRight = Utility.DoubleLerp(Color.LimeGreen, Color.Yellow, Color.Red, (float)item.useAnimation / EnhancedTooltip.GetStat("Speed"))
				};
			}

			string text;
			if (item.useAnimation <= 8) text = Language.GetTextValue("LegacyTooltip.6");
			else if (item.useAnimation <= 20) text = Language.GetTextValue("LegacyTooltip.7");
			else if (item.useAnimation <= 25) text = Language.GetTextValue("LegacyTooltip.8");
			else if (item.useAnimation <= 30) text = Language.GetTextValue("LegacyTooltip.9");
			else if (item.useAnimation <= 35) text = Language.GetTextValue("LegacyTooltip.10");
			else if (item.useAnimation <= 45) text = Language.GetTextValue("LegacyTooltip.11");
			else if (item.useAnimation <= 55) text = Language.GetTextValue("LegacyTooltip.12");
			else text = Language.GetTextValue("LegacyTooltip.13");

			return new TwoColumnLine(line)
			{
				textLeft = text
			};
		}
	}
}