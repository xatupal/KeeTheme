using System.Drawing;
using System.Windows.Forms;

namespace DarkTheme
{
	internal interface IControlStyle
	{
		Color BackColor { get; }
		Color ForeColor { get; }
		Color ButtonBorderColor { get; }
		FlatStyle ButtonFlatStyle { get; }
		Color LinkColor { get; }
		BorderStyle BorderStyle { get; }
		Color TreeViewBackColor { get; }
		TreeViewDrawMode TreeViewDrawMode { get; }
		Color ListViewBackColor { get; }
		Color NormalField { get; }
		Color DisabledField { get; }
		Image ListViewBackground { get; }
		bool ListViewBackgroundTiled { get; }
	}
}