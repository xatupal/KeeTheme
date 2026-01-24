using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using KeePass.App;
using KeePass.UI;
using KeePassLib.Utility;
using KeeTheme.Decorators;
using KeeTheme.Options;
using KeeTheme.Theme;

namespace KeeTheme
{
	internal class KeeTheme
	{
		private readonly KeeThemeOptions _options;
		private readonly CustomTheme _defaultTheme;

		private ITheme _customTheme;
		private ITheme _theme;
		private bool _enabled;

		public bool Enabled
		{
			get { return _enabled; }
			set { SetEnable(value); }
		}

		public string Name
		{
			get { return _customTheme.Name; }
		}

		public KeeTheme(KeeThemeOptions options)
		{
			_options = options;
			_defaultTheme = CustomTheme.GetDefaultTheme();
			_customTheme = GetCustomTheme();
			_theme = _defaultTheme;
		}

		private void SetEnable(bool enable)
		{
			_enabled = enable;

			if (_enabled)
				_customTheme = GetCustomTheme();

			_theme = _enabled ? _customTheme : _defaultTheme;

			ToolStripManager.Renderer = _theme.ToolStripRenderer;
			ObjectListViewDecorator.Initialize();
			KnownColorsDecorator.Apply(_theme, _enabled);

			ApplyOther();
		}

		private ITheme GetCustomTheme()
		{
			var templateFile = TemplateReader.Get(_options.Template) ?? TemplateReader.GetDefaultTemplate();
			var themeTemplate = new CustomThemeTemplate(templateFile);
			return new CustomTheme(themeTemplate);
		}

		private void ApplyOther()
		{
			var colorControlNormalField =
				typeof(AppDefs).GetField("ColorControlNormal", BindingFlags.Static | BindingFlags.Public);
			var colorControlDisabledField =
				typeof(AppDefs).GetField("ColorControlDisabled", BindingFlags.Static | BindingFlags.Public);
			var colorEditError =
				typeof(AppDefs).GetField("ColorEditError", BindingFlags.Static | BindingFlags.Public);

			if (colorControlNormalField != null)
				colorControlNormalField.SetValue(null, _theme.Other.ControlNormalColor);

			if (colorControlDisabledField != null)
				colorControlDisabledField.SetValue(null, _theme.Other.ControlDisabledColor);

			if (colorEditError != null)
				colorEditError.SetValue(null, _theme.Other.ColorEditError);
		}

		public void Apply(Control control)
		{
			if (control.InvokeRequired)
			{
				control.Invoke(new MethodInvoker(() => Apply(control)));
			}

			if (!(control is ToolStrip))
			{
				control.BackColor = _theme.Control.BackColor;
				control.ForeColor = _theme.Control.ForeColor;
			}

			var form = control as Form;
			if (form != null) Apply(form);

			var userControl = control as UserControl;
			if (userControl != null) Apply(userControl);

			var dataGridView = control as DataGridView;
			if (dataGridView != null) Apply(dataGridView);
			
			var button = control as Button;
			if (button != null) Apply(button);

			var treeView = control as TreeView;
			if (treeView != null) Apply(treeView);

			var richTextBox = control as RichTextBox;
			if (richTextBox != null) Apply(richTextBox);

			var linkLabel = control as LinkLabel;
			if (linkLabel != null) Apply(linkLabel);

			var listView = control as ListView;
			if (listView != null) Apply(listView);

			var secureTextBoxEx = control as SecureTextBoxEx;
			if (secureTextBoxEx != null) Apply(secureTextBoxEx);

			var hotKeyControlEx = control as HotKeyControlEx;
			if (hotKeyControlEx != null) Apply(hotKeyControlEx);

			var toolStrip = control as ToolStrip;
			if (toolStrip != null) Apply(toolStrip);

			var menuStrip = control as MenuStrip;
			if (menuStrip != null) Apply(menuStrip);

			var contextMenuStrip = control as ContextMenuStrip;
			if (contextMenuStrip != null) Apply(contextMenuStrip);

			var statusStrip = control as StatusStrip;
			if (statusStrip != null) Apply(statusStrip);

			var tabControl = control as TabControl;
			if (tabControl != null) Apply(tabControl);

			var qualityProgressBar = control as QualityProgressBar;
			if (qualityProgressBar != null) Apply(qualityProgressBar);

			var comboBox = control as ComboBox;
			if (comboBox != null) Apply(comboBox);

			var checkBox = control as CheckBox;
			if (checkBox != null) Apply(checkBox);
			
			var propertyGrid = control as PropertyGrid;
			if (propertyGrid != null) Apply(propertyGrid);
			
			OverrideResetBackground(control);
			OverrideScrollBarsSetExplorerTheme(control);
			OverrideOptionsFormSetExplorerTheme(control);
		}

