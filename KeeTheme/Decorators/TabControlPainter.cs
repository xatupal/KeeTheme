using System;
using System.Drawing;
using System.Windows.Forms;

namespace KeeTheme.Decorators
{
	class TabControlPainter : NativeWindow
	{
		private const int WM_PAINT = 0x000F;
		private const int WM_ERASEBKGND = 0x0014;
		private readonly TabControl _tabControl;

		internal event PaintEventHandler Paint;

		public TabControlPainter(TabControl tabControl)
		{
			_tabControl = tabControl;

			AssignTabHandle();
			_tabControl.HandleCreated += HandleTabControlHandleCreated;
			_tabControl.HandleDestroyed += HandleTabControlHandleDestroyed;
		}

		private void HandleTabControlHandleCreated(object sender, EventArgs e)
		{
			if (_tabControl != null) AssignTabHandle();
		}

		private void HandleTabControlHandleDestroyed(object sender, EventArgs e)
		{
			ReleaseHandle();
		}

		private void AssignTabHandle()
		{
			if (_tabControl.Handle == IntPtr.Zero) 
				return;
			
			AssignHandle(_tabControl.Handle);
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_ERASEBKGND)
			{
				// Return 1 to indicate we handled the background erase
				// This prevents default background clearing
				m.Result = (IntPtr)1;
				return;
			}
			
			base.WndProc(ref m);

			if (m.Msg != WM_PAINT)
				return;

			if (_tabControl.TabCount == 0)
				return;

			using (var g = Graphics.FromHwnd(m.HWnd))
			{
				var args = new PaintEventArgs(g, _tabControl.ClientRectangle);
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
