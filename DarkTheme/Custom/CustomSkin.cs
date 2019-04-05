using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using KeePass.UI.ToolStripRendering;

namespace DarkTheme.Custom
{
	class CustomSkin : DefaultSkin
	{
		public static CustomSkin LoadFromIni()
		{
			var iniFile = GetIniFile();
			if (iniFile == null)
				return null;

			var customSkin = new CustomSkin();

			var darkThemeSection = iniFile.GetSection("DarkTheme");
			if (darkThemeSection.ContainsKey("Name"))
				customSkin.Name = darkThemeSection["Name"];

			var paletteSection = iniFile.GetSection("Palette");
			var palette = new Palette(paletteSection);

			var toolStripSection = iniFile.GetSection("ToolStrip");
			var customColors = toolStripSection.ToDictionary(x => x.Key, x => palette.GetColor(x.Value));
			var colorTable = new CustomColorTable(customColors);
			customSkin.ToolStripRenderer = new ProExtTsr(colorTable);

			var otherSection = iniFile.GetSection("Other");
			LoadLook(otherSection, palette, customSkin.Other);

			var controlSection = iniFile.GetSection("Control");
			LoadLook(controlSection, palette, customSkin.Control);

			var formSection = iniFile.GetSection("Form");
			LoadLook(formSection, palette, customSkin.Form);

			var buttonSection = iniFile.GetSection("Button");
			LoadLook(buttonSection, palette, customSkin.Button);

			var treeViewSection = iniFile.GetSection("TreeView");
			LoadLook(treeViewSection, palette, customSkin.TreeView);

			var richTextBoxSection = iniFile.GetSection("RichTextBox");
			LoadLook(richTextBoxSection, palette, customSkin.RichTextBox);

			var linkLabelSection = iniFile.GetSection("LinkLabel");
			LoadLook(linkLabelSection, palette, customSkin.LinkLabel);

			var listViewSection = iniFile.GetSection("ListView");
			LoadLook(listViewSection, palette, customSkin.ListView);

			var secureTextBoxSection = iniFile.GetSection("SecureTextBox");
			LoadLook(secureTextBoxSection, palette, customSkin.SecureTextBox);

			customSkin.TreeViewDrawMode = TreeViewDrawMode.OwnerDrawText;

			if (customSkin.ListView.BackColor != Color.Empty)
			{
				customSkin.ListViewBackgroundTiled = true;

				var bitmap = new Bitmap(1, 1);
				bitmap.SetPixel(0, 0, customSkin.ListView.BackColor);
				customSkin.ListViewBackground = bitmap;
			}

			return customSkin;
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
			}
		}

		private static IniFile GetIniFile()
		{
			var location = typeof(CustomSkin).Assembly.Location;
			var path = Path.ChangeExtension(location, ".ini");
			if (!File.Exists(path))
				return null;

			try
			{
				return new IniFile(path);
			}
			catch (InvalidDataException)
			{
				return null;
			}
		}
	}
}
