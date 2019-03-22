using System.Drawing;
using System.Windows.Forms;

namespace DarkTheme
{
	internal class DarkStyle : IControlStyle
	{
		private static readonly Image ListViewBackgroundImage;

		static DarkStyle()
		{
			var bitmap = new Bitmap(1, 1);
			bitmap.SetPixel(0, 0, Colors.Window);
			ListViewBackgroundImage = bitmap;
		}

		public Color BackColor
		{
			get { return Colors.Control; }
		}

		public Color ForeColor
		{
			get { return Colors.ControlText; }
		}

		public Color ButtonBorderColor
		{
			get { return Colors.ButtonBorderColor; }
		}

		public FlatStyle ButtonFlatStyle
		{
			get { return FlatStyle.Flat; }
		}

		public Color LinkColor
		{
			get { return Colors.Link; }
		}

		public BorderStyle BorderStyle
		{
			get { return BorderStyle.FixedSingle; }
		}
		public Color TreeViewBackColor
		{
			get { return Colors.Window; }
		}
		public TreeViewDrawMode TreeViewDrawMode
		{
			get { return TreeViewDrawMode.OwnerDrawText; }
		}

		public Color ListViewBackColor
		{
			get { return Colors.Window; }
		}

		public Color NormalField
		{
			get { return Colors.Window; }
		}
		public Color DisabledField
		{
			get { return Colors.Control; }
		}

		public Image ListViewBackground
		{
			get { return ListViewBackgroundImage; }
		}

		public bool ListViewBackgroundTiled
		{
			get { return true; }
		}
	}
}