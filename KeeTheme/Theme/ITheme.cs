using System.Drawing;
using System.Windows.Forms;

namespace KeeTheme.Theme
{
	internal interface ITheme
	{
		string Name { get; }

		TreeViewDrawMode TreeViewDrawMode { get; }
		Image ListViewBackground { get; }
		bool ListViewBackgroundTiled { get; }
		ToolStripRenderer ToolStripRenderer { get; }

		OtherLook Other { get; }
		ControlLook Control { get; }
		ControlLook Form { get; }
		ButtonLook Button { get; }
		TreeViewLook TreeView { get; }
		RichTextBoxLook RichTextBox { get; }
		LinkLabelLook LinkLabel { get; }
		ListViewLook ListView { get; }
		TabControlLook TabControl { get; }
		ControlLook SecureTextBox { get; }
		CheckBoxLook CheckBox { get; }
		CheckBoxButtonLook CheckBoxButton { get; }
		MenuLook MenuItem { get; }
		PropertyGridLook PropertyGrid { get; }
	}
}