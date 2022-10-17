using System.ComponentModel;
using System.Drawing;
using KeeTheme.Editor;

namespace KeeTheme.Theme
{
	[TypeConverter(typeof(LinkLabelLookTypeConverter))]
	class LinkLabelLook : ControlLook
	{
		public Color LinkColor { get; set; }
	}
}