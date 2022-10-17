using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using KeeTheme.Editor;

namespace KeeTheme.Theme
{
	[TypeConverter(typeof(RichTextBoxLookTypeConverter))]
	class RichTextBoxLook : ControlLook
	{
		public BorderStyle BorderStyle { get; set; }
		public Color SelectionColor { get; set; }
	}
}