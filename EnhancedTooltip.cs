using BaseLibrary;
using EnhancedTooltip.Module;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static EnhancedTooltip.EnhancedTooltip.Stat;

namespace EnhancedTooltip
{
	public class EnhancedTooltip : Mod
	{
		internal static EnhancedTooltip Instance;

		internal static Texture2D textureRarityBack;

		internal static Dictionary<Stat, float> MaxItemStats;

		public override void Load()
		{
			Instance = this;

			Hooking.Load();
			ModuleManager.Load();

			if (!Main.dedServ) textureRarityBack = ModContent.GetTexture("EnhancedTooltip/Textures/RarityBack");
		}

		internal static float GetStat(Stat stat) => MaxItemStats.TryGetValue(stat, out float value) ? value : 1f;

		internal enum Stat
		{
			PickaxePower,
			AxePower,
			HammerPower,
			FishingRodPower,
			BaitPower,
			TileBoost,

			UseMana,

			MeleeCrit,
			RangedCrit,
			MagicCrit,
			ThrownCrit,

			MeleeSpeed,
			RangedItemSpeed,
			RangedAmmoSpeed,
			MagicSpeed,
			SummonSpeed,
			ThrownSpeed,

			MeleeKnockback,
			RangedItemKnockback,
			RangedAmmoKnockback,
			MagicKnockback,
			SummonKnockback,
			ThrownKnockback,

			MeleeDamage,

			// todo: per different type? arrow, buller, dart...
			RangedItemDamage,
			RangedAmmoDamage,
			MagicDamage,
			SummonDamage,
			ThrownDamage,

			Defense,
			BuffTime,
			HealLife,
			HealMana
		}

