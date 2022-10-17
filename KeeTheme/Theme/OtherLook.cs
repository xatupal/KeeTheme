using System.ComponentModel;
using System.Drawing;
using KeeTheme.Editor;

namespace KeeTheme.Theme
{
	[TypeConverter(typeof(OtherLookTypeConverter))]
	class OtherLook
	{
		public Color ControlNormalColor { get; set; }
		public Color ControlDisabledColor { get; set; }
		public Color ColorEditError { get; set; }
		
		public override string ToString()
		{
			return GetType().Name;
		}
	}
}