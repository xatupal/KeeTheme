using System.ComponentModel;
using System.Drawing;
using KeeTheme.Editor;

namespace KeeTheme.Theme
{
	[TypeConverter(typeof(ControlLookTypeConverter))]
	class ControlLook
	{
		public Color BackColor { get; set; }
		public Color ForeColor { get; set; }

		public override string ToString()
		{
			return GetType().Name;
		}
	}
}
