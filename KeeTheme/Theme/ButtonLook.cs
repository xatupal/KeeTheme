using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using KeeTheme.Editor;

namespace KeeTheme.Theme
{
	[TypeConverter(typeof(ButtonLookTypeConverter))]
	class ButtonLook : ControlLook
	{
		public Color BorderColor { get; set; }
		public FlatStyle FlatStyle { get; set; }
	}
}