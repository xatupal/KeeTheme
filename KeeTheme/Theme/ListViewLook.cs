using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using KeeTheme.Editor;

namespace KeeTheme.Theme
{
	[TypeConverter(typeof(ListViewLookTypeConverter))]
	class ListViewLook : ControlLook
	{
		public BorderStyle BorderStyle { get; set; }
		public Color OddRowColor { get; set; }
		public Color EvenRowColor { get; set; }
		public Color ColumnBorderColor { get; set; }
		public Color HeaderBackColor { get; set; }
		public Color HeaderForeColor { get; set; }
		public Color HeaderColumnBorderColor { get; set; }
		public Color GroupBackColor { get; set; }
		public Color GroupForeColor { get; set; }
		public Color GroupHighlightColor { get; set; }
		[DefaultValue(ContentAlignment.TopLeft)]
		public ContentAlignment BackgroundImageAlignment { get; set; }
		[DefaultValue("")]
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		public string BackgroundImage { get; set; }
	}
}