		private void OverrideScrollBarsSetExplorerTheme(Control control)
		{
			if (!CanHaveScrollBars(control) || MonoWorkarounds.IsRequired()) 
				return;
			
			TrySetWindowTheme(control.Handle, _enabled);

			// Subscribe to handle creation for dynamically created controls
			control.HandleCreated += (s, e) =>
			{
				var createdControl = s as Control;
				if (createdControl != null && createdControl.IsHandleCreated)
					SetWindowTheme(createdControl.Handle, "DarkMode_Explorer", null);
			};
		}

		private void OverrideOptionsFormSetExplorerTheme(Control control)
		{
			if (MonoWorkarounds.IsRequired())
				return;
			
			var form = control as Form;
			if (form != null)
			{
				form.Load -= HandleOptionsFormLoad;
				form.Load += HandleOptionsFormLoad;
			}
		}

		private void HandleOptionsFormLoad(object sender, EventArgs e)
		{
			var form = sender as Form;
			if (form == null) return;

			// Re-apply dark mode theme to override SetExplorerTheme
			foreach (var control in form.Controls)
			{
				var listView = control as CustomListViewEx;
				if (listView != null && listView.IsHandleCreated)
				{
					TrySetWindowTheme(listView.Handle, _enabled);
				}
			}
		}

		private bool CanHaveScrollBars(Control control)
		{
			return control is TextBoxBase ||
			       control is ListBox ||
			       control is ListView ||
			       control is TreeView ||
			       control is DataGridView ||
			       control is Panel ||
			       control is UserControl ||
			       control is Form ||
			       control is ScrollableControl || 
			       control is TabControl;
		}

		private void OverrideResetBackground(Control control)
		{
			if (control.Name == "m_cmbStringName" && control.Parent.Name == "EditStringForm")
			{
				control.BackColorChanged += (sender, args) =>
				{
					if (control.BackColor.IsSystemColor)
						control.BackColor = _theme.Control.BackColor;
				};
			}
			if (control.Name == "m_tbSearch" && control.Parent.Name == "OptionsForm")
			{
				control.BackColorChanged += (sender, args) =>
				{
					if (control.BackColor.IsSystemColor)
						control.BackColor = _theme.Control.BackColor;
				};
			}
		}

		private void Apply(DataGridView dataGridView)
		{
			dataGridView.BackgroundColor = _theme.Control.BackColor;
			dataGridView.RowsDefaultCellStyle.BackColor = _theme.Control.BackColor;
			dataGridView.RowsDefaultCellStyle.ForeColor = _theme.Control.ForeColor;
		}
		
		private void Apply(CheckBox checkBox)
		{
			var checkBoxLook = checkBox.Appearance == Appearance.Button
				? _theme.CheckBoxButton
				: _theme.CheckBox;

			checkBox.BackColor = checkBoxLook.BackColor;
			checkBox.ForeColor = checkBoxLook.ForeColor;
			checkBox.FlatStyle = checkBoxLook.FlatStyle;
			checkBox.FlatAppearance.BorderColor = checkBoxLook.BorderColor;
			checkBox.FlatAppearance.CheckedBackColor = checkBoxLook.CheckedBackColor;
			checkBox.FlatAppearance.MouseDownBackColor = checkBoxLook.MouseDownBackColor;
			checkBox.FlatAppearance.MouseOverBackColor = checkBoxLook.MouseOverBackColor;
			
			checkBox.EnabledChanged -= HandleCheckBoxEnabledChanged;
			checkBox.EnabledChanged += HandleCheckBoxEnabledChanged;
		}

		private void HandleCheckBoxEnabledChanged(object sender, EventArgs e)
		{
			var checkBox = (CheckBox) sender;
			if (checkBox.Enabled)
			{
				checkBox.Paint -= HandleCheckBoxPaint;
				checkBox.Invalidate();
			}
			else
			{
				checkBox.Paint += HandleCheckBoxPaint;
			}
		}

		private void HandleCheckBoxPaint(object sender, PaintEventArgs e)
		{
			var checkBox = (CheckBox)sender;
			var disabledForeColor = ControlPaint.Dark(_theme.Button.ForeColor, 0.25f);
			var glyphSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, CheckBoxState.UncheckedNormal);
			var flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine;
			var clientRectangle = new Rectangle(glyphSize.Width, -1, 
				checkBox.ClientRectangle.Size.Width - glyphSize.Width, checkBox.ClientRectangle.Size.Height);
			
			TextRenderer.DrawText(e.Graphics, checkBox.Text, checkBox.Font, clientRectangle, disabledForeColor, flags);
		}

		private void Apply(ComboBox comboBox)
		{
			if (comboBox.DropDownStyle == ComboBoxStyle.DropDownList)
				comboBox.FlatStyle = FlatStyle.Popup;

			comboBox.BackColorChanged -= HandleComboBoxBackColorChanged;
			comboBox.BackColorChanged += HandleComboBoxBackColorChanged;
		}

		private void HandleComboBoxBackColorChanged(object sender, EventArgs e)
		{
			if (!_enabled)
			{
				return;
			}
			
			var comboBox = (ComboBox) sender;
			if (comboBox.BackColor == SystemColors.Window)
				comboBox.BackColor = _theme.Control.BackColor;
		}

		private void Apply(QualityProgressBar qualityProgressBar)
		{
			qualityProgressBar.ForeColor = _theme.Form.BackColor;
		}

		private void Apply(TabControl tabControl)
		{
			var decoratorName = tabControl.Name + "_decorator";
			var decorator =
				tabControl.Parent.Controls.Find(decoratorName, false).FirstOrDefault() as TabControlDecorator;
			
			if (decorator == null)
			{
				decorator = new TabControlDecorator(tabControl, _theme);
				decorator.Name = decoratorName;
			}

			tabControl.BackColor = _theme.TabControl.BackColor;
			tabControl.ForeColor = _theme.TabControl.ForeColor;
			
			decorator.EnableTheme(_enabled, _theme);			
			
			tabControl.ControlAdded -= HandleTabControlAdded;
			tabControl.ControlAdded += HandleTabControlAdded;
		}

		private void HandleTabControlAdded(object sender, ControlEventArgs e)
		{
			if (e.Control is TabPage)
			{
				var visitor = new ControlVisitor(Apply);
				visitor.Visit(e.Control);
			}
		}

		private void Apply(StatusStrip statusStrip)
		{
			statusStrip.BackColor = _theme.MenuItem.BackColor;
			statusStrip.ForeColor = _theme.MenuItem.ForeColor;

			Apply(statusStrip.Items);
		}

		private void Apply(ContextMenuStrip contextMenuStrip)
		{
			contextMenuStrip.BackColor = _theme.MenuItem.BackColor;
			contextMenuStrip.ForeColor = _theme.MenuItem.ForeColor;

			Apply(contextMenuStrip.Items);
		}

		private void Apply(MenuStrip menuStrip)
		{
			menuStrip.BackColor = _theme.MenuItem.BackColor;
			menuStrip.ForeColor = _theme.MenuItem.ForeColor;

			Apply(menuStrip.Items);
		}

		private void Apply(ToolStrip toolStrip)
		{
			toolStrip.BackColor = _theme.MenuItem.BackColor;
			toolStrip.ForeColor = _theme.MenuItem.ForeColor;

			Apply(toolStrip.Items);
		}

		private void Apply(ToolStripItemCollection toolStripItemCollection)
		{
			foreach (ToolStripItem item in toolStripItemCollection)
			{
				item.ForeColor = _theme.MenuItem.ForeColor;
				item.BackColor = _theme.MenuItem.BackColor;

				var menuItem = item as ToolStripMenuItem;
				if (menuItem != null)
				{
					menuItem.DropDownOpening -= HandleMenuItemOnDropDownOpening;
					menuItem.DropDownOpening += HandleMenuItemOnDropDownOpening;
				}
			}
		}

		private void HandleMenuItemOnDropDownOpening(object sender, EventArgs e)
		{
			var menuItem = (ToolStripMenuItem) sender;
			Apply(menuItem.DropDownItems);
		}

		private void Apply(SecureTextBoxEx secureTextBoxEx)
		{
			secureTextBoxEx.BackColorChanged -= HandleSecureTextBoxExOnBackColorChanged;
			secureTextBoxEx.BackColorChanged += HandleSecureTextBoxExOnBackColorChanged;
		}

		private void HandleSecureTextBoxExOnBackColorChanged(object sender, EventArgs e)
		{
			if (!_enabled)
			{
				return;
			}

			var textBox = (SecureTextBoxEx)sender;
			if (textBox.BackColor == SystemColors.Window)
				textBox.BackColor = _theme.SecureTextBox.BackColor;
		}

		private void Apply(HotKeyControlEx hotKeyControlEx)
		{
			hotKeyControlEx.BackColorChanged -= HandleHotKeyControlExOnBackColorChanged;
			hotKeyControlEx.BackColorChanged += HandleHotKeyControlExOnBackColorChanged;
		}

		private void HandleHotKeyControlExOnBackColorChanged(object sender, EventArgs e)
		{
			if (!_enabled)
			{
				return;
			}

			var textBox = (HotKeyControlEx) sender;
			if (textBox.BackColor == SystemColors.Window)
				textBox.BackColor = _theme.Control.BackColor;
		}

		private void Apply(Form form)
		{
			form.BackColor = _theme.Form.BackColor;
			form.ForeColor = _theme.Form.ForeColor;

			foreach (var component in GetComponents(form))
			{
				Apply(component);
			}
		}

		private void Apply(UserControl userControl)
		{
			userControl.BackColor = _theme.Form.BackColor;
			userControl.ForeColor = _theme.Form.ForeColor;

			foreach (var component in GetComponents(userControl))
			{
				Apply(component);
			}
		}

		private IEnumerable<Control> GetComponents(ContainerControl containerControl)
		{
			var componentsField = containerControl.GetType()
				.GetField("components", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			if (componentsField != null)
			{
				var components = componentsField.GetValue(containerControl) as IContainer;
				if (components != null)
				{
					return components.Components.OfType<Control>();
				}
			}

			return Enumerable.Empty<Control>();
		}

		private void Apply(Button button)
		{
			button.BackColor = _theme.Button.BackColor;
			button.ForeColor = _theme.Button.ForeColor;
			button.FlatAppearance.BorderColor = _theme.Button.BorderColor;
			button.FlatStyle = _theme.Button.FlatStyle;

			if (button is SplitButtonEx)
			{
				var decorator = button.Controls.OfType<SplitButtonExDecorator>().FirstOrDefault() 
				                ?? new SplitButtonExDecorator((SplitButtonEx) button, _theme);
				
				decorator.EnableTheme(_enabled, _theme);
			}

			button.EnabledChanged -= HandleButtonEnabledChanged;
			button.EnabledChanged += HandleButtonEnabledChanged;
		}

		private void HandleButtonEnabledChanged(object sender, EventArgs e)
		{
			var button = (Button) sender;
			if (button.Enabled)
			{
				button.Paint -= HandleButtonPaint;
			}
			else
			{
				button.Paint += HandleButtonPaint;
			}
		}

		private void HandleButtonPaint(object sender, PaintEventArgs e)
		{
			var button = (Button)sender;
			var disabledForeColor = ControlPaint.Dark(_theme.Button.ForeColor, 0.25f);
			if (button.Enabled)
			{
				disabledForeColor = _theme.Button.ForeColor;
			}
			var flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;
			TextRenderer.DrawText(e.Graphics, button.Text, button.Font, button.ClientRectangle, disabledForeColor, flags);
		}

		private void Apply(LinkLabel linkLabel)
		{
			linkLabel.LinkColor = _theme.LinkLabel.LinkColor;
		}

		private void Apply(TreeView treeView)
		{
			treeView.BorderStyle = _theme.TreeView.BorderStyle;
			treeView.BackColor = _theme.TreeView.BackColor;
			
			if (!MonoWorkarounds.IsRequired())
			{
				treeView.DrawMode = _theme.TreeViewDrawMode;
				treeView.DrawNode -= HandleTreeViewDrawNode;
				treeView.DrawNode += HandleTreeViewDrawNode;
			}
		}

		[DllImport("UxTheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
		private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

		public static void TrySetWindowTheme(IntPtr hWnd, bool enable)
		{
			if (hWnd == IntPtr.Zero || MonoWorkarounds.IsRequired())
				return;

			try
			{
				SetWindowTheme(hWnd, enable ? "DarkMode_Explorer" : "explorer", null);
			}
			catch (Exception)
			{
				// ignored
			}
		}
		
		private void HandleTreeViewDrawNode(object sender, DrawTreeNodeEventArgs e)
		{
			// DrawDefault = true does not have TextFormatFlags.NoPrefix flag set
			var node = e.Node;

			var isNodeSelected = (e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected;
			var foreColor = isNodeSelected && node.TreeView.Focused 
				? _theme.TreeView.SelectionColor 
				: _theme.TreeView.ForeColor != Color.Empty ? _theme.TreeView.ForeColor : node.TreeView.ForeColor;
			
			var backColor = isNodeSelected ? _theme.TreeView.SelectionBackColor : _theme.TreeView.BackColor;
			
			var font = node.NodeFont ?? node.TreeView.Font;
			var size = TextRenderer.MeasureText(node.Text, font, e.Bounds.Size, TextFormatFlags.NoPrefix);
			var rectangle = new Rectangle(new Point(node.Bounds.X - 1, node.Bounds.Y), new Size(size.Width, node.Bounds.Height));

			using (var backColorBrush = new SolidBrush(backColor))
				e.Graphics.FillRectangle(backColorBrush, rectangle);

			if (isNodeSelected && node.TreeView.Focused)
				ControlPaint.DrawFocusRectangle(e.Graphics, rectangle, foreColor, backColor);
			
			TextRenderer.DrawText(e.Graphics, node.Text, font, rectangle, foreColor, TextFormatFlags.NoPrefix);
		}
		
		private void Apply(RichTextBox richTextBox)
		{
			var decorator = richTextBox.Parent as RichTextBoxDecorator;
			if (decorator == null)
			{
				decorator = new RichTextBoxDecorator(richTextBox, _theme);
			}

			decorator.EnableTheme(_enabled, _theme);

		}

		private void Apply(ListView listView)
		{
			if (ObjectListViewDecorator.CanDecorate(listView))
			{
				ObjectListViewDecorator.Apply(listView, _theme);
				return;
			}

			var decorator = listView.Controls.OfType<ListViewDecorator>().FirstOrDefault()
			                ?? new ListViewDecorator(listView , _theme);

			decorator.EnableTheme(_enabled, _theme);
		}

		private void Apply(PropertyGrid propertyGrid)
		{
			propertyGrid.CategoryForeColor = _theme.PropertyGrid.CategoryForeColor;
			propertyGrid.LineColor = _theme.PropertyGrid.LineColor;
		}
	}
}