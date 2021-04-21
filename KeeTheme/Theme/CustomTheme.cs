using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using KeePass.App;

namespace KeeTheme.Theme
{
	class CustomTheme : DefaultTheme
	{
		public CustomTheme(IniFile iniFile)
		{
			var themeSection = iniFile.GetSection("KeeTheme");
			if (themeSection.ContainsKey("Name"))
				Name = themeSection["Name"];

			var paletteSection = iniFile.GetSection("Palette");
			var palette = new Palette(paletteSection);

			var toolStripSection = iniFile.GetSection("ToolStrip");
			var customColors = toolStripSection.ToDictionary(x => x.Key, x => palette.GetColor(x.Value));
			var colorTable = new CustomColorTable(customColors);
			ToolStripRenderer = new CustomToolStripRenderer(this, colorTable);

			var otherSection = iniFile.GetSection("Other");
			LoadLook(otherSection, palette, Other);

			var controlSection = iniFile.GetSection("Control");
			LoadLook(controlSection, palette, Control);

			var formSection = iniFile.GetSection("Form");
			LoadLook(formSection, palette, Form);

			var buttonSection = iniFile.GetSection("Button");
			LoadLook(buttonSection, palette, Button);

			var treeViewSection = iniFile.GetSection("TreeView");
			LoadLook(treeViewSection, palette, TreeView);

			var richTextBoxSection = iniFile.GetSection("RichTextBox");
			LoadLook(richTextBoxSection, palette, RichTextBox);

			var linkLabelSection = iniFile.GetSection("LinkLabel");
			LoadLook(linkLabelSection, palette, LinkLabel);

			var listViewSection = iniFile.GetSection("ListView");
			LoadLook(listViewSection, palette, ListView);

			var secureTextBoxSection = iniFile.GetSection("SecureTextBox");
			LoadLook(secureTextBoxSection, palette, SecureTextBox);

			var checkBoxSection = iniFile.GetSection("CheckBox");
			LoadLook(checkBoxSection, palette, CheckBox);

			var checkBoxButtonSection = iniFile.GetSection("CheckBoxButton");
			LoadLook(checkBoxButtonSection, palette, CheckBoxButton);

			var menuItemSection = iniFile.GetSection("MenuItem");
			LoadLook(menuItemSection, palette, MenuItem);

			TreeViewDrawMode = TreeViewDrawMode.OwnerDrawText;

			if (!string.IsNullOrEmpty(ListView.BackgroundImage))
			{
				var exeLocation = Path.GetDirectoryName(typeof(KeePass.Program).Assembly.Location);
				var pluginsPath = Path.Combine(exeLocation, AppDefs.PluginsDir);
				var imagePath = Path.Combine(pluginsPath, ListView.BackgroundImage);

				if (File.Exists(imagePath) && imagePath.StartsWith(exeLocation))
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

		private static void LoadLook<T>(Dictionary<string, string> controlSection, Palette palette, T look)
		{
			var properties = look.GetType().GetProperties();
			foreach (var property in properties)
			{
				string value;
				if (!controlSection.TryGetValue(property.Name, out value))
					continue;

				if (string.IsNullOrEmpty(value))
					continue;

				if (property.PropertyType == typeof(Color))
					property.SetValue(look, palette.GetColor(controlSection[property.Name]), null);

				if (property.PropertyType == typeof(FlatStyle))
					property.SetValue(look, Enum.Parse(typeof(FlatStyle), value, true), null);

				if (property.PropertyType == typeof(BorderStyle))
					property.SetValue(look, Enum.Parse(typeof(BorderStyle), value, true), null);

				if (property.PropertyType == typeof(ContentAlignment))
					property.SetValue(look, Enum.Parse(typeof(ContentAlignment), value, true), null);

				if (property.PropertyType == typeof(string))
					property.SetValue(look, value, null);
			}
		}
	}
}
