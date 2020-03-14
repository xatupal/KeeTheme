using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeeTheme.Decorators
{
	class ListViewHeaderPainter : ListViewNativeWindow
	{
		const int LVM_GETHEADER = LVM_FIRST + 0x1F;

		[DllImport("user32.dll")]
		static extern bool GetWindowRect(HandleRef hwnd, out RECT lpRect);

		private readonly ListView _listView;

		internal int HeaderHeight { get; private set; }
		internal event PaintEventHandler Paint;

		public ListViewHeaderPainter(ListView listView)
		{
			_listView = listView;

			AssignHeaderHandle();
			_listView.HandleCreated += On_listView_HandleCreated;
			_listView.HandleDestroyed += On_listView_HandleDestroyed;
		}

		private void On_listView_HandleCreated(object sender, EventArgs e)
		{
			if (_listView != null) AssignHeaderHandle();
		}

		private void On_listView_HandleDestroyed(object sender, EventArgs e)
		{
			ReleaseHandle();
		}

		private void AssignHeaderHandle()
		{
			var headerHwnd = SendMessage(_listView.Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);
			if (headerHwnd != IntPtr.Zero)
			{
				RECT rect;
				if (GetWindowRect(new HandleRef(null, headerHwnd), out rect))
				{
					HeaderHeight = rect.Bottom - rect.Top;
				}

				AssignHandle(headerHwnd);
			}
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			if (m.Msg != WM_PAINT)
				return;

			var width = 0;
			foreach (ColumnHeader column in _listView.Columns)
			{
				width += column.Width;
			}

			using (var g = Graphics.FromHwnd(m.HWnd))
			{
				var rect = new Rectangle(width, 0, _listView.ClientSize.Width - width, HeaderHeight);
				var args = new PaintEventArgs(g, rect);
				OnPaint(args);
			}
		}

		protected virtual void OnPaint(PaintEventArgs e)
		{
			if (Paint != null)
				Paint.Invoke(this, e);
		}
	}
}
