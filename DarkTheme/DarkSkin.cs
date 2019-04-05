using System.Drawing;
using System.Windows.Forms;
using KeePass.UI.ToolStripRendering;

namespace DarkTheme
{
	internal class DarkSkin : DefaultSkin
	{
		public DarkSkin()
		{
			ToolStripRenderer = new ProExtTsr(new DarkSkinColorTable());
			TreeViewDrawMode = TreeViewDrawMode.OwnerDrawText;
			ListViewBackgroundTiled = true;

			var bitmap = new Bitmap(1, 1);
			bitmap.SetPixel(0, 0, Colors.Window);
			ListViewBackground = bitmap;

			Other.ControlNormalColor = Colors.Window;
			Other.ControlDisabledColor = Colors.Control;

			Control.BackColor = Colors.Control;
			Control.ForeColor = Colors.ControlText;

			Form.BackColor = Colors.Control;
			Form.ForeColor = Colors.ControlText;

			Button.BackColor = Colors.Control;
			Button.ForeColor = Colors.ControlText;
			Button.BorderColor = Colors.ButtonBorderColor;
			Button.FlatStyle = FlatStyle.Flat;

			TreeView.BackColor = Colors.Window;
			TreeView.ForeColor = Colors.ControlText;
			TreeView.SelectionColor = Colors.WindowText;
			TreeView.BorderStyle = BorderStyle.FixedSingle;

			RichTextBox.BackColor = Colors.Control;
			RichTextBox.ForeColor = Colors.ControlText;
			RichTextBox.BorderStyle = BorderStyle.FixedSingle;
			RichTextBox.SelectionColor = Colors.ControlText;

			LinkLabel.BackColor = Colors.Control;
			LinkLabel.ForeColor = Colors.ControlText;
			LinkLabel.LinkColor = Colors.Link;

			ListView.BackColor = Colors.Window;
			ListView.ForeColor = Colors.ControlText;
			ListView.BorderStyle = BorderStyle.FixedSingle;
			ListView.EvenRowColor = Colors.Window;
			ListView.OddRowColor = Colors.LightWindow;
			ListView.ColumnBorderColor = Colors.ColumnBorder;
			ListView.HeaderBackColor = Colors.HeaderBackground;
			ListView.HeaderForeColor = Colors.WindowText;
			ListView.HeaderColumnBorderColor = Colors.LightBorder;

			SecureTextBox.BackColor = Colors.Control;
			SecureTextBox.ForeColor = Colors.ControlText;
		}
	}
}