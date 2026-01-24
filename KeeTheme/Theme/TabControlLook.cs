using System.ComponentModel;
using System.Drawing;
using KeeTheme.Editor;

namespace KeeTheme.Theme
{
	[TypeConverter(typeof(TabControlTypeConverter))]
	class TabControlLook : ControlLook
	{
		public Color SelectedTabColor { get; set; }
		public Color SelectedTabBorderColor { get; set; }
		public Color UnselectedTabColor { get; set; }
		public Color UnselectedTabBorderColor { get; set; }
	}
}