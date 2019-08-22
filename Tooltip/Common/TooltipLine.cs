using System.Collections.ObjectModel;
using System.Linq;
using Terraria;

namespace EnhancedTooltip.Tooltip.Common
{
	public class TooltipLine : BaseSimpleLine
	{
		public override string Text => Item.ToolTip.GetLine(Index);

		public int Index;

		public TooltipLine(int index) => Index = index;

		public override int GetTopMargin(ReadOnlyCollection<BaseTooltipLine> lines) => lines.FirstOrDefault(line => line is TooltipLine) == this ? 8 : 0;
	}
}