		public override void PostSetupContent()
		{
			MaxItemStats = new Dictionary<Stat, float>
			{
				[PickaxePower] = 0,
				[AxePower] = 0,
				[HammerPower] = 0,
				[FishingRodPower] = 0,
				[BaitPower] = 0,
				[TileBoost] = 0,

				[UseMana] = 0,

				[MeleeCrit] = 0,
				[RangedCrit] = 0,
				[MagicCrit] = 0,
				[ThrownCrit] = 0,

				[MeleeSpeed] = 0,
				[RangedItemSpeed] = 0,
				[RangedAmmoSpeed] = 0,
				[MagicSpeed] = 0,
				[SummonSpeed] = 0,
				[ThrownSpeed] = 0,

				[MeleeKnockback] = 0,
				[RangedItemKnockback] = 0,
				[RangedAmmoKnockback] = 0,
				[MagicKnockback] = 0,
				[SummonKnockback] = 0,
				[ThrownKnockback] = 0,

				[MeleeDamage] = 0,
				[RangedItemDamage] = 0,
				[RangedAmmoDamage] = 0,
				[MagicDamage] = 0,
				[ThrownDamage] = 0,
				[SummonDamage] = 0,

				[Defense] = 0,
				[HealLife] = 0,
				[HealMana] = 0,
				[BuffTime] = 0
			};

			foreach (Item item in Utility.Cache.ItemCache)
			{
				if (item == null) continue;

				if (item.pick > MaxItemStats[PickaxePower]) MaxItemStats[PickaxePower] = item.pick;
				if (item.axe > MaxItemStats[AxePower]) MaxItemStats[AxePower] = item.axe;
				if (item.hammer > MaxItemStats[HammerPower]) MaxItemStats[HammerPower] = item.hammer;
				if (item.fishingPole > MaxItemStats[FishingRodPower]) MaxItemStats[FishingRodPower] = item.fishingPole;
				if (item.bait > MaxItemStats[BaitPower]) MaxItemStats[BaitPower] = item.bait;
				if (item.tileBoost > MaxItemStats[TileBoost]) MaxItemStats[TileBoost] = item.tileBoost;

				if (item.mana > MaxItemStats[UseMana]) MaxItemStats[UseMana] = item.mana;

				if (item.melee && item.crit > MaxItemStats[MeleeCrit]) MaxItemStats[MeleeCrit] = item.crit;
				if (item.ranged && item.crit > MaxItemStats[RangedCrit]) MaxItemStats[RangedCrit] = item.crit;
				if (item.magic && item.crit > MaxItemStats[MagicCrit]) MaxItemStats[MagicCrit] = item.crit;
				if (item.thrown && item.crit > MaxItemStats[ThrownCrit]) MaxItemStats[ThrownCrit] = item.crit;

				if (item.melee && item.useAnimation > MaxItemStats[MeleeSpeed]) MaxItemStats[MeleeSpeed] = item.useAnimation;
				if (item.ranged && item.ammo == AmmoID.None && item.useAnimation > MaxItemStats[RangedItemSpeed]) MaxItemStats[RangedItemSpeed] = item.useAnimation;
				if (item.ranged && item.ammo != AmmoID.None && item.shootSpeed > MaxItemStats[RangedAmmoSpeed]) MaxItemStats[RangedAmmoSpeed] = item.shootSpeed;
				if (item.magic && item.useAnimation > MaxItemStats[MagicSpeed]) MaxItemStats[MagicSpeed] = item.useAnimation;
				if (item.summon && item.useAnimation > MaxItemStats[SummonSpeed]) MaxItemStats[SummonSpeed] = item.useAnimation;
				if (item.thrown && item.useAnimation > MaxItemStats[ThrownSpeed]) MaxItemStats[ThrownSpeed] = item.useAnimation;

				if (item.melee && item.knockBack > MaxItemStats[MeleeKnockback]) MaxItemStats[MeleeKnockback] = item.knockBack;
				if (item.ranged && item.ammo == AmmoID.None && item.knockBack > MaxItemStats[RangedItemKnockback]) MaxItemStats[RangedItemKnockback] = item.knockBack;
				if (item.ranged && item.ammo != AmmoID.None && item.knockBack > MaxItemStats[RangedAmmoKnockback]) MaxItemStats[RangedAmmoKnockback] = item.knockBack;
				if (item.magic && item.knockBack > MaxItemStats[MagicKnockback]) MaxItemStats[MagicKnockback] = item.knockBack;
				if (item.summon && item.knockBack > MaxItemStats[SummonKnockback]) MaxItemStats[SummonKnockback] = item.knockBack;
				if (item.thrown && item.knockBack > MaxItemStats[ThrownKnockback]) MaxItemStats[ThrownKnockback] = item.knockBack;

				if (item.melee && item.damage > MaxItemStats[MeleeDamage]) MaxItemStats[MeleeDamage] = item.damage;
				if (item.ranged && item.ammo == AmmoID.None && item.damage > MaxItemStats[RangedItemDamage]) MaxItemStats[RangedItemDamage] = item.damage;
				if (item.ranged && item.ammo != AmmoID.None && item.damage > MaxItemStats[RangedAmmoDamage]) MaxItemStats[RangedAmmoDamage] = item.damage;
				if (item.magic && item.damage > MaxItemStats[MagicDamage]) MaxItemStats[MagicDamage] = item.damage;
				if (item.summon && item.damage > MaxItemStats[SummonDamage]) MaxItemStats[SummonDamage] = item.damage;
				if (item.thrown && item.damage > MaxItemStats[ThrownDamage]) MaxItemStats[ThrownDamage] = item.damage;

				if (item.defense > MaxItemStats[Defense]) MaxItemStats[Defense] = item.defense;
				if (item.healLife > MaxItemStats[HealLife]) MaxItemStats[HealLife] = item.healLife;
				if (item.healMana > MaxItemStats[HealMana]) MaxItemStats[HealMana] = item.healMana;
				if (item.buffTime > MaxItemStats[BuffTime]) MaxItemStats[BuffTime] = item.buffTime;
			}
		}
	}
}