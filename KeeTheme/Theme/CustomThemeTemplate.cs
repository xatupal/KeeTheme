using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace KeeTheme.Theme
{
	internal class CustomThemeTemplate
	{
		[Category("Theme")]
		public string Name { get; set; }
		[Browsable(false)]
		public Palette Palette { get; private set; }

		[Category("Appearance")]
		public OtherLook Other { get; private set; }
		[Category("Appearance")]
		public ControlLook Control { get; private set; }
		[Category("Appearance")]
		public ControlLook Form { get; private set; }
		[Category("Appearance")]
		public ButtonLook Button { get; private set; }
		[Category("Appearance")]
		public TreeViewLook TreeView { get; private set; }
		[Category("Appearance")]
		public RichTextBoxLook RichTextBox { get; private set; }
		[Category("Appearance")]
		public LinkLabelLook LinkLabel { get; private set; }
		[Category("Appearance")]
		public ListViewLook ListView { get; private set; }
		[Category("Appearance")]
		public TabControlLook TabControl { get; private set; }
		[Category("Appearance")]
		public ControlLook SecureTextBox { get; private set; }
		[Category("Appearance")]
		public CheckBoxLook CheckBox { get; private set; }
		[Category("Appearance")]
		public CheckBoxButtonLook CheckBoxButton { get; private set; }
		[Category("Appearance")]
		public MenuLook MenuItem { get; private set; }
		[Category("Appearance")]
		public ToolStripLook ToolStrip { get; private set; }
		[Category("Appearance")]
		public PropertyGridLook PropertyGrid { get; private set; }

		private CustomThemeTemplate()
		{
			Name = "Dark Theme";
			Palette = new Palette();
			Other = new OtherLook();
			Control = new ControlLook();
			Form = new ControlLook();
			Button = new ButtonLook();
			TreeView = new TreeViewLook();
			RichTextBox = new RichTextBoxLook();
			LinkLabel = new LinkLabelLook();
			ListView = new ListViewLook();
			TabControl = new TabControlLook();
			SecureTextBox = new ControlLook();
			CheckBox = new CheckBoxLook();
			CheckBoxButton = new CheckBoxButtonLook();
			MenuItem = new MenuLook();
			ToolStrip = new ToolStripLook();
			PropertyGrid = new PropertyGridLook();
		}

		public CustomThemeTemplate(IniFile iniFile) : this()
		{
			var themeSection = iniFile.GetSection("KeeTheme");
			if (themeSection.ContainsKey("Name"))
				Name = themeSection["Name"];

			var paletteSection = iniFile.GetSection("Palette");
			Palette = new Palette(paletteSection);

			var otherSection = iniFile.GetSection("Other");
			LoadLook(otherSection, Palette, Other);

			var controlSection = iniFile.GetSection("Control");
			LoadLook(controlSection, Palette, Control);

			var formSection = iniFile.GetSection("Form");
			LoadLook(formSection, Palette, Form);

			var buttonSection = iniFile.GetSection("Button");
			LoadLook(buttonSection, Palette, Button);

			var treeViewSection = iniFile.GetSection("TreeView");
			LoadLook(treeViewSection, Palette, TreeView);

			var richTextBoxSection = iniFile.GetSection("RichTextBox");
			LoadLook(richTextBoxSection, Palette, RichTextBox);

			var linkLabelSection = iniFile.GetSection("LinkLabel");
			LoadLook(linkLabelSection, Palette, LinkLabel);

			var listViewSection = iniFile.GetSection("ListView");
			LoadLook(listViewSection, Palette, ListView);
			
			var tabControlSection = iniFile.GetSection("TabControl");
			LoadLook(tabControlSection, Palette, TabControl);

			var secureTextBoxSection = iniFile.GetSection("SecureTextBox");
			LoadLook(secureTextBoxSection, Palette, SecureTextBox);

			var checkBoxSection = iniFile.GetSection("CheckBox");
			LoadLook(checkBoxSection, Palette, CheckBox);

			var checkBoxButtonSection = iniFile.GetSection("CheckBoxButton");
			LoadLook(checkBoxButtonSection, Palette, CheckBoxButton);

			var menuItemSection = iniFile.GetSection("MenuItem");
			LoadLook(menuItemSection, Palette, MenuItem);
			
			var toolStripSection = iniFile.GetSection("ToolStrip");
			LoadLook(toolStripSection, Palette, ToolStrip);		

			var propertyGridSection = iniFile.GetSection("PropertyGrid");
			LoadLook(propertyGridSection, Palette, PropertyGrid);		
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
		
		public IniFile GetIniFile()
		{
			var iniFile = new IniFile();
			
			var section = iniFile.AddSection("KeeTheme");
			section.Add("Name", Name);

			SavePalette(iniFile, Palette);
			SaveLook(iniFile, "Other", Other);
			SaveLook(iniFile, "Control", Control);
			SaveLook(iniFile, "Form", Form);
			SaveLook(iniFile, "Button", Button);
			SaveLook(iniFile, "TreeView", TreeView);
			SaveLook(iniFile, "RichTextBox", RichTextBox);
			SaveLook(iniFile, "LinkLabel", LinkLabel);
			SaveLook(iniFile, "ListView", ListView);
			SaveLook(iniFile, "TabControl", TabControl);
			SaveLook(iniFile, "SecureTextBox", SecureTextBox);
			SaveLook(iniFile, "CheckBox", CheckBox);
			SaveLook(iniFile, "CheckBoxButton", CheckBoxButton);
			SaveLook(iniFile, "MenuItem", MenuItem);
			SaveLook(iniFile, "ToolStrip", ToolStrip);
			SaveLook(iniFile, "PropertyGrid", PropertyGrid);

			return iniFile;
		}

		private void SavePalette(IniFile iniFile, Palette palette)
		{
			var section = iniFile.AddSection("Palette");
			var colors = palette.GetColors();
			for (int i = 0; i < colors.Length; i++)
			{
				section.Add("Color" + i, GetColorTextValue(colors[i]));
			}
		}

		private void SaveLook<T>(IniFile iniFile, string sectionName, T look)
		{
			var properties = look.GetType().GetProperties();
			var section = iniFile.AddSection(sectionName);
			foreach (var property in properties)
			{
				var value = property.GetValue(look, null) ?? string.Empty;
				var textValue = value.ToString();
				if (property.PropertyType == typeof(Color))
					textValue = GetColorTextValue((Color) value);
					
				section.Add(property.Name, textValue);
			}
		}

		private string GetColorTextValue(Color value)
		{
			return value == Color.Empty ? "" : string.Format("({0}, {1}, {2})", value.R, value.G, value.B);
		}
	}
}