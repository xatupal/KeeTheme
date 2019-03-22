using System.Drawing;
using System.Windows.Forms;

namespace DarkTheme
{
	internal class DefaultStyle : IControlStyle
	{
		public Color BackColor
		{
			get { return Color.Empty; }
		}
		public Color ForeColor
		{
			get { return Color.Empty; }
		}
		public Color ButtonBorderColor
		{
			get { return Color.Empty; }
		}
		public FlatStyle ButtonFlatStyle
		{
			get { return FlatStyle.Standard; }
		}
		public Color LinkColor
		{
			get { return Color.Empty; }
		}

		public BorderStyle BorderStyle
		{
			get { return BorderStyle.Fixed3D; }
		}
		public Color TreeViewBackColor
		{
			get { return Color.Empty; }
		}

		public TreeViewDrawMode TreeViewDrawMode
		{
			get { return TreeViewDrawMode.Normal; }
		}

		public Color ListViewBackColor
		{
			get { return Color.Empty; }
		}

		public Color NormalField
		{
			get { return SystemColors.Window; }
		}

		public Color DisabledField
		{
			get { return SystemColors.Control; }
		}

		public Image ListViewBackground
		{
			get { return null; }
		}

		public bool ListViewBackgroundTiled
		{
			get { return false; }
		}
	}
}