using Terraria;
using Terraria.ModLoader;

namespace EnhancedTooltip.Module
{
	internal abstract class BaseModule
	{
		internal static EnhancedTooltipConfig Config => EnhancedTooltip.Instance.GetConfig<EnhancedTooltipConfig>();

		internal abstract string Name { get; }

		internal abstract TwoColumnLine Create(Item item, DrawableTooltipLine line);
	}
}