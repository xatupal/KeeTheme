using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace KeeTheme.Theme
{
	class CustomColorTable : ProfessionalColorTable
	{
		private readonly Dictionary<string, Color> _customColors = new Dictionary<string, Color>();

		public override Color ButtonSelectedHighlight { get { return _customColors.ContainsKey("ButtonSelectedHighlight") ? _customColors["ButtonSelectedHighlight"] : base.ButtonSelectedHighlight; } }
		public override Color ButtonSelectedHighlightBorder { get { return _customColors.ContainsKey("ButtonSelectedHighlightBorder") ? _customColors["ButtonSelectedHighlightBorder"] : base.ButtonSelectedHighlightBorder; } }
		public override Color ButtonPressedHighlight { get { return _customColors.ContainsKey("ButtonPressedHighlight") ? _customColors["ButtonPressedHighlight"] : base.ButtonPressedHighlight; } }
		public override Color ButtonPressedHighlightBorder { get { return _customColors.ContainsKey("ButtonPressedHighlightBorder") ? _customColors["ButtonPressedHighlightBorder"] : base.ButtonPressedHighlightBorder; } }
		public override Color ButtonCheckedHighlight { get { return _customColors.ContainsKey("ButtonCheckedHighlight") ? _customColors["ButtonCheckedHighlight"] : base.ButtonCheckedHighlight; } }
		public override Color ButtonCheckedHighlightBorder { get { return _customColors.ContainsKey("ButtonCheckedHighlightBorder") ? _customColors["ButtonCheckedHighlightBorder"] : base.ButtonCheckedHighlightBorder; } }
		public override Color ButtonPressedBorder { get { return _customColors.ContainsKey("ButtonPressedBorder") ? _customColors["ButtonPressedBorder"] : base.ButtonPressedBorder; } }
		public override Color ButtonSelectedBorder { get { return _customColors.ContainsKey("ButtonSelectedBorder") ? _customColors["ButtonSelectedBorder"] : base.ButtonSelectedBorder; } }
		public override Color ButtonCheckedGradientBegin { get { return _customColors.ContainsKey("ButtonCheckedGradientBegin") ? _customColors["ButtonCheckedGradientBegin"] : base.ButtonCheckedGradientBegin; } }
		public override Color ButtonCheckedGradientMiddle { get { return _customColors.ContainsKey("ButtonCheckedGradientMiddle") ? _customColors["ButtonCheckedGradientMiddle"] : base.ButtonCheckedGradientMiddle; } }
		public override Color ButtonCheckedGradientEnd { get { return _customColors.ContainsKey("ButtonCheckedGradientEnd") ? _customColors["ButtonCheckedGradientEnd"] : base.ButtonCheckedGradientEnd; } }
		public override Color ButtonSelectedGradientBegin { get { return _customColors.ContainsKey("ButtonSelectedGradientBegin") ? _customColors["ButtonSelectedGradientBegin"] : base.ButtonSelectedGradientBegin; } }
		public override Color ButtonSelectedGradientMiddle { get { return _customColors.ContainsKey("ButtonSelectedGradientMiddle") ? _customColors["ButtonSelectedGradientMiddle"] : base.ButtonSelectedGradientMiddle; } }
		public override Color ButtonSelectedGradientEnd { get { return _customColors.ContainsKey("ButtonSelectedGradientEnd") ? _customColors["ButtonSelectedGradientEnd"] : base.ButtonSelectedGradientEnd; } }
		public override Color ButtonPressedGradientBegin { get { return _customColors.ContainsKey("ButtonPressedGradientBegin") ? _customColors["ButtonPressedGradientBegin"] : base.ButtonPressedGradientBegin; } }
		public override Color ButtonPressedGradientMiddle { get { return _customColors.ContainsKey("ButtonPressedGradientMiddle") ? _customColors["ButtonPressedGradientMiddle"] : base.ButtonPressedGradientMiddle; } }
		public override Color ButtonPressedGradientEnd { get { return _customColors.ContainsKey("ButtonPressedGradientEnd") ? _customColors["ButtonPressedGradientEnd"] : base.ButtonPressedGradientEnd; } }
		public override Color CheckBackground { get { return _customColors.ContainsKey("CheckBackground") ? _customColors["CheckBackground"] : base.CheckBackground; } }
		public override Color CheckSelectedBackground { get { return _customColors.ContainsKey("CheckSelectedBackground") ? _customColors["CheckSelectedBackground"] : base.CheckSelectedBackground; } }
		public override Color CheckPressedBackground { get { return _customColors.ContainsKey("CheckPressedBackground") ? _customColors["CheckPressedBackground"] : base.CheckPressedBackground; } }
		public override Color GripDark { get { return _customColors.ContainsKey("GripDark") ? _customColors["GripDark"] : base.GripDark; } }
		public override Color GripLight { get { return _customColors.ContainsKey("GripLight") ? _customColors["GripLight"] : base.GripLight; } }
		public override Color ImageMarginGradientBegin { get { return _customColors.ContainsKey("ImageMarginGradientBegin") ? _customColors["ImageMarginGradientBegin"] : base.ImageMarginGradientBegin; } }
		public override Color ImageMarginGradientMiddle { get { return _customColors.ContainsKey("ImageMarginGradientMiddle") ? _customColors["ImageMarginGradientMiddle"] : base.ImageMarginGradientMiddle; } }
		public override Color ImageMarginGradientEnd { get { return _customColors.ContainsKey("ImageMarginGradientEnd") ? _customColors["ImageMarginGradientEnd"] : base.ImageMarginGradientEnd; } }
		public override Color ImageMarginRevealedGradientBegin { get { return _customColors.ContainsKey("ImageMarginRevealedGradientBegin") ? _customColors["ImageMarginRevealedGradientBegin"] : base.ImageMarginRevealedGradientBegin; } }
		public override Color ImageMarginRevealedGradientMiddle { get { return _customColors.ContainsKey("ImageMarginRevealedGradientMiddle") ? _customColors["ImageMarginRevealedGradientMiddle"] : base.ImageMarginRevealedGradientMiddle; } }
		public override Color ImageMarginRevealedGradientEnd { get { return _customColors.ContainsKey("ImageMarginRevealedGradientEnd") ? _customColors["ImageMarginRevealedGradientEnd"] : base.ImageMarginRevealedGradientEnd; } }
		public override Color MenuStripGradientBegin { get { return _customColors.ContainsKey("MenuStripGradientBegin") ? _customColors["MenuStripGradientBegin"] : base.MenuStripGradientBegin; } }
		public override Color MenuStripGradientEnd { get { return _customColors.ContainsKey("MenuStripGradientEnd") ? _customColors["MenuStripGradientEnd"] : base.MenuStripGradientEnd; } }
		public override Color MenuItemSelected { get { return _customColors.ContainsKey("MenuItemSelected") ? _customColors["MenuItemSelected"] : base.MenuItemSelected; } }
		public override Color MenuItemBorder { get { return _customColors.ContainsKey("MenuItemBorder") ? _customColors["MenuItemBorder"] : base.MenuItemBorder; } }
		public override Color MenuBorder { get { return _customColors.ContainsKey("MenuBorder") ? _customColors["MenuBorder"] : base.MenuBorder; } }
		public override Color MenuItemSelectedGradientBegin { get { return _customColors.ContainsKey("MenuItemSelectedGradientBegin") ? _customColors["MenuItemSelectedGradientBegin"] : base.MenuItemSelectedGradientBegin; } }
		public override Color MenuItemSelectedGradientEnd { get { return _customColors.ContainsKey("MenuItemSelectedGradientEnd") ? _customColors["MenuItemSelectedGradientEnd"] : base.MenuItemSelectedGradientEnd; } }
		public override Color MenuItemPressedGradientBegin { get { return _customColors.ContainsKey("MenuItemPressedGradientBegin") ? _customColors["MenuItemPressedGradientBegin"] : base.MenuItemPressedGradientBegin; } }
		public override Color MenuItemPressedGradientMiddle { get { return _customColors.ContainsKey("MenuItemPressedGradientMiddle") ? _customColors["MenuItemPressedGradientMiddle"] : base.MenuItemPressedGradientMiddle; } }
		public override Color MenuItemPressedGradientEnd { get { return _customColors.ContainsKey("MenuItemPressedGradientEnd") ? _customColors["MenuItemPressedGradientEnd"] : base.MenuItemPressedGradientEnd; } }
		public override Color RaftingContainerGradientBegin { get { return _customColors.ContainsKey("RaftingContainerGradientBegin") ? _customColors["RaftingContainerGradientBegin"] : base.RaftingContainerGradientBegin; } }
		public override Color RaftingContainerGradientEnd { get { return _customColors.ContainsKey("RaftingContainerGradientEnd") ? _customColors["RaftingContainerGradientEnd"] : base.RaftingContainerGradientEnd; } }
		public override Color SeparatorDark { get { return _customColors.ContainsKey("SeparatorDark") ? _customColors["SeparatorDark"] : base.SeparatorDark; } }
		public override Color SeparatorLight { get { return _customColors.ContainsKey("SeparatorLight") ? _customColors["SeparatorLight"] : base.SeparatorLight; } }
		public override Color StatusStripGradientBegin { get { return _customColors.ContainsKey("StatusStripGradientBegin") ? _customColors["StatusStripGradientBegin"] : base.StatusStripGradientBegin; } }
		public override Color StatusStripGradientEnd { get { return _customColors.ContainsKey("StatusStripGradientEnd") ? _customColors["StatusStripGradientEnd"] : base.StatusStripGradientEnd; } }
		public override Color ToolStripBorder { get { return _customColors.ContainsKey("ToolStripBorder") ? _customColors["ToolStripBorder"] : base.ToolStripBorder; } }
		public override Color ToolStripDropDownBackground { get { return _customColors.ContainsKey("ToolStripDropDownBackground") ? _customColors["ToolStripDropDownBackground"] : base.ToolStripDropDownBackground; } }
		public override Color ToolStripGradientBegin { get { return _customColors.ContainsKey("ToolStripGradientBegin") ? _customColors["ToolStripGradientBegin"] : base.ToolStripGradientBegin; } }
		public override Color ToolStripGradientMiddle { get { return _customColors.ContainsKey("ToolStripGradientMiddle") ? _customColors["ToolStripGradientMiddle"] : base.ToolStripGradientMiddle; } }
		public override Color ToolStripGradientEnd { get { return _customColors.ContainsKey("ToolStripGradientEnd") ? _customColors["ToolStripGradientEnd"] : base.ToolStripGradientEnd; } }
		public override Color ToolStripContentPanelGradientBegin { get { return _customColors.ContainsKey("ToolStripContentPanelGradientBegin") ? _customColors["ToolStripContentPanelGradientBegin"] : base.ToolStripContentPanelGradientBegin; } }
		public override Color ToolStripContentPanelGradientEnd { get { return _customColors.ContainsKey("ToolStripContentPanelGradientEnd") ? _customColors["ToolStripContentPanelGradientEnd"] : base.ToolStripContentPanelGradientEnd; } }
		public override Color ToolStripPanelGradientBegin { get { return _customColors.ContainsKey("ToolStripPanelGradientBegin") ? _customColors["ToolStripPanelGradientBegin"] : base.ToolStripPanelGradientBegin; } }
		public override Color ToolStripPanelGradientEnd { get { return _customColors.ContainsKey("ToolStripPanelGradientEnd") ? _customColors["ToolStripPanelGradientEnd"] : base.ToolStripPanelGradientEnd; } }
		public override Color OverflowButtonGradientBegin { get { return _customColors.ContainsKey("OverflowButtonGradientBegin") ? _customColors["OverflowButtonGradientBegin"] : base.OverflowButtonGradientBegin; } }
		public override Color OverflowButtonGradientMiddle { get { return _customColors.ContainsKey("OverflowButtonGradientMiddle") ? _customColors["OverflowButtonGradientMiddle"] : base.OverflowButtonGradientMiddle; } }
		public override Color OverflowButtonGradientEnd { get { return _customColors.ContainsKey("OverflowButtonGradientEnd") ? _customColors["OverflowButtonGradientEnd"] : base.OverflowButtonGradientEnd; } }

		public CustomColorTable(Dictionary<string, Color> customColors)
		{
			foreach (var customColor in customColors)
			{
				if (customColor.Value != Color.Empty)
					_customColors.Add(customColor.Key, customColor.Value);
			}
		}

	}
}
