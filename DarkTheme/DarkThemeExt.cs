using System;
using System.Drawing;
using System.Windows.Forms;
using DarkTheme.Properties;
using KeePass.Plugins;
using KeePass.UI;
using KeePassLib.Utility;

namespace DarkTheme
{
	public sealed class DarkThemeExt : Plugin
	{
		private const string DarkModeOnConfigItem = "DarkTheme.Enabled";

		private ControlVisitor _controlVisitor;
		private DarkTheme _theme;
		private IPluginHost _host;
		private ToolStripMenuItem _menuItem;

		public override bool Initialize(IPluginHost host)
		{
			if (host == null)
				return false;

			_host = host;

			_controlVisitor = new ControlVisitor(HandleControlVisit);
			var themeEnabled = host.CustomConfig.GetBool(DarkModeOnConfigItem, false);
			
			_theme = new DarkTheme(themeEnabled);
			if (_theme.Enabled)
				ApplyThemeInOpenForms();

			GlobalWindowManager.WindowAdded += HandleGlobalWindowManagerWindowAdded;

			return true;
		}

		public override ToolStripMenuItem GetMenuItem(PluginMenuType t)
		{
			if (t == PluginMenuType.Main)
			{
				_menuItem = new ToolStripMenuItem(_theme.Name);
				_menuItem.CheckOnClick = true;
				_menuItem.Checked = _theme.Enabled;
				_menuItem.ShortcutKeys = Keys.Control | Keys.T;
				_menuItem.Click += HandleToggleDarkModeMenuItemClick;
				return _menuItem;
			}

			return base.GetMenuItem(t);
		}

		private void HandleToggleDarkModeMenuItemClick(object sender, EventArgs eventArgs)
		{
			_theme.Enabled = !_theme.Enabled;
			_host.CustomConfig.SetBool(DarkModeOnConfigItem, _theme.Enabled);
			_menuItem.Text = _theme.Name;

			ApplyThemeInOpenForms();
		}

		private void ApplyThemeInOpenForms()
		{
			foreach (Form openForm in Application.OpenForms)
			{
				_controlVisitor.Visit(openForm);
			}
		}

		public override void Terminate()
		{
			GlobalWindowManager.WindowAdded -= HandleGlobalWindowManagerWindowAdded;
		}

		private void HandleControlVisit(Control control)
		{
			_theme.Apply(control);
		}

		private void HandleGlobalWindowManagerWindowAdded(object sender, GwmWindowEventArgs e)
		{
			if (_theme.Enabled)
				_controlVisitor.Visit(e.Form);
		}

		public override string UpdateUrl
		{
			get { return "https://nibiru.pl/keepass/plugins.php?name=DarkTheme"; }
		}

		public override Image SmallIcon
		{
			get { return GfxUtil.ScaleImage(Resource.PluginIcon, 16, 16); }
		}
	}
}