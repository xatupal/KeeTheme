using System.ComponentModel;
using System.Drawing;
using KeeTheme.Editor;

namespace KeeTheme.Theme
{
	[TypeConverter(typeof(ToolStripLookTypeConverter))]
	class ToolStripLook
	{
		public Color ButtonSelectedHighlight { get; set; }
		public Color ButtonSelectedHighlightBorder { get; set; }
		public Color ButtonPressedHighlight { get; set; }
		public Color ButtonPressedHighlightBorder { get; set; }
		public Color ButtonCheckedHighlight { get; set; }
		public Color ButtonCheckedHighlightBorder { get; set; }
		public Color ButtonPressedBorder { get; set; }
		public Color ButtonSelectedBorder { get; set; }
		public Color ButtonCheckedGradientBegin { get; set; }
		public Color ButtonCheckedGradientMiddle { get; set; }
		public Color ButtonCheckedGradientEnd { get; set; }
		public Color ButtonSelectedGradientBegin { get; set; }
		public Color ButtonSelectedGradientMiddle { get; set; }
		public Color ButtonSelectedGradientEnd { get; set; }
		public Color ButtonPressedGradientBegin { get; set; }
		public Color ButtonPressedGradientMiddle { get; set; }
		public Color ButtonPressedGradientEnd { get; set; }
		public Color CheckBackground { get; set; }
		public Color CheckSelectedBackground { get; set; }
		public Color CheckPressedBackground { get; set; }
		public Color GripDark { get; set; }
		public Color GripLight { get; set; }
		public Color ImageMarginGradientBegin { get; set; }
		public Color ImageMarginGradientMiddle { get; set; }
		public Color ImageMarginGradientEnd { get; set; }
		public Color ImageMarginRevealedGradientBegin { get; set; }
		public Color ImageMarginRevealedGradientMiddle { get; set; }
		public Color ImageMarginRevealedGradientEnd { get; set; }
		public Color MenuStripGradientBegin { get; set; }
		public Color MenuStripGradientEnd { get; set; }
		public Color MenuItemSelected { get; set; }
		public Color MenuItemBorder { get; set; }
		public Color MenuBorder { get; set; }
		public Color MenuItemSelectedGradientBegin { get; set; }
		public Color MenuItemSelectedGradientEnd { get; set; }
		public Color MenuItemPressedGradientBegin { get; set; }
		public Color MenuItemPressedGradientMiddle { get; set; }
		public Color MenuItemPressedGradientEnd { get; set; }
		public Color RaftingContainerGradientBegin { get; set; }
		public Color RaftingContainerGradientEnd { get; set; }
		public Color SeparatorDark { get; set; }
		public Color SeparatorLight { get; set; }
		public Color StatusStripGradientBegin { get; set; }
		public Color StatusStripGradientEnd { get; set; }
		public Color ToolStripBorder { get; set; }
		public Color ToolStripDropDownBackground { get; set; }
		public Color ToolStripGradientBegin { get; set; }
		public Color ToolStripGradientMiddle { get; set; }
		public Color ToolStripGradientEnd { get; set; }
		public Color ToolStripContentPanelGradientBegin { get; set; }
		public Color ToolStripContentPanelGradientEnd { get; set; }
		public Color ToolStripPanelGradientBegin { get; set; }
		public Color ToolStripPanelGradientEnd { get; set; }
		public Color OverflowButtonGradientBegin { get; set; }
		public Color OverflowButtonGradientMiddle { get; set; }
		public Color OverflowButtonGradientEnd { get; set; }

		public override string ToString()
		{
			return GetType().Name;
		}
	}
}