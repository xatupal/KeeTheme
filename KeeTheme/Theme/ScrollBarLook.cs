using System.ComponentModel;
using KeeTheme.Editor;

namespace KeeTheme.Theme
{
	[TypeConverter(typeof(ScrollBarLookTypeConverter))]
	class ScrollBarLook
	{
		public bool UseExplorerDarkMode { get; set; }
		
		public override string ToString()
		{
			return GetType().Name;
		}
	}
}