using IL.Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace EnhancedTooltip
{
	internal static partial class Hooking
	{
		private static EnhancedTooltipConfig Config => EnhancedTooltip.Instance.GetConfig<EnhancedTooltipConfig>();

		private static Dictionary<int, Color> Rarities;

		internal static void Load()
		{
			On.Terraria.Main.MouseText_DrawItemTooltip += Main_MouseText_DrawItemTooltip;

			ItemSlot.Draw_SpriteBatch_ItemArray_int_int_Vector2_Color += ItemSlot_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color;

			//On.Terraria.Main.MouseText_DrawItemTooltip += Main_MouseText_DrawItemTooltip;

			Rarities = new Dictionary<int, Color>
			{
				[-11] = new Color(255, 175, 0),
				[-1] = new Color(130, 150, 255),
				[1] = new Color(150, 150, 255),
				[2] = new Color(150, 255, 150),
				[3] = new Color(255, 200, 150),
				[4] = new Color(255, 150, 150),
				[5] = new Color(255, 150, 255),
				[6] = new Color(210, 160, 255),
				[7] = new Color(150, 255, 10),
				[8] = new Color(255, 255, 10),
				[9] = new Color(5, 200, 255),
				[10] = new Color(255, 40, 100),
				[11] = new Color(180, 40, 255)
			};
		}

		private static void ItemSlot_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color(ILContext il)
		{
			ILCursor cursor = new ILCursor(il);
			Texture2D rarityBack = ModContent.GetTexture("EnhancedTooltip/Textures/RarityBack");
			Texture2D rarityBackFavorite = ModContent.GetTexture("EnhancedTooltip/Textures/RarityBackFavorite");
			Texture2D rarityBackNew = ModContent.GetTexture("EnhancedTooltip/Textures/RarityBackNew");

			if (cursor.TryGotoNext(i => i.MatchLdcI4(-1), i => i.MatchStloc(10)))
			{
				cursor.Emit(OpCodes.Ldarg, 0);
				cursor.Emit(OpCodes.Ldloc, 1);
				cursor.Emit(OpCodes.Ldarg, 4);
				cursor.Emit(OpCodes.Ldloc, 2);
				cursor.Emit(OpCodes.Ldarg, 2);

				cursor.EmitDelegate<Action<SpriteBatch, Item, Vector2, float, int>>((spriteBatch, item, position, scale, context) =>
				{
					if (!item.IsAir && Config.DrawRarityBack && Config.RarityBackContexts.IsContextSet(context))
					{
						Color color = GetRarityColor(item) * Config.RarityBackAlpha;
						if (Config.TooltipTextPulse) color *= Main.mouseTextColor / 255f;

						bool validContext = context != 13 && context != 21 && context != 14 && context != 22;
						Texture2D texture;
						if (Terraria.UI.ItemSlot.Options.HighlightNewItems && item.newAndShiny && validContext) texture = rarityBackNew;
						else if (item.favorited && validContext) texture = rarityBackFavorite;
						else texture = rarityBack;

						spriteBatch.Draw(texture, position, null, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
					}
				});
			}
		}

		private static void Main_MouseText_DrawItemTooltip(On.Terraria.Main.orig_MouseText_DrawItemTooltip orig, Main self, int rare, byte diff, int X, int Y)
		{
			byte prevColor = Main.mouseTextColor;
			if (!Config.TooltipTextPulse) Main.mouseTextColor = 250;

			DrawTooltip(diff, X, Y);

			Main.mouseTextColor = prevColor;
		}

		public static void RegisterRarityColor(int rarity, Color color)
		{
			if (!Rarities.ContainsKey(rarity)) Rarities.Add(rarity, color);
		}

		internal static Color GetRarityColor(Item item)
		{
			if (Rarities.ContainsKey(item.rare)) return Rarities[item.rare];
			if (item.expert || item.rare == -12) return Main.DiscoColor;
			return item.rare > 11 ? Rarities[11] : Color.White;
		}
	}
}