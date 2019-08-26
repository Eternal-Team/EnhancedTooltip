using EnhancedTooltip.Tooltip.Common;
using EnhancedTooltip.Tooltip.Consumable;
using EnhancedTooltip.Tooltip.Equipable;
using EnhancedTooltip.Tooltip.Tool;
using EnhancedTooltip.Tooltip.Weapon;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace EnhancedTooltip.Tooltip
{
	public static class TooltipManager
	{
		public static IEnumerable<BaseTooltipLine> GetLines(Player player, Item item)
		{
			// todo: have get bool for visible in BaseTooltipLine and dynamically load all subclasses of BaseTooltipLine

			List<BaseTooltipLine> lines = new List<BaseTooltipLine>();

			lines.Add(new ItemNameLine());
			if (item.favorited) lines.Add(new FavoriteLine());
			if (item.social) lines.Add(new SocialLine());
			else
			{
				if (item.damage > 0 && (!item.notAmmo || item.useStyle > 0) && (item.type < 71 || item.type > 74 || player.HasItem(905)))
				{
					lines.Add(new DamageLine(item));

					if (!item.summon) lines.Add(new CritLine(item));

					if (item.useStyle > 0 && !item.summon) lines.Add(new SpeedLine(item));
				}

				if (item.fishingPole > 0) lines.Add(new FishingRodLine());

				if (item.bait > 0) lines.Add(new BaitLine());

				if (item.headSlot > 0 || item.bodySlot > 0 || item.legSlot > 0 || item.accessory || Main.projHook[item.shoot] || item.mountType != -1 || item.buffType > 0 && (Main.lightPet[item.buffType] || Main.vanityPet[item.buffType])) lines.Add(new EquipableLine());

				if (item.tileWand > 0) lines.Add(new TileWandLine());

				if (item.questItem) lines.Add(new QuestItemLine());

				if (item.vanity) lines.Add(new VanityLine());

				if (item.defense > 0) lines.Add(new DefenseLine());

				if (item.pick > 0) lines.Add(new PickaxeLine());

				if (item.axe > 0) lines.Add(new AxeLine());

				if (item.hammer > 0) lines.Add(new HammerLine());

				if (item.tileBoost != 0) lines.Add(new TileBoostLine());

				if (item.healLife > 0) lines.Add(new HealLifeLine());

				if (item.healMana > 0) lines.Add(new HealManaLine());

				if (item.mana > 0 && (item.type != 127 || !player.spaceGun)) lines.Add(new UseManaLine());

				if (item.createWall > 0 || item.createTile > -1)
				{
					if (item.type != 213 && item.tileWand < 1) lines.Add(new PlaceableLine());
				}
				else if (item.ammo > 0 && !item.notAmmo) lines.Add(new AmmoLine());
				else if (item.consumable) lines.Add(new ConsumableLine());

				if (item.material) lines.Add(new MaterialLine());

				if ((item.type == 3818 || item.type == 3819 || item.type == 3820 || item.type == 3824 || item.type == 3825 || item.type == 3826 || item.type == 3829 || item.type == 3830 || item.type == 3831 || item.type == 3832 || item.type == 3833 || item.type == 3834) && !player.downedDD2EventAnyDifficulty) lines.Add(new EtherianManaWarningLine());

				if (item.buffType == BuffID.WellFed && Main.expertMode) lines.Add(new WellFedExpertLine());

				if (item.buffTime > 0) lines.Add(new BuffTimeLine());

				if (item.type == 3262 || item.type == 3282 || item.type == 3283 || item.type == 3284 || item.type == 3285 || item.type == 3286 || item.type == 3316 || item.type == 3315 || item.type == 3317 || item.type == 3291 || item.type == 3389) lines.Add(new OneDropLogoLine());

				if (item.prefix > 0) lines.Add(new PrefixLine(item));

				if (Main.HoverItem.wornArmor && Main.player[Main.myPlayer].setBonus != "") lines.Add(new SetBonusLine());

				if (item.ToolTip != null)
				{
					for (int j = 0; j < item.ToolTip.Lines; j++)
					{
						if (j == 0 && item.type >= 1533 && item.type <= 1537 && !NPC.downedPlantBoss)
						{
							lines.Add(new TextLine
							{
								Text = Language.GetTextValue("LegacyTooltip.59")
							});
						}
						else lines.Add(new Common.TooltipLine(j));
					}
				}

				/*
					bool canApplyDiscount = true;
					int num62 = Main.reforgeItem.value;

					if (ItemLoader.ReforgePrice(Main.reforgeItem, ref num62, ref canApplyDiscount))
					{
					if (canApplyDiscount && Main.player[Main.myPlayer].discount)
					{
					num62 = (int)((double)num62 * 0.8);
					}
					num62 /= 3;
					}
				*/
			}

			if (item.expert) lines.Add(new ExpertLine());

			if (Main.npcShop > 0 && (item.shopSpecialCurrency != -1 || item.GetStoreValue() > 0 || item.type != ItemID.DefenderMedal)) lines.Add(new PriceLine(item));

			EnhancedTooltipItem.ModifyTooltips(item, lines);

			lines.ForEach(line =>
			{
				line.Player = player;
				line.Item = item;
			});

			return lines;
		}
	}
}