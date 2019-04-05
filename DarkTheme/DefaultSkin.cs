using System.Drawing;
using System.Windows.Forms;
using DarkTheme.Custom;

namespace DarkTheme
{
	internal class DefaultSkin : ISkin
	{
		public string Name { get; protected set; } = "Dark theme";

		public TreeViewDrawMode TreeViewDrawMode { get; protected set; }
		public Image ListViewBackground { get; protected set; }
		public bool ListViewBackgroundTiled { get; protected set; }
		public ToolStripRenderer ToolStripRenderer { get; protected set; }

		public OtherLook Other { get; } = new OtherLook();
		public ControlLook Control { get; } = new ControlLook();
		public ControlLook Form { get; } = new ControlLook();
		public ButtonLook Button { get; } = new ButtonLook();
		public TreeViewLook TreeView { get; } = new TreeViewLook();
		public RichTextBoxLook RichTextBox { get; } = new RichTextBoxLook();
		public LinkLabelLook LinkLabel { get; } = new LinkLabelLook();
		public ListViewLook ListView { get; } = new ListViewLook();
		public ControlLook SecureTextBox { get; } = new ControlLook();

		public DefaultSkin()
		{
			Other.ControlNormalColor = SystemColors.Window;
			Other.ControlDisabledColor = SystemColors.Control;
			Button.FlatStyle = FlatStyle.Standard;
			TreeView.BorderStyle = BorderStyle.Fixed3D;
			RichTextBox.BorderStyle = BorderStyle.Fixed3D;
			ListView.BorderStyle = BorderStyle.Fixed3D;
			ToolStripRenderer = ToolStripManager.Renderer;
		}
	}
}