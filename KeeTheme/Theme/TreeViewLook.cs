using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using KeeTheme.Editor;

namespace KeeTheme.Theme
{
	[TypeConverter(typeof(TreeViewLookTypeConverter))]
	class TreeViewLook : ControlLook
	{
		public Color SelectionColor { get; set; }
		public Color SelectionBackColor { get; set; }
		public BorderStyle BorderStyle { get; set; }
	}
}