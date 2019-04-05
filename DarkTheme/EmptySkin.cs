using System.Drawing;
using System.Windows.Forms;
using DarkTheme.Custom;

namespace DarkTheme
{
	internal class EmptySkin : ISkin
	{
		public string Name { get; protected set; }

		public TreeViewDrawMode TreeViewDrawMode { get; protected set; }
		public Image ListViewBackground { get; protected set; }
		public bool ListViewBackgroundTiled { get; protected set; }
		public ToolStripRenderer ToolStripRenderer { get; protected set; }

		public OtherLook Other { get; private set; }
		public ControlLook Control { get; private set; }
		public ControlLook Form { get; private set; }
		public ButtonLook Button { get; private set; }
		public TreeViewLook TreeView { get; private set; }
		public RichTextBoxLook RichTextBox { get; private set; }
		public LinkLabelLook LinkLabel { get; private set; }
		public ListViewLook ListView { get; private set; }
		public ControlLook SecureTextBox { get; private set; }

		public EmptySkin()
		{
			Name = "Dark theme";
			Other = new OtherLook();
			Control = new ControlLook();
			Form = new ControlLook();
			Button = new ButtonLook();
			TreeView = new TreeViewLook();
			RichTextBox = new RichTextBoxLook();
			LinkLabel = new LinkLabelLook();
			ListView = new ListViewLook();
			SecureTextBox = new ControlLook();
		}
	}
}