using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EnhancedTooltip
{
	public class ETPlayer : ModPlayer
	{
		public static bool MoreInfo;
		public static bool PrefixInfo;

		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (EnhancedTooltip.MoreInfo.JustPressed) MoreInfo = !MoreInfo;
			if (EnhancedTooltip.PrefixInfo.JustPressed) PrefixInfo = !PrefixInfo;
		}

		public override TagCompound Save() => new TagCompound
		{
			["MoreInfo"] = MoreInfo,
			["PrefixInfo"] = PrefixInfo
		};

		public override void Load(TagCompound tag)
		{
			MoreInfo = tag.GetBool("MoreInfo");
			PrefixInfo = tag.GetBool("PrefixInfo");
		}
	}
}