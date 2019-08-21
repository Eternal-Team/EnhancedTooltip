using BaseLibrary;
using EnhancedTooltip.Tooltip;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
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
			ModuleManager.Load();

			if (!Main.dedServ) textureRarityBack = ModContent.GetTexture("EnhancedTooltip/Textures/RarityBack");
		}

		public static int GetStat(string key) => MaxItemStats.TryGetValue(key, out int value) ? value : 1;

		public override void PostSetupContent()
		{
			// note: use enums?
			MaxItemStats = new Dictionary<string, int>
			{
				["PickaxePower"] = 0,
				["AxePower"] = 0,
				["HammerPower"] = 0,
				["FishingRodPower"] = 0,
				["BaitPower"] = 0,
				["Crit"] = 0,
				["Speed"] = 0,
				["Knockback"] = 0,
				["MeleeDamage"] = 0,
				["RangedDamage"] = 0,
				["MagicDamage"] = 0,
				["ThrownDamage"] = 0,
				["SummonDamage"] = 0,
				["Defense"] = 0,
				["UseMana"] = 0,
				["HealLife"] = 0,
				["HealMana"] = 0,
				["TileBoost"] = 0
			};

			foreach (Item item in Utility.Cache.ItemCache)
			{
				if (item == null) continue;

				if (item.pick > MaxItemStats["PickaxePower"]) MaxItemStats["PickaxePower"] = item.pick;
				if (item.axe > MaxItemStats["AxePower"]) MaxItemStats["AxePower"] = item.axe;
				if (item.hammer > MaxItemStats["HammerPower"]) MaxItemStats["HammerPower"] = item.hammer;
				if (item.fishingPole > MaxItemStats["FishingRodPower"]) MaxItemStats["FishingRodPower"] = item.fishingPole;
				if (item.bait > MaxItemStats["BaitPower"]) MaxItemStats["BaitPower"] = item.bait;

				if (item.mana > MaxItemStats["UseMana"]) MaxItemStats["UseMana"] = item.mana;
				if (item.crit > MaxItemStats["Crit"]) MaxItemStats["Crit"] = item.crit;
				if (item.useAnimation > MaxItemStats["Speed"]) MaxItemStats["Speed"] = item.useAnimation;
				if (item.knockBack > MaxItemStats["Knockback"]) MaxItemStats["Knockback"] = (int)item.knockBack;

				// todo: separate damage from ammos from weapons
				if (item.melee && item.damage > MaxItemStats["MeleeDamage"]) MaxItemStats["MeleeDamage"] = item.damage;
				if (item.ranged && item.damage > MaxItemStats["RangedDamage"]) MaxItemStats["RangedDamage"] = item.damage;
				if (item.magic && item.damage > MaxItemStats["MagicDamage"]) MaxItemStats["MagicDamage"] = item.damage;
				if (item.thrown && item.damage > MaxItemStats["ThrownDamage"]) MaxItemStats["ThrownDamage"] = item.damage;
				if (item.summon && item.damage > MaxItemStats["SummonDamage"]) MaxItemStats["SummonDamage"] = item.damage;

				if (item.defense > MaxItemStats["Defense"]) MaxItemStats["Defense"] = item.defense;

				if (item.healLife > MaxItemStats["HealLife"]) MaxItemStats["HealLife"] = item.healLife;
				if (item.healMana > MaxItemStats["HealMana"]) MaxItemStats["HealMana"] = item.healMana;

				if (item.tileBoost > MaxItemStats["TileBoost"]) MaxItemStats["TileBoost"] = item.tileBoost;
			}
		}
	}
}