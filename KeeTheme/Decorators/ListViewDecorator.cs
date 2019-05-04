using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using KeePass;
using KeeTheme.Theme;

namespace KeeTheme.Decorators
{
	class ListViewDecorator : Control
	{
		private readonly ListView _listView;
		private readonly ListViewHeaderPainter _headerPainter;

		private ITheme _theme;
		private bool _enabled;

		public ListViewDecorator(ListView listView, ITheme theme)
		{
			_listView = listView;
			_theme = theme;

			_headerPainter = new ListViewHeaderPainter(_listView);
			_headerPainter.Paint += HandleHeaderPaint;

			_listView.Controls.Add(this);
		}

		private void HandleHeaderPaint(object sender, PaintEventArgs e)
		{
			if (!_enabled)
				return;

			using (var brush = new SolidBrush(_theme.ListView.HeaderBackColor))
			{
				e.Graphics.FillRectangle(brush, e.ClipRectangle);
				using (var pen = new Pen(_theme.ListView.HeaderColumnBorderColor))
				{
					e.Graphics.DrawLine(pen, 
						e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Right, e.ClipRectangle.Y);
				}
			}
		}

		private void Apply(Control control)
		{
			var listView = (ListView)control;

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

			var bgSize = new Size(listView.ClientSize.Width, listView.ClientSize.Height - _headerPainter.HeaderHeight);
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
				var listView = (ListView)sender;
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

		public void EnableTheme(bool enabled, ITheme theme)
		{
			_enabled = enabled;
			_theme = theme;

			Apply(_listView);
		}
	}
}
