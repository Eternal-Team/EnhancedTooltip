using BaseLibrary;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace EnhancedTooltip
{
	public class EnhancedTooltip : Mod
	{
		internal static EnhancedTooltip Instance;

		internal static Texture2D textureRarityBack;

		internal static Dictionary<string, int> MaxItemStats;

		public override void Load()
		{
			Instance = this;

			Hooking.Load();

			if (!Main.dedServ)
			{
				textureRarityBack = ModContent.GetTexture("EnhancedTooltip/Textures/RarityBack");
			}
		}

		public override void PostSetupContent()
		{
			// note: split different damage types, buff related maximums?

			// note: use one foreach loop instead of this
			MaxItemStats = new Dictionary<string, int>
			{
				["PickaxePower"] = Utility.Cache.ItemCache.Max(item => item.pick),
				["AxePower"] = Utility.Cache.ItemCache.Max(item => item.axe),
				["HammerPower"] = Utility.Cache.ItemCache.Max(item => item.hammer),
				["FishingRodPower"] = Utility.Cache.ItemCache.Max(item => item.fishingPole),
				["BaitPower"] = Utility.Cache.ItemCache.Max(item => item.bait),
				["ManaCost"] = Utility.Cache.ItemCache.Max(item => item.mana),
				["Damage"] = Utility.Cache.ItemCache.Max(item => item.damage),
				["Defense"] = Utility.Cache.ItemCache.Max(item => item.defense),
				["Crit"] = Utility.Cache.ItemCache.Max(item => item.crit)
			};
		}
	}
}