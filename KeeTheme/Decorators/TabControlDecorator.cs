using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using KeePassLib.Utility;
using KeeTheme.Theme;

namespace KeeTheme.Decorators
{
	class TabControlDecorator : Control
	{
		private readonly TabControl _tabControl;
		private readonly TabControlPainter _painter;

		private ITheme _theme;
		private bool _enabled;

		public TabControlDecorator(TabControl tabControl, ITheme theme)
		{
			_tabControl = tabControl;
			_theme = theme;

			if (!MonoWorkarounds.IsRequired())
			{
				typeof(TabControl).InvokeMember("DoubleBuffered",
					BindingFlags.SetProperty | 
					BindingFlags.Instance | 
					BindingFlags.NonPublic,
					null, _tabControl, new object[] { true });
				
				_painter = new TabControlPainter(_tabControl);
				_painter.Paint += HandlePaint;
			}

			if (_tabControl.Parent != null)
				_tabControl.Parent.Controls.Add(this);
		}

		private void HandlePaint(object sender, PaintEventArgs e)
		{
			if (!_enabled)
				return;

			using (var buffer = BufferedGraphicsManager.Current.Allocate(e.Graphics, e.ClipRectangle))
			{
				using (var brush = new SolidBrush(_theme.Control.BackColor))
				{
					buffer.Graphics.FillRectangle(brush, e.ClipRectangle);
					DrawTabs(buffer.Graphics);
					DrawContentFrame(buffer.Graphics);
				}
				buffer.Render(e.Graphics);
			}
		}

		private void DrawTabs(Graphics g)
		{
			for (int i = 0; i < _tabControl.TabPages.Count; i++)
			{
				var tabPage = _tabControl.TabPages[i];
				var tabBounds = _tabControl.GetTabRect(i);
				var isSelected = _tabControl.SelectedIndex == i;

				var backColor = isSelected
					? _theme.TabControl.SelectedTabColor
					: _theme.TabControl.UnselectedTabColor;
				var foreColor = _theme.Control.ForeColor;

			
				using (var backBrush = new SolidBrush(backColor))
				{
					g.FillRectangle(backBrush, tabBounds);
				}

				var borderColor = isSelected
					? _theme.TabControl.SelectedTabBorderColor
					: _theme.TabControl.UnselectedTabBorderColor;
				
				// Draw tab frame
				using (var pen = new Pen(borderColor))
				{
					// Top line
					g.DrawLine(pen, tabBounds.Left, tabBounds.Top, tabBounds.Right, tabBounds.Top);
					// Left line
					g.DrawLine(pen, tabBounds.Left, tabBounds.Top, tabBounds.Left, tabBounds.Bottom);
					// Right line
					g.DrawLine(pen, tabBounds.Right, tabBounds.Top, tabBounds.Right, tabBounds.Bottom);
            
					// Bottom line only for non-selected tabs
					if (!isSelected)
					{
						g.DrawLine(pen, tabBounds.Left, tabBounds.Bottom, tabBounds.Right, tabBounds.Bottom);
					}
				}
				
				var textBounds = tabBounds;
  
				if (_tabControl.ImageList != null && tabPage.ImageIndex >= 0)
				{
					var image = _tabControl.ImageList.Images[tabPage.ImageIndex];
					var iconX = tabBounds.Left + 4;
					var iconY = tabBounds.Top + (tabBounds.Height - image.Height) / 2;
   
					g.DrawImage(image, iconX, iconY);
   
					textBounds.X += image.Width + 8;
					textBounds.Width -= image.Width + 8;
				}
				else if (_tabControl.ImageList != null && !string.IsNullOrEmpty(tabPage.ImageKey))
				{
					var image = _tabControl.ImageList.Images[tabPage.ImageKey];
					var iconX = tabBounds.Left + 4;
					var iconY = tabBounds.Top + (tabBounds.Height - image.Height) / 2;
   
					g.DrawImage(image, iconX, iconY);
   
					textBounds.X += image.Width + 8;
					textBounds.Width -= image.Width + 8;
				}

				TextRenderer.DrawText(
					g,
					tabPage.Text,
					_tabControl.Font,
					textBounds,
					foreColor,
					TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
				);
			}
		}
		
		private void DrawContentFrame(Graphics g)
		{
			var displayRect = _tabControl.DisplayRectangle;
			var activeTabBounds = _tabControl.GetTabRect(_tabControl.SelectedIndex);
    
			using (var pen = new Pen(_theme.TabControl.SelectedTabBorderColor))
			{
				// Adjust the display rectangle to account for the frame
				var frameRect = new Rectangle(
					displayRect.X - 2,
					displayRect.Y - 2,
					displayRect.Width + 3,
					displayRect.Height + 3
				);

				// Draw the frame, but skip the line segment that connects to the active tab
				if (_tabControl.Alignment == TabAlignment.Top)
				{
					// Top line (with gap for active tab)
					g.DrawLine(pen, frameRect.Left, frameRect.Top, activeTabBounds.Left, frameRect.Top);
					g.DrawLine(pen, activeTabBounds.Right, frameRect.Top, frameRect.Right, frameRect.Top);
            
					// Right, bottom, and left lines
					g.DrawLine(pen, frameRect.Right, frameRect.Top, frameRect.Right, frameRect.Bottom);
					g.DrawLine(pen, frameRect.Right, frameRect.Bottom, frameRect.Left, frameRect.Bottom);
					g.DrawLine(pen, frameRect.Left, frameRect.Bottom, frameRect.Left, frameRect.Top);
				}}
		}

		public void EnableTheme(bool enabled, ITheme theme)
		{
			_enabled = enabled;
			_theme = theme;
		}
	}
}
