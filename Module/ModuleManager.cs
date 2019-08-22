using System;
using System.Collections.Generic;
using Terraria;

namespace EnhancedTooltip.Module
{
	internal static class ModuleManager
	{
		public static Dictionary<string, BaseModule> Tooltips;

		internal static void Load()
		{
			Tooltips = new Dictionary<string, BaseModule>();

			foreach (Type type in EnhancedTooltip.Instance.Code.GetTypes())
			{
				if (type.IsAbstract || !type.IsSubclassOf(typeof(BaseModule))) continue;

				BaseModule tooltip = (BaseModule)Activator.CreateInstance(type);
				Tooltips.Add(tooltip.Name, tooltip);
			}
		}
	}
}