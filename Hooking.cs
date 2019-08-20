using IL.Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using On.Terraria;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace EnhancedTooltip
{
	internal static class Hooking
	{
		private static EnhancedTooltipConfig Config => EnhancedTooltip.Instance.GetConfig<EnhancedTooltipConfig>();

		private static Dictionary<int, Color> Rarities;

		internal static void Load()
		{
			Main.MouseText_DrawItemTooltip += ModifyColorPulse;

			ItemSlot.Draw_SpriteBatch_ItemArray_int_int_Vector2_Color += ItemSlot_Draw_SpriteBatch_ItemArray_int_int_Vector2_Color;

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

			if (cursor.TryGotoNext(i => i.MatchLdcI4(-1), i => i.MatchStloc(10)))
			{
				cursor.Emit(OpCodes.Ldarg, 0);
				cursor.Emit(OpCodes.Ldloc, 1);
				cursor.Emit(OpCodes.Ldarg, 4);
				cursor.Emit(OpCodes.Ldloc, 2);
				cursor.Emit(OpCodes.Ldarg, 2);

				cursor.EmitDelegate<Action<SpriteBatch, Terraria.Item, Vector2, float, int>>((spriteBatch, item, position, scale, context) =>
				{
					// todo: fix inventoryback10 and inventoryback15

					if (Config.DrawRarityBack && Config.RarityBackContexts.IsContextSet(context))
					{
						Color color = GetRarityColor(item) * Config.RarityBackAlpha;
						if (Config.TooltipTextPulse) color *= Terraria.Main.mouseTextColor / 255f;

						spriteBatch.Draw(rarityBack, position, null, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
					}
				});
			}
		}

		private static void ModifyColorPulse(Main.orig_MouseText_DrawItemTooltip orig, Terraria.Main self, int rare, byte diff, int X, int Y)
		{
			byte prevColor = Terraria.Main.mouseTextColor;
			if (!Config.TooltipTextPulse) Terraria.Main.mouseTextColor = 250;

			orig(self, rare, diff, X, Y);

			Terraria.Main.mouseTextColor = prevColor;
		}

		// note: a way to register custom rarity?
		private static Color GetRarityColor(Terraria.Item item)
		{
			if (Rarities.ContainsKey(item.rare)) return Rarities[item.rare];
			if (item.expert || item.rare == -12) return Terraria.Main.DiscoColor;
			return item.rare > 11 ? Rarities[11] : Color.White;
		}
	}
}