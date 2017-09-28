using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace EnhancedTooltip
{
	public class EnhancedTooltip : Mod
	{
		public static EnhancedTooltip Instance;

		public int maxPowerPick, maxPowerAxe, maxPowerHammer, maxPowerRod, maxPowerBait, maxManaCost, maxDamage, maxDefense;

		public static bool shiftInUse;
		public static bool controlInUse;

		public static Texture2D rarityBack;

		public EnhancedTooltip()
		{
			Properties = new ModProperties
			{
				Autoload = true,
				AutoloadBackgrounds = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}

		public override void Load()
		{
			Instance = this;

			rarityBack = ModLoader.GetTexture("EnhancedTooltip/RarityBack");
		}

		public override void Unload()
		{
			Instance = null;

			rarityBack = null;
		}

		public override void PostUpdateInput()
		{
			shiftInUse = Main.keyState.GetPressedKeys().Contains(Keys.RightShift);
			controlInUse = Main.keyState.GetPressedKeys().Contains(Keys.RightControl);
		}

		public override void PostSetupContent()
		{
			List<Item> items = new List<Item>();

			for (int i = 0; i < ItemLoader.ItemCount; i++)
			{
				Item item = new Item();
				item.SetDefaults(i, true);
				items.Add(item);
			}

			maxPowerPick = items.Select(x => x.pick).OrderByDescending(x => x).First();
			maxPowerAxe = items.Select(x => x.axe).OrderByDescending(x => x).First();
			maxPowerHammer = items.Select(x => x.hammer).OrderByDescending(x => x).First();
			maxPowerRod = items.Select(x => x.fishingPole).OrderByDescending(x => x).First();
			maxPowerBait = items.Select(x => x.bait).OrderByDescending(x => x).First();
			maxManaCost = items.Select(x => x.mana).OrderByDescending(x => x).First();
			maxDamage = items.Select(x => x.damage).OrderByDescending(x => x).First();
			maxDefense = items.Select(x => x.defense).OrderByDescending(x => x).First();

			items.Clear();
		}
	}
}