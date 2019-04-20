using System.Drawing;
using System.Windows.Forms;

namespace KeeTheme.Theme
{
	class ListViewLook : ControlLook
	{
		public BorderStyle BorderStyle { get; set; }
		public Color OddRowColor { get; set; }
		public Color EvenRowColor { get; set; }
		public Color ColumnBorderColor { get; set; }
		public Color HeaderBackColor { get; set; }
		public Color HeaderForeColor { get; set; }
		public Color HeaderColumnBorderColor { get; set; }
		public ContentAlignment BackgroundImageAlignment { get; set; }
		public string BackgroundImage { get; set; }
	}
}