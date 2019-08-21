using BaseLibrary;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ModLoader.Config;
using Terraria.UI;

namespace EnhancedTooltip
{
	// todo: add lang entries
	public class EnhancedTooltipConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[DefaultValue(typeof(Color), "73, 94, 171, 255"), Label("Tooltip background color")]
		public Color TooltipPanelColor;

		[DefaultValue(true), Label("Draw rarity background")]
		public bool DrawRarityBack;

		[DefaultValue(true), Label("Pulse tooltip text")]
		public bool TooltipTextPulse;

		[DefaultValue(true), Label("Draw favorite icon instead of text")]
		public bool FavoriteUseIcon;

		[DefaultValue(false)]
		public bool ShowMaxStack;

		[DefaultValue(true)]
		public bool UseTwoColumnLines;

		[DefaultValue(true)]
		public bool ShowModName;

		[DefaultValue(1f), Label("Scale of item name")]
		public float ItemNameScale;

		[DefaultValue(0.9f), Label("Scale of tooltip text")]
		public float TooltipTextScale;

		[DefaultValue(0.9f), Label("Scale of other text")]
		public float OtherTextScale;

		[DefaultValue(0.7f), Range(0f, 1f), Label("Alpha for rarity background")]
		public float RarityBackAlpha;

		[Label("Valid contexts for rendering rarity background")]
		public RarityBackPage RarityBackContexts = new RarityBackPage();

		[SeparatePage]
		public class RarityBackPage
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

			[DefaultValue(true)]
			public bool InventoryItem;

			[DefaultValue(true)]
			public bool InventoryCoin;

			[DefaultValue(true)]
			public bool InventoryAmmo;

			[DefaultValue(true)]
			public bool ChestItem;

			[DefaultValue(true)]
			public bool BankItem;

			public bool PrefixItem;
			public bool TrashItem;
			public bool GuideItem;

			[DefaultValue(true)]
			public bool EquipArmor;

			public bool EquipArmorVanity;

			[DefaultValue(true)]
			public bool EquipAccessory;

			public bool EquipAccessoryVanity;
			public bool EquipDye;

			[DefaultValue(true)]
			public bool HotbarItem;

			public bool ChatItem;

			[DefaultValue(true)]
			public bool ShopItem;

			public bool EquipGrapple;
			public bool EquipMount;
			public bool EquipMinecart;
			public bool EquipPet;
			public bool EquipLight;
			public bool MouseItem;
			public bool CraftingMaterial;

			public override bool Equals(object obj)
			{
				if (obj is RarityBackPage other)
				{
					FieldInfo[] infos = GetType().GetFields(Utility.defaultFlags).Where(info => info.FieldType == typeof(bool)).ToArray();
					bool[] fields = infos.Select(info => info.GetValue(this)).Cast<bool>().ToArray();
					bool[] otherFields = infos.Select(info => info.GetValue(other)).Cast<bool>().ToArray();

					return fields.SequenceEqual(otherFields);
				}

				return false;
			}

			public override int GetHashCode()
			{
				FieldInfo[] infos = GetType().GetFields(Utility.defaultFlags).Where(info => info.FieldType == typeof(bool)).ToArray();
				return infos.Select(info => info.GetValue(this)).Cast<bool>().ToArray().GetHashCode();
			}
		}

		[Label("Formatting styles for numbers")]
		public NumberStylesPage NumberStyles = new NumberStylesPage();

		[SeparatePage]
		public class NumberStylesPage
		{
			public string FormatNumber(double value)
			{
				if (Normal) return value.ToString("F0");
				if (ThousandsSeparator) return value.ToString("N0");
				return Shortened ? value.ToSI("F1") : null;
			}

			private bool normal;
			private bool thousandsSeparator;
			private bool shortened;

			[DefaultValue(true)]
			public bool Normal
			{
				get => normal;
				set
				{
					if (value)
					{
						normal = true;
						thousandsSeparator = shortened = false;
					}
				}
			}

			public bool ThousandsSeparator
			{
				get => thousandsSeparator;
				set
				{
					if (value)
					{
						thousandsSeparator = true;
						normal = shortened = false;
					}
				}
			}

			public bool Shortened
			{
				get => shortened;
				set
				{
					if (value)
					{
						shortened = true;
						normal = thousandsSeparator = false;
					}
				}
			}

			public override bool Equals(object obj) => obj is NumberStylesPage other && Normal == other.Normal && ThousandsSeparator == other.ThousandsSeparator && Shortened == other.Shortened;

			public override int GetHashCode() => new { Normal, ThousandsSeparator, Shortened }.GetHashCode();
		}
	}
}