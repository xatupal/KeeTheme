using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeeTheme.Decorators
{
	class ListViewNativeWindow : NativeWindow
	{
		[StructLayout(LayoutKind.Sequential)]
		internal struct RECT
		{
			internal int Left;
			internal int Top;
			internal int Right;
			internal int Bottom;
		}

		internal const int WM_PAINT = 0x000F;
		internal const int LVM_FIRST = 0x1000;

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		internal static extern IntPtr SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
	}
}
