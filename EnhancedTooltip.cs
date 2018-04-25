using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ModLoader;
using TheOneLibrary.Base;
using TheOneLibrary.Utils;

namespace EnhancedTooltip
{
	public class EnhancedTooltip : Mod
	{
		[Null] public static EnhancedTooltip Instance;

		[Null] public static ModHotKey MoreInfo;
		[Null] public static ModHotKey PrefixInfo;

		public int maxPowerPick, maxPowerAxe, maxPowerHammer, maxPowerRod, maxPowerBait, maxManaCost, maxDamage, maxDefense;

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

			MoreInfo = this.Register("Show more info", Keys.RightShift);
			PrefixInfo = this.Register("Show prefix info", Keys.RightControl);
		}

		public override void Unload()
		{
			this.UnloadNullableTypes();

			GC.Collect();
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