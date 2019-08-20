using Microsoft.Xna.Framework;
using System.ComponentModel;
using Terraria.ModLoader.Config;
using Terraria.UI;

namespace EnhancedTooltip
{
	// todo: add lang entries
	public class EnhancedTooltipConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[DefaultValue(typeof(Color), "73, 94, 171, 255")] [Label("Tooltip background color")]
		public Color TooltipPanelColor;

		[DefaultValue(true)] [Label("Draw rarity background")]
		public bool DrawRarityBack;

		[DefaultValue(true)] [Label("Pulse tooltip text")]
		public bool TooltipTextPulse;

		[DefaultValue(true)] [Label("Draw favorite icon instead of text")]
		public bool FavoriteUseIcon;

		[DefaultValue(1f)] [Label("Scale of item name")]
		public float ItemNameScale;

		[DefaultValue(0.8f)] [Label("Scale of tooltip text")]
		public float TooltipTextScale;

		[DefaultValue(0.9f)] [Label("Scale of other text")]
		public float OtherTextScale;

		[DefaultValue(0.5f)] [Range(0f, 1f)] [Label("Alpha for rarity background")]
		public float RarityBackAlpha;

		[Label("Valid contexts for rendering rarity background")]
		public SubConfigExample RarityBackContexts = new SubConfigExample();

		[SeparatePage]
		public class SubConfigExample
		{
			public bool IsContextSet(int context)
			{
				switch (context)
				{
					case ItemSlot.Context.InventoryItem:
						return InventoryItem;
					case ItemSlot.Context.InventoryCoin:
						return InventoryCoin;
					case ItemSlot.Context.InventoryAmmo:
						return InventoryAmmo;
					case ItemSlot.Context.ChestItem:
						return ChestItem;
					case ItemSlot.Context.BankItem:
						return BankItem;
					case ItemSlot.Context.PrefixItem:
						return PrefixItem;
					case ItemSlot.Context.TrashItem:
						return TrashItem;
					case ItemSlot.Context.GuideItem:
						return GuideItem;
					case ItemSlot.Context.EquipArmor:
						return EquipArmor;
					case ItemSlot.Context.EquipArmorVanity:
						return EquipArmorVanity;
					case ItemSlot.Context.EquipAccessory:
						return EquipAccessory;
					case ItemSlot.Context.EquipAccessoryVanity:
						return EquipAccessoryVanity;
					case ItemSlot.Context.EquipDye:
						return EquipDye;
					case ItemSlot.Context.HotbarItem:
						return HotbarItem;
					case ItemSlot.Context.ChatItem:
						return ChatItem;
					case ItemSlot.Context.ShopItem:
						return ShopItem;
					case ItemSlot.Context.EquipGrapple:
						return EquipGrapple;
					case ItemSlot.Context.EquipMount:
						return EquipMount;
					case ItemSlot.Context.EquipMinecart:
						return EquipMinecart;
					case ItemSlot.Context.EquipPet:
						return EquipPet;
					case ItemSlot.Context.EquipLight:
						return EquipLight;
					case ItemSlot.Context.MouseItem:
						return MouseItem;
					case ItemSlot.Context.CraftingMaterial:
						return CraftingMaterial;
				}

				return false;
			}

			[DefaultValue(true)] public bool InventoryItem;
			[DefaultValue(true)] public bool InventoryCoin;
			[DefaultValue(true)] public bool InventoryAmmo;
			[DefaultValue(true)] public bool ChestItem;
			[DefaultValue(true)] public bool BankItem;
			public bool PrefixItem;
			public bool TrashItem;
			public bool GuideItem;
			[DefaultValue(true)] public bool EquipArmor;
			public bool EquipArmorVanity;
			[DefaultValue(true)] public bool EquipAccessory;
			public bool EquipAccessoryVanity;
			public bool EquipDye;
			[DefaultValue(true)] public bool HotbarItem;
			public bool ChatItem;
			[DefaultValue(true)] public bool ShopItem;
			public bool EquipGrapple;
			public bool EquipMount;
			public bool EquipMinecart;
			public bool EquipPet;
			public bool EquipLight;
			public bool MouseItem;
			public bool CraftingMaterial;
		}
	}
}