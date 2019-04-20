using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeeTheme
{
	static class ListViewExtensions
	{
		[Serializable, StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}

		const int LVM_FIRST = 0x1000;
		const int LVM_GETHEADER = (LVM_FIRST + 31);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		private static extern IntPtr SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		private static extern bool GetWindowRect(HandleRef hwnd, out RECT lpRect);

		public static int GetHeaderHeight(this ListView listView)
		{
			var hwnd = SendMessage(listView.Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);
			if (hwnd != IntPtr.Zero)
			{
				var rc = new RECT();
				if (GetWindowRect(new HandleRef(null, hwnd), out rc))
				{
					return rc.Bottom - rc.Top;
				}
			}

			return 0;
		}
	}
}
