using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Windows.Forms;
using KeePass;
using KeePass.App;
using KeePass.UI;
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

		public KeeTheme(bool enabled)
		{
			_defaultTheme = new DefaultTheme();

			SetEnable(enabled);
		}

		private void SetEnable(bool enable)
		{
			_enabled = enable;

			if (_enabled || _theme == null)
				_customTheme = new CustomTheme(IniFile.GetFromFile() ?? IniFile.GetFromResources());

			_theme = _enabled ? _customTheme : _defaultTheme;

			ToolStripManager.Renderer = _theme.ToolStripRenderer;

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
			control.BackColor = _theme.Control.BackColor;
			control.ForeColor = _theme.Control.ForeColor;

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
				textBox.BackColor = _theme.SecureTextBox.BackColor;
		}

		private void Apply(Form form)
		{
			form.BackColor = _theme.Form.BackColor;
			form.ForeColor = _theme.Form.ForeColor;
		}

		private void Apply(Button button)
		{
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
			richTextBox.BorderStyle = _theme.RichTextBox.BorderStyle;
			richTextBox.TextChanged -= HandleRichTextBoxTextChanged;
			richTextBox.TextChanged += HandleRichTextBoxTextChanged;
		}

		private void HandleRichTextBoxTextChanged(object sender, EventArgs e)
		{
			var richTextBox = (RichTextBox) sender;
			var selectionStart = richTextBox.SelectionStart;
			var selectionLength = richTextBox.SelectionLength;

			richTextBox.SelectAll();
			richTextBox.SelectionColor = _theme.RichTextBox.SelectionColor;
			richTextBox.Select(selectionStart, selectionLength);
		}

		private void Apply(ListView listView)
		{
			if (!listView.OwnerDraw && listView.View == View.Details)
			{
				listView.OwnerDraw = true;

				listView.Resize += HandleListViewResize;
				listView.DrawColumnHeader += HandleListViewDrawColumnHeader;
				listView.DrawItem += HandleListViewDrawItem;
				listView.DrawSubItem += HandleListViewDrawSubItem;
			}

			listView.BorderStyle = _theme.ListView.BorderStyle;
			listView.BackColor = _theme.ListView.BackColor;

			if (_theme.ListViewBackgroundTiled)
			{
				listView.BackgroundImage = _theme.ListViewBackground;
				listView.BackgroundImageTiled = _theme.ListViewBackgroundTiled;
			}
		}

		private void HandleListViewResize(object sender, EventArgs e)
		{
			var listView = (ListView)sender;
			if (string.IsNullOrEmpty(_theme.ListView.BackgroundImage))
			{
				if (!_theme.ListViewBackgroundTiled)
				{
					if (listView.BackgroundImage != null)
						listView.BackgroundImage.Dispose();

					listView.BackgroundImage = null;
				}

				return;
			}

			if (_theme.ListViewBackground == null)
				return;

			var alignment = _theme.ListView.BackgroundImageAlignment > 0
				? _theme.ListView.BackgroundImageAlignment
				: ContentAlignment.TopLeft;

			var bgSize = new Size(listView.ClientSize.Width, listView.ClientSize.Height - listView.GetHeaderHeight());
			var image = new Bitmap(bgSize.Width, bgSize.Height, PixelFormat.Format32bppArgb);

			using (var g = Graphics.FromImage(image))
			using (var brush = new SolidBrush(_theme.ListView.BackColor))
			using (var pen = new Pen(_theme.ListView.ColumnBorderColor))
			{
				g.FillRectangle(brush, 0, 0, bgSize.Width, bgSize.Height);

				var location = GetImageLocation(alignment, bgSize, _theme.ListViewBackground.Size);
				g.DrawImage(_theme.ListViewBackground, location.X, location.Y);

				var offset = 0;
				foreach (ColumnHeader column in listView.Columns)
				{
					g.DrawLine(pen, offset + column.Width - 2, 0, offset + column.Width - 2, bgSize.Height);
					offset += column.Width;
				}
			}

			var prevImage = listView.BackgroundImage;
			listView.BackgroundImage = image;
			if (prevImage != null)
				prevImage.Dispose();
		}

		private Point GetImageLocation(ContentAlignment alignment, Size bgSize, Size imageSize)
		{
			switch (alignment)
			{
				case ContentAlignment.TopLeft: return new Point(0, 0);
				case ContentAlignment.TopCenter: return new Point((bgSize.Width - imageSize.Width) / 2, 0);
				case ContentAlignment.TopRight: return new Point(bgSize.Width - imageSize.Width, 0);
				case ContentAlignment.MiddleLeft:
					return new Point(0, (bgSize.Height - imageSize.Height) / 2);
				case ContentAlignment.MiddleCenter:
					return new Point((bgSize.Width - imageSize.Width) / 2, (bgSize.Height - imageSize.Height) / 2);
				case ContentAlignment.MiddleRight:
					return new Point(bgSize.Width - imageSize.Width, (bgSize.Height - imageSize.Height) / 2);
				case ContentAlignment.BottomLeft:
					return new Point(0, bgSize.Height - imageSize.Height);
				case ContentAlignment.BottomCenter:
					return new Point((bgSize.Width - imageSize.Width) / 2, bgSize.Height - imageSize.Height);
				case ContentAlignment.BottomRight:
					return new Point(bgSize.Width - imageSize.Width, bgSize.Height - imageSize.Height);
				default:
					return new Point(0, 0);
			}
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
				? _theme.ListView.EvenRowColor
				: _theme.ListView.OddRowColor;

			if (_theme.ListViewBackground != null)
				backColor = Color.FromArgb(191, backColor);

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

			using (var pen = new Pen(_theme.ListView.ColumnBorderColor))
				e.Graphics.DrawLine(pen, e.Bounds.Right - 2, e.Bounds.Y, e.Bounds.Right - 2, e.Bounds.Bottom);
		}

		private void HandleListViewDrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
		{
			if (!_enabled)
			{
				e.DrawDefault = true;
				return;
			}

			if (_theme.ListViewBackgroundTiled)
			{
				var listView = (ListView) sender;
				listView.BackgroundImage = listView.Items.Count == 0 ? _theme.ListViewBackground : null;
			}

			var graphics = e.Graphics;
			var r = e.Bounds;

			using (Brush backBrush = new SolidBrush(_theme.ListView.HeaderBackColor))
			{
				graphics.FillRectangle(backBrush, r);
			}

			using (var pen = new Pen(_theme.ListView.HeaderColumnBorderColor))
			{
				graphics.DrawLine(pen, r.X, r.Y, r.Right, r.Y);
				graphics.DrawLine(pen, r.Right - 2, r.Y, r.Right - 2, r.Bottom);
			}

			var flags = GetTextFormatFlags(e.Header.TextAlign);
			TextRenderer.DrawText(graphics, " " + e.Header.Text + " ", e.Font, r,
				_theme.ListView.HeaderForeColor, flags);
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