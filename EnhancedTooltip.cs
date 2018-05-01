using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ModLoader;
using TheOneLibrary.Base;
using static TheOneLibrary.Utils.Utility;

namespace EnhancedTooltip
{
	public class EnhancedTooltip : Mod
	{
		[Null] public static EnhancedTooltip Instance;

		[Null] public static ModHotKey MoreInfo;
		[Null] public static ModHotKey PrefixInfo;

		public int maxPowerPick, maxPowerAxe, maxPowerHammer, maxPowerRod, maxPowerBait, maxManaCost, maxDamage, maxDefense;

		public override void Load()
		{
			Instance = this;

			MoreInfo = this.Register("Show more info", Keys.RightShift);
			PrefixInfo = this.Register("Show prefix info", Keys.RightControl);
		}

		public override void Unload()
		{
			UnloadNullableTypes();
		}

		public override void PostSetupContent()
		{
			List<Item> items = new List<Item>();

			for (int i = 0; i < ItemLoader.ItemCount; i++)
			{
				Item item = new Item();
				item.SetDefaults(i);
				items.Add(item);
			}

			maxPowerPick = items.Select(x => x.pick).Max();
			maxPowerAxe = items.Select(x => x.axe).Max();
			maxPowerHammer = items.Select(x => x.hammer).Max();
			maxPowerRod = items.Select(x => x.fishingPole).Max();
			maxPowerBait = items.Select(x => x.bait).Max();
			maxManaCost = items.Select(x => x.mana).Max();
			maxDamage = items.Select(x => x.damage).Max();
			maxDefense = items.Select(x => x.defense).Max();

			items.Clear();
		}
	}
}