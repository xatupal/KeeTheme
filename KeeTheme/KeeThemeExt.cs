using System;
using System.Drawing;
using System.Windows.Forms;
using KeePass;
using KeePass.Plugins;
using KeePass.UI;
using KeePassLib;
using KeePassLib.Utility;
using KeeTheme.Properties;

namespace KeeTheme
{
	public sealed class KeeThemeExt : Plugin
	{
		// Copied from EcasEventIDs.AppInitPost instead of using reflection because probably it will never change
		private static readonly PwUuid AppInitPost = new PwUuid(new byte[] {
			0xD4, 0xCE, 0xCD, 0xB5, 0x4B, 0x98, 0x4F, 0xF2,
			0xA6, 0xA9, 0xE2, 0x55, 0x26, 0x1E, 0xC8, 0xE8
		});

		private const string KeeThemeOnConfigItem = "KeeTheme.Enabled";

		private ControlVisitor _controlVisitor;
		private KeeTheme _theme;
		private IPluginHost _host;
		private ToolStripMenuItem _menuItem;

		public override bool Initialize(IPluginHost host)
		{
			if (host == null)
				return false;

			_host = host;

			_controlVisitor = new ControlVisitor(HandleControlVisit);
			_theme = new KeeTheme();

			if (Program.TriggerSystem.Enabled)
			{
				// It's better to enable theme as late as possible, but not too late
				Program.TriggerSystem.RaisingEvent += HandleTriggerSystemRaisingEvent;
			}
			else
			{
				InitializeTheme();
			}

			GlobalWindowManager.WindowAdded += HandleGlobalWindowManagerWindowAdded;

			return true;
		}

		private void HandleTriggerSystemRaisingEvent(object sender, KeePass.Ecas.EcasRaisingEventArgs e)
		{
			if (e.Event.Type.Equals(AppInitPost))
			{
				InitializeTheme();

				Program.TriggerSystem.RaisingEvent -= HandleTriggerSystemRaisingEvent;
			}
		}

		private void InitializeTheme()
		{
			_theme.Enabled = _host.CustomConfig.GetBool(KeeThemeOnConfigItem, false);
			if (_theme.Enabled)
				ApplyThemeInOpenForms();
		}

		public override ToolStripMenuItem GetMenuItem(PluginMenuType t)
		{
			if (t == PluginMenuType.Main)
			{
				_menuItem = new ToolStripMenuItem(_theme.Name);
				_menuItem.CheckOnClick = true;
				_menuItem.Checked = _host.CustomConfig.GetBool(KeeThemeOnConfigItem, false);
				_menuItem.ShortcutKeys = Keys.Control | Keys.T;
				_menuItem.Click += HandleToggleKeeThemeMenuItemClick;
				return _menuItem;
			}

			return base.GetMenuItem(t);
		}

		private void HandleToggleKeeThemeMenuItemClick(object sender, EventArgs eventArgs)
		{
			_theme.Enabled = !_theme.Enabled;
			_host.CustomConfig.SetBool(KeeThemeOnConfigItem, _theme.Enabled);
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
			get { return "https://nibiru.pl/keepass/plugins.php?name=KeeTheme"; }
		}

		public override Image SmallIcon
		{
			get { return GfxUtil.ScaleImage(Resource.PluginIcon, 16, 16); }
		}
	}
}