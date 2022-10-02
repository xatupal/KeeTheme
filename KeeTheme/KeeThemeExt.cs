using System;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using KeePass;
using KeePass.Forms;
using KeePass.Plugins;
using KeePassLib;
using KeePassLib.Utility;
using KeeTheme.Decorators;
using KeeTheme.Options;
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

		private ControlVisitor _controlVisitor;
		private KeeTheme _theme;
		private IPluginHost _host;
		private ToolStripMenuItem _menuItem;
		private KeeThemeOptions _options;
		private OptionsPanel _optionsPanel;
		private Win10ThemeMonitor _win10ThemeMonitor;

		public override bool Initialize(IPluginHost host)
		{
			if (host == null)
				return false;

			_host = host;

			_options = new KeeThemeOptions(host);
			_controlVisitor = new ControlVisitor(HandleControlVisit);
			_theme = new KeeTheme(_options);

			_win10ThemeMonitor = new Win10ThemeMonitor(_options);
			_win10ThemeMonitor.Initialize();

			if (Program.TriggerSystem.Enabled)
			{
				// It's better to enable theme as late as possible, but not too late
				Program.TriggerSystem.RaisingEvent += HandleTriggerSystemRaisingEvent;
			}
			else
			{
				InitializeTheme();
			}
			
			AttachApplicationOpenFormsAddedHandler();

			return true;
		}

		private void AttachApplicationOpenFormsAddedHandler()
		{
			var customArrayList = new FormsArrayList();
			customArrayList.AddRange(Application.OpenForms);
			customArrayList.Added += HandleOpenFormsAdded;
			
			var listField = typeof(ReadOnlyCollectionBase).GetField("list", BindingFlags.Instance | BindingFlags.NonPublic);
			if (listField != null)
				listField.SetValue(Application.OpenForms, customArrayList);
		}

		private void HandleOpenFormsAdded(object sender, FormAddedEventArgs args)
		{
			if (_theme.Enabled)
				_controlVisitor.Visit(args.Form);
			
			Win10ThemeMonitor.UseImmersiveDarkMode(args.Form, _theme.Enabled);
			
			var optionsForm = args.Form as OptionsForm;
			if (optionsForm != null)
			{
				optionsForm.Shown += HandleOptionsFormShown;
			}

			var editStringForm = args.Form as EditStringForm;
			if (editStringForm != null)
			{
				editStringForm.Load += HandleEditStringFormLoad;
			}			
			
			var pwEntryForm = args.Form as PwEntryForm;
			if (pwEntryForm != null)
			{
				pwEntryForm.Load += HandlePwEntryFormFormLoad;
			}
		}

		private void HandlePwEntryFormFormLoad(object sender, EventArgs e)
		{
			PwGeneratorMenuDecorator.TryFindAndDecorate(sender, _theme);
		}

		private void HandleEditStringFormLoad(object sender, EventArgs e)
		{
			PwGeneratorMenuDecorator.TryFindAndDecorate(sender, _theme);
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
			_theme.Enabled = _options.Enabled;
			if (_theme.Enabled)
				ApplyThemeInOpenForms();

			_options.EnabledChanged += enable =>
			{
				_theme.Enabled = enable;
				ApplyThemeInOpenForms();
			};
			
			_options.TemplateChanged += template =>
			{
				if (_theme.Enabled)
				{
					_theme.Enabled = false;
					_theme.Enabled = true;

					var iniFile = TemplateReader.Get(template);
					var themeSection = iniFile.GetSection("KeeTheme");
					_menuItem.Text = themeSection["Name"];

					ApplyThemeInOpenForms();
				}
			}; 
		}

		public override ToolStripMenuItem GetMenuItem(PluginMenuType t)
		{
			if (t == PluginMenuType.Main)
			{
				_menuItem = new ToolStripMenuItem(_theme.Name);
				_menuItem.CheckOnClick = true;
				_menuItem.Checked = _options.Enabled;
				_menuItem.ShortcutKeys = _options.HotKey;
				_menuItem.Click += HandleToggleKeeThemeMenuItemClick;
				_options.HotKeyChanged += keys => _menuItem.ShortcutKeys = keys;
				return _menuItem;
			}

			return base.GetMenuItem(t);
		}

		private void HandleToggleKeeThemeMenuItemClick(object sender, EventArgs eventArgs)
		{
			_options.Enabled = !_options.Enabled;
		}

		private void ApplyThemeInOpenForms()
		{
			foreach (Form openForm in Application.OpenForms)
			{
				Win10ThemeMonitor.UseImmersiveDarkMode(openForm, _theme.Enabled);
				_controlVisitor.Visit(openForm);
			}
			Program.MainForm.RefreshEntriesList();
		}

		private void HandleControlVisit(Control control)
		{
			_theme.Apply(control);
		}

		private void HandleOptionsFormShown(object sender, EventArgs e)
		{
			var optionsForm = (OptionsForm) sender;
			OptionsPanel.Create(optionsForm, _options);
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