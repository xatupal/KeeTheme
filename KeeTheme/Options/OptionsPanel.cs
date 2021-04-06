using System.Drawing;
using System.Windows.Forms;
using KeePass.Forms;
using KeePass.UI;

namespace KeeTheme.Options
{
	public partial class OptionsPanel : UserControl
	{
		private readonly KeeThemeOptions _options;

		public OptionsPanel()
		{
			InitializeComponent();
		}

		private OptionsPanel(KeeThemeOptions options) : this()
		{
			_options = options;

			LoadOptions();
		}

		public static void Create(OptionsForm optionsForm, KeeThemeOptions options)
		{
			var controls = optionsForm.Controls.Find("m_tabMain", false);
			if (controls.Length != 1)
				return;

			var tabControl = controls[0] as TabControl;
			if (tabControl == null)
				return;

			if (tabControl.ImageList == null)
			{
				tabControl.ImageList = new ImageList();
				tabControl.ImageList.ImageSize = new Size(DpiUtil.ScaleIntX(16), DpiUtil.ScaleIntY(16));
			}
			var imageIndex = tabControl.ImageList.Images.Add(Properties.Resource.PluginIcon, Color.Transparent);
			
			var optionsPanel = new OptionsPanel(options);
			var tabPage = new TabPage("KeeTheme");
			tabPage.ImageIndex = imageIndex;
			tabPage.Controls.Add(optionsPanel);
			tabControl.TabPages.Add(tabPage);
			optionsPanel.Dock = DockStyle.Fill;

			optionsForm.FormClosed += (sender, args) =>
			{
				if (optionsForm.DialogResult == DialogResult.OK)
					optionsPanel.SaveOptions();
			};
		}

		private void LoadOptions()
		{
			hotKeyTextBox.HotKey = _options.HotKey;
		}

		private void SaveOptions()
		{
			_options.HotKey = hotKeyTextBox.HotKey;
		}
	}
}