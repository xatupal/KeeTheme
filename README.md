# KeeTheme

[![Version](https://img.shields.io/github/release/xatupal/KeeTheme)](https://github.com/xatupal/KeeTheme/releases/latest)
[![Releasedate](https://img.shields.io/github/release-date/xatupal/KeeTheme)](https://github.com/xatupal/KeeTheme/releases/latest)
[![Downloads](https://img.shields.io/github/downloads/xatupal/KeeTheme/total)](https://github.com/xatupal/KeeTheme/releases/latest/download/KeeTheme.plgx)

KeePass Plugin

Plugin changes the appearance of KeePass, to make it look better at night. 

You can enable it using hotkey `CTRL+T` or using the menu `Tools -> DarkTheme`.

### Options

In [options](docs/KeePassDarkThemeCustomOptions.png) `Tools -> Options... -> KeeTheme` you can:
* Select a theme
* Create your own theme
* Change the default hotkey
* Auto-sync with Windows 10 theme

### Customizations

You can use built-in theme editor to create your own theme.
Custom themes should be saved in the plugins folder.

![Theme editor](docs/KeePassDarkThemeEditor.png)


### Installation

Copy [KeeTheme.dll](https://github.com/xatupal/KeeTheme/releases/latest/download/KeeTheme.dll) or [KeeTheme.plgx](https://github.com/xatupal/KeeTheme/releases/latest/download/KeeTheme.plgx) to the KeePass Plugins directory or install via [Chocolatey](https://chocolatey.org):

```
choco install keepass-plugin-keetheme
```

### Note

KeePass was created using standard Windows controls, which unfortunately were not designed for easy customization. They are extremely resistant to any attempts to change their appearance, especially from a plugin which has no control over their creation.

Therefore the plugin is not perfect and will never be, but is good enough to use it.

### Screenshots
#### DarkTheme

![Main form](docs/KeePassDarkTheme.png)

![Open database](docs/KeePassDarkThemeOpenDatabase.png)

![Options](docs/KeePassDarkThemeOptions.png)

#### DarkThemeWin11

![Main form](docs/KeePassDarkThemeWin11.png)

![Open database](docs/KeePassDarkThemeWin11OpenDatabase.png)

![Options](docs/KeePassDarkThemeWin11Options.png)
