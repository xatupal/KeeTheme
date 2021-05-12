using KeePass.Util;
using KeeTheme.Options;
using Microsoft.Win32;

namespace KeeTheme
{
	internal class Win10ThemeMonitor
	{
		private readonly KeeThemeOptions _options;

		public Win10ThemeMonitor(KeeThemeOptions options)
		{
			_options = options;
		}

		public void Initialize()
		{
			if (WinUtil.IsAtLeastWindows10)
			{
				if (_options.AutoSyncWithWin10Theme)
				{
					_options.Enabled = IsDarkThemeEnabled();
				}
				_options.AutoSyncWithWin10ThemeChanged += HandleAutoSyncWithWin10ThemeChanged;
				SystemEvents.UserPreferenceChanged += HandleUserPreferenceChanged;
			}
		}
		
		private void HandleAutoSyncWithWin10ThemeChanged(bool value)
		{
			ApplyTheme();
		}

		private void HandleUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			ApplyTheme();
		}

		private void ApplyTheme()
		{
			if (_options.AutoSyncWithWin10Theme) 
				_options.Enabled = IsDarkThemeEnabled();
		}

		private bool IsDarkThemeEnabled()
		{
			var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
			return key != null && (int) key.GetValue("AppsUseLightTheme", "1") != 1;
		}

	}
}