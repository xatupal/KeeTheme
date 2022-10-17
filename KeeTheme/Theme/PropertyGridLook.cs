using System.ComponentModel;
using System.Drawing;
using KeeTheme.Editor;

namespace KeeTheme.Theme
{
	[TypeConverter(typeof(PropertyGridLookTypeConverter))]
	class PropertyGridLook : ControlLook
	{
		public Color LineColor { get; set; }
		public Color CategoryForeColor{ get; set; }
	}
}