using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using DarkTheme.Skin;
using KeePass;
using KeePass.App;
using KeePass.UI;

namespace DarkTheme
{
	internal class DarkTheme
	{
		private readonly DefaultSkin _defaultSkin;

		private ISkin _customSkin;
		private ISkin _skin;
		private bool _enabled;

		public bool Enabled
		{
			get { return _enabled; }
			set { SetEnable(value); }
		}

		public string Name
		{
			get { return _customSkin.Name; }
		}

		public DarkTheme(bool enabled)
		{
			_defaultSkin = new DefaultSkin();

			SetEnable(enabled);
		}

		private void SetEnable(bool enable)
		{
			_enabled = enable;

			if (_enabled || _skin == null)
				_customSkin = new CustomSkin(IniFile.GetFromFile() ?? IniFile.GetFromResources());

			_skin = _enabled ? _customSkin : _defaultSkin;

			ToolStripManager.Renderer = _skin.ToolStripRenderer;

			ApplyOther();
		}

		private void ApplyOther()
		{
			var colorControlNormalField =
				typeof(AppDefs).GetField("ColorControlNormal", BindingFlags.Static | BindingFlags.Public);
			var colorControlDisabledField =
				typeof(AppDefs).GetField("ColorControlDisabled", BindingFlags.Static | BindingFlags.Public);

			if (colorControlNormalField != null)
				colorControlNormalField.SetValue(null, _skin.Other.ControlNormalColor);

			if (colorControlDisabledField != null)
				colorControlDisabledField.SetValue(null, _skin.Other.ControlDisabledColor);
		}

		public void Apply(Control control)
		{
			control.BackColor = _skin.Control.BackColor;
			control.ForeColor = _skin.Control.ForeColor;

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
				textBox.BackColor = _skin.SecureTextBox.BackColor;
		}

		private void Apply(Form form)
		{
			form.BackColor = _skin.Form.BackColor;
			form.ForeColor = _skin.Form.ForeColor;
		}

		private void Apply(Button button)
		{
			button.FlatAppearance.BorderColor = _skin.Button.BorderColor;
			button.FlatStyle = _skin.Button.FlatStyle;
		}

		private void Apply(LinkLabel linkLabel)
		{
			linkLabel.LinkColor = _skin.LinkLabel.LinkColor;
		}

		private void Apply(TreeView treeView)
		{
			treeView.BorderStyle = _skin.TreeView.BorderStyle;
			treeView.BackColor = _skin.TreeView.BackColor;
			treeView.DrawMode = _skin.TreeViewDrawMode;

			treeView.DrawNode -= HandleTreeViewDrawNode;
			treeView.DrawNode += HandleTreeViewDrawNode;
		}

		private void HandleTreeViewDrawNode(object sender, DrawTreeNodeEventArgs e)
		{
			e.DrawDefault = true;
			e.Node.ForeColor = e.State == TreeNodeStates.Selected
				? _skin.TreeView.SelectionColor
				: _skin.TreeView.ForeColor;
		}

		private void Apply(RichTextBox richTextBox)
		{
			richTextBox.BorderStyle = _skin.RichTextBox.BorderStyle;
			richTextBox.TextChanged -= HandleRichTextBoxTextChanged;
			richTextBox.TextChanged += HandleRichTextBoxTextChanged;
		}

		private void HandleRichTextBoxTextChanged(object sender, EventArgs e)
		{
			var richTextBox = (RichTextBox) sender;
			var selectionStart = richTextBox.SelectionStart;
			var selectionLength = richTextBox.SelectionLength;

			richTextBox.SelectAll();
			richTextBox.SelectionColor = _skin.RichTextBox.SelectionColor;
			richTextBox.Select(selectionStart, selectionLength);
		}

		private void Apply(ListView listView)
		{
			if (!listView.OwnerDraw && listView.View == View.Details)
			{
				listView.OwnerDraw = true;

				listView.DrawColumnHeader += HandleListViewDrawColumnHeader;
				listView.DrawItem += HandleListViewDrawItem;
				listView.DrawSubItem += HandleListViewDrawSubItem;
			}

			listView.BorderStyle = _skin.ListView.BorderStyle;
			listView.BackColor = _skin.ListView.BackColor;
			listView.BackgroundImage = _skin.ListViewBackground;
			listView.BackgroundImageTiled = _skin.ListViewBackgroundTiled;
		}

		private void HandleListViewDrawItem(object sender, DrawListViewItemEventArgs e)
		{
			if (!_enabled)
			{
				e.DrawDefault = true;
				return;
			}

			if (e.State == 0)
			{
				e.DrawFocusRectangle();
				return;
			}

			var backColor = Program.Config.MainWindow.EntryListAlternatingBgColors && (e.Item.Index & 1) == 0
				? _skin.ListView.EvenRowColor
				: _skin.ListView.OddRowColor;

			using (var brush = new SolidBrush(backColor))
			{
				e.Graphics.FillRectangle(brush, e.Bounds);
			}

			e.DrawFocusRectangle();
		}

		private void HandleListViewDrawSubItem(object sender, DrawListViewSubItemEventArgs e)
		{
			if (!_enabled)
			{
				e.DrawDefault = true;
				return;
			}

			var flags = GetTextFormatFlags(e.Header.TextAlign);
			var text = e.ItemIndex == -1 ? e.Item.Text : e.SubItem.Text;
			var font = e.ItemIndex == -1 ? e.Item.Font : e.SubItem.Font;
			var color = e.ItemIndex == -1 ? e.Item.ForeColor : e.SubItem.ForeColor;
			var textBounds = new Rectangle(e.Bounds.Location, e.Bounds.Size);

			const int iconSize = 16;
			if (e.ColumnIndex == 0)
			{
				e.Item.ImageList.Draw(e.Graphics, e.Bounds.X + 4, e.Bounds.Y + 1, iconSize, iconSize,
					e.Item.ImageIndex);

				textBounds.Inflate(-iconSize - 4 - 2, 0);
			}

			TextRenderer.DrawText(e.Graphics, " " + text + " ", font, textBounds, color, flags);

			using (var pen = new Pen(_skin.ListView.ColumnBorderColor))
				e.Graphics.DrawLine(pen, e.Bounds.Right - 2, e.Bounds.Y, e.Bounds.Right - 2, e.Bounds.Bottom);
		}

		private void HandleListViewDrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
		{
			if (!_enabled)
			{
				e.DrawDefault = true;
				return;
			}

			var listView = (ListView) sender;
			listView.BackgroundImage = listView.Items.Count == 0 ? _skin.ListViewBackground : null;

			var graphics = e.Graphics;
			var r = e.Bounds;

			using (Brush backBrush = new SolidBrush(_skin.ListView.HeaderBackColor))
			{
				graphics.FillRectangle(backBrush, r);
			}

			using (var pen = new Pen(_skin.ListView.HeaderColumnBorderColor))
			{
				graphics.DrawLine(pen, r.X, r.Y, r.Right, r.Y);
				graphics.DrawLine(pen, r.Right - 2, r.Y, r.Right - 2, r.Bottom);
			}

			var flags = GetTextFormatFlags(e.Header.TextAlign);
			TextRenderer.DrawText(graphics, " " + e.Header.Text + " ", e.Font, r,
				_skin.ListView.HeaderForeColor, flags);
		}

		private static TextFormatFlags GetTextFormatFlags(HorizontalAlignment textAlign)
		{
			var flags = textAlign == HorizontalAlignment.Left
				? TextFormatFlags.Left
				: textAlign == HorizontalAlignment.Center
					? TextFormatFlags.HorizontalCenter
					: TextFormatFlags.Right;

			return flags | TextFormatFlags.WordEllipsis | TextFormatFlags.VerticalCenter;
		}
	}
}