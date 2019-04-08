using System.Drawing;
using System.Windows.Forms;

namespace DarkTheme.Custom
{
	internal class DefaultSkin : EmptySkin
	{
		public DefaultSkin()
		{
			Other.ControlNormalColor = SystemColors.Window;
			Other.ControlDisabledColor = SystemColors.Control;
			Button.FlatStyle = FlatStyle.Standard;
			TreeView.BorderStyle = BorderStyle.Fixed3D;
			RichTextBox.BorderStyle = BorderStyle.Fixed3D;
			ListView.BorderStyle = BorderStyle.Fixed3D;
			ToolStripRenderer = ToolStripManager.Renderer;
		}
	}
}