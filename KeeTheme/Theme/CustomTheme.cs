using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using KeePass.App;

namespace KeeTheme.Theme
{
	internal class CustomTheme : ITheme
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
		public CheckBoxLook CheckBox { get; private set; }
		public CheckBoxButtonLook CheckBoxButton { get; private set; }
		public MenuLook MenuItem { get; private set; }
		public PropertyGridLook PropertyGrid { get; private set; }

		public CustomTheme()
		{
			Name = "Dark Theme";
			Other = new OtherLook();
			Control = new ControlLook();
			Form = new ControlLook();
			Button = new ButtonLook();
			TreeView = new TreeViewLook();
			RichTextBox = new RichTextBoxLook();
			LinkLabel = new LinkLabelLook();
			ListView = new ListViewLook();
			SecureTextBox = new ControlLook();
			CheckBox = new CheckBoxLook();
			CheckBoxButton = new CheckBoxButtonLook();
			MenuItem = new MenuLook();
			PropertyGrid = new PropertyGridLook();
		}
		
		public CustomTheme(CustomThemeTemplate themeTemplate) : this()
		{
			Name = themeTemplate.Name;
			Other = themeTemplate.Other;
			Control = themeTemplate.Control;
			Form = themeTemplate.Form;
			Button = themeTemplate.Button;
			TreeView = themeTemplate.TreeView;
			RichTextBox = themeTemplate.RichTextBox;
			LinkLabel = themeTemplate.LinkLabel;
			ListView = themeTemplate.ListView;
			SecureTextBox = themeTemplate.SecureTextBox;
			CheckBox = themeTemplate.CheckBox;
			CheckBoxButton = themeTemplate.CheckBoxButton;
			MenuItem = themeTemplate.MenuItem;
			PropertyGrid = themeTemplate.PropertyGrid;

			ToolStripRenderer = GetToolStripRenderer(themeTemplate.ToolStrip);
			TreeViewDrawMode = TreeViewDrawMode.OwnerDrawText;

			if (!string.IsNullOrEmpty(ListView.BackgroundImage))
			{
				var imagePath = Path.Combine(TemplateReader.GetTemplatesDir(), ListView.BackgroundImage);

				if (File.Exists(imagePath))
					ListViewBackground = Image.FromFile(imagePath);
			}

			if (ListView.BackColor != Color.Empty && ListViewBackground == null)
			{
				ListViewBackgroundTiled = true;

				var bitmap = new Bitmap(1, 1);
				bitmap.SetPixel(0, 0, ListView.BackColor);
				ListViewBackground = bitmap;
			}
		}

		private ToolStripRenderer GetToolStripRenderer(ToolStripLook toolStripLook)
		{
			var toolStripLookProperties = typeof(ToolStripLook).GetProperties();
			var customColors = new Dictionary<string, Color>();
			foreach (var toolStripLookProperty in toolStripLookProperties)
			{
				customColors.Add(toolStripLookProperty.Name, (Color) toolStripLookProperty.GetValue(toolStripLook, null));
			}
			var colorTable = new CustomColorTable(customColors);
			return new CustomToolStripRenderer(this, colorTable);
		}

		public static CustomTheme GetDefaultTheme()
		{
			var emptyTheme = new CustomTheme();
			emptyTheme.Other.ControlNormalColor = SystemColors.Window;
			emptyTheme.Other.ControlDisabledColor = SystemColors.Control;
			emptyTheme.Other.ColorEditError = AppDefs.ColorEditError;
			emptyTheme.Button.FlatStyle = FlatStyle.Standard;
			emptyTheme.TreeView.BorderStyle = BorderStyle.Fixed3D;
			emptyTheme.RichTextBox.BorderStyle = BorderStyle.Fixed3D;
			emptyTheme.ListView.BorderStyle = BorderStyle.Fixed3D;
			emptyTheme.CheckBox.FlatStyle = FlatStyle.Standard;
			emptyTheme.CheckBoxButton.FlatStyle = FlatStyle.Standard;
			emptyTheme.ToolStripRenderer = ToolStripManager.Renderer;
			return emptyTheme;
		}
	}
}