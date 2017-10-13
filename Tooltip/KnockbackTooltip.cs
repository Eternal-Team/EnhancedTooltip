using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EnhancedTooltip.Tooltip
{
	public class KnockbackTooltip
	{
		public static TwoColumnLine GetLine(DrawableTooltipLine line, Item item)
		{
			float knockback = item.knockBack;
			if (item.ranged && Main.LocalPlayer.stealth > 0) knockback *= 1f + (1f - Main.LocalPlayer.stealth) * 0.5f;
			if (item.summon) knockback += Main.LocalPlayer.minionKB;

			string knockbackText;
			if (knockback == 0) knockbackText = "No";
			else if (knockback <= 1.5f) knockbackText = "Extremely weak";
			else if (knockback <= 3f) knockbackText = "Very weak";
			else if (knockback <= 4f) knockbackText = "Weak";
			else if (knockback <= 6f) knockbackText = "Average";
			else if (knockback <= 7f) knockbackText = "Strong";
			else if (knockback <= 9f) knockbackText = "Very strong";
			else if (knockback <= 11f) knockbackText = "Extremely strong";
			else knockbackText = "Insane";

			return new TwoColumnLine("Knockback:", $"{knockbackText} ({knockback:0.0})", line.color, BaseLib.Utility.Utility.DoubleLerp(Color.Red, Color.Yellow, Color.Lime, knockback / 11f), line.baseScale);
		}
	}
}