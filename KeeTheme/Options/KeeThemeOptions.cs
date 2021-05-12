using System;
using System.Windows.Forms;
using KeePass.Plugins;

namespace KeeTheme.Options
{
	public class KeeThemeOptions
	{
		private const string EnabledOption = "KeeTheme.Enabled";
		private const string HotKeyOption = "KeeTheme.HotKey";
		private const string AutoSyncWithWin10ThemeOption = "KeeTheme.AutoSyncWithWin10Theme";
		
		private readonly IPluginHost _pluginHost;
		private bool _enabled;
		private Keys _hotKey;
		private bool _autoSyncWithWin10Theme;

		public bool Enabled
		{
			get { return _enabled; }
			set
			{
				_enabled = value;
				_pluginHost.CustomConfig.SetBool(EnabledOption, value);
				if (EnabledChanged != null)
					EnabledChanged.Invoke(value);
			}
		}

		public Keys HotKey
		{
			get { return _hotKey; }
			set
			{
				_hotKey = value;
				_pluginHost.CustomConfig.SetString(HotKeyOption, value.ToString());
				if (HotKeyChanged != null)
					HotKeyChanged.Invoke(value);
			}
		}

		public bool AutoSyncWithWin10Theme
		{
			get { return _autoSyncWithWin10Theme; }
			set
			{
				_autoSyncWithWin10Theme = value;
				_pluginHost.CustomConfig.SetBool(AutoSyncWithWin10ThemeOption, value);
				if (AutoSyncWithWin10ThemeChanged != null)
					AutoSyncWithWin10ThemeChanged.Invoke(value);
			}
		}

		public event Action<bool> EnabledChanged;
		public event Action<Keys> HotKeyChanged;
		public event Action<bool> AutoSyncWithWin10ThemeChanged;

		public KeeThemeOptions(IPluginHost pluginHost)
		{
			_pluginHost = pluginHost;
			
			_enabled = pluginHost.CustomConfig.GetBool(EnabledOption, false);
			var hotKey = pluginHost.CustomConfig.GetString(HotKeyOption, "T, Control");
			_hotKey = (Keys) Enum.Parse(typeof(Keys), hotKey);
			_autoSyncWithWin10Theme = pluginHost.CustomConfig.GetBool(AutoSyncWithWin10ThemeOption, false);
		}
	}
}