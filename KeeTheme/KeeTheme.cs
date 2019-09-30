using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using KeePass.App;
using KeePass.UI;
using KeeTheme.Decorators;
using KeeTheme.Theme;

namespace KeeTheme
{
	internal class KeeTheme
	{
		private readonly DefaultTheme _defaultTheme;

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

		public KeeTheme()
		{
			_defaultTheme = new DefaultTheme();
			_customTheme = new CustomTheme(IniFile.GetFromFile() ?? IniFile.GetFromResources());
			_theme = _defaultTheme;
		}

		private void SetEnable(bool enable)
		{
			_enabled = enable;

			if (_enabled)
				_customTheme = new CustomTheme(IniFile.GetFromFile() ?? IniFile.GetFromResources());

			_theme = _enabled ? _customTheme : _defaultTheme;

			ToolStripManager.Renderer = _theme.ToolStripRenderer;
			ObjectListViewDecorator.Initialize();

			ApplyOther();
		}

		private void ApplyOther()
		{
			var colorControlNormalField =
				typeof(AppDefs).GetField("ColorControlNormal", BindingFlags.Static | BindingFlags.Public);
			var colorControlDisabledField =
				typeof(AppDefs).GetField("ColorControlDisabled", BindingFlags.Static | BindingFlags.Public);

			if (colorControlNormalField != null)
				colorControlNormalField.SetValue(null, _theme.Other.ControlNormalColor);

			if (colorControlDisabledField != null)
				colorControlDisabledField.SetValue(null, _theme.Other.ControlDisabledColor);
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

			var toolStrip = control as ToolStrip;
			if (toolStrip != null) Apply(toolStrip);

			var menuStrip = control as MenuStrip;
			if (menuStrip != null) Apply(menuStrip);

			var contextMenuStrip = control as ContextMenuStrip;
			if (contextMenuStrip != null) Apply(contextMenuStrip);

			var statusStrip = control as StatusStrip;
			if (statusStrip != null) Apply(statusStrip);
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

			var textBox = (SecureTextBoxEx) sender;
			if (textBox.BackColor == SystemColors.Window)
				textBox.BackColor = _theme.SecureTextBox.BackColor;
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

		private IEnumerable<Control> GetComponents(Form form)
		{
			var componentsField = form.GetType()
				.GetField("components", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			if (componentsField != null)
			{
				var components = componentsField.GetValue(form) as IContainer;
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
		}

		private void Apply(LinkLabel linkLabel)
		{
			linkLabel.LinkColor = _theme.LinkLabel.LinkColor;
		}

		private void Apply(TreeView treeView)
		{
			treeView.BorderStyle = _theme.TreeView.BorderStyle;
			treeView.BackColor = _theme.TreeView.BackColor;
			treeView.DrawMode = _theme.TreeViewDrawMode;

			treeView.DrawNode -= HandleTreeViewDrawNode;
			treeView.DrawNode += HandleTreeViewDrawNode;
		}

		private void HandleTreeViewDrawNode(object sender, DrawTreeNodeEventArgs e)
		{
			e.DrawDefault = true;
			e.Node.ForeColor = e.State == TreeNodeStates.Selected
				? _theme.TreeView.SelectionColor
				: _theme.TreeView.ForeColor;
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

			var decorator = listView.Controls.OfType<ListViewDecorator>().FirstOrDefault();
			if (decorator == null)
			{
				decorator = new ListViewDecorator(listView , _theme);
			}

			decorator.EnableTheme(_enabled, _theme);
		}

	}
}