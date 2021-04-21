using System.Drawing;
using System.Windows.Forms;
using KeePass.App;

namespace KeeTheme.Theme
{
	internal class DefaultTheme : EmptyTheme
	{
		public DefaultTheme()
		{
			Other.ControlNormalColor = SystemColors.Window;
			Other.ControlDisabledColor = SystemColors.Control;
			Other.ColorEditError = AppDefs.ColorEditError;
			Button.FlatStyle = FlatStyle.Standard;
			TreeView.BorderStyle = BorderStyle.Fixed3D;
			RichTextBox.BorderStyle = BorderStyle.Fixed3D;
			ListView.BorderStyle = BorderStyle.Fixed3D;
			CheckBox.FlatStyle = FlatStyle.Standard;
			CheckBoxButton.FlatStyle = FlatStyle.Standard;
			ToolStripRenderer = ToolStripManager.Renderer;
		}
	}
}