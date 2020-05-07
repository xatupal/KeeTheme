using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeeTheme.Decorators
{
	class ListViewGroupsPainter : ListViewNativeWindow
	{
		[StructLayout(LayoutKind.Sequential)]
		internal struct LVGROUP_V6
		{
			internal int cbSize;
			internal int mask;
			internal IntPtr pszHeader;
			internal int cchHeader;
			internal IntPtr pszFooter;
			internal int cchFooter;
			internal int iGroupID;
			internal int stateMask;
			internal int state;
			internal int align;

			internal IntPtr pszSubtitle;
			internal int cchSubtitle;
			internal IntPtr pszTask;
			internal int cchTask;
			internal IntPtr pszDescriptionTop;
			internal int cchDescriptionTop;
			internal IntPtr pszDescriptionBottom;
			internal int cchDescriptionBottom;
			internal int iTitleImage;
			internal int iExtendedImage;
			internal int iFirstItem;
			internal int cItems;
			internal IntPtr pszSubsetTitle;
			internal int cchSubsetTitle;

			public LVGROUP_V6(int cbSize) : this()
			{
				this.cbSize = cbSize;
				iGroupID = -1;
			}
		}

		const int LVM_GETGROUPRECT = LVM_FIRST + 0x62;
		const int LVM_GETGROUPINFOBYINDEX = LVM_FIRST + 0x99;
		const int LVGF_GROUPID = 0x0010;
		const int LVGGR_HEADER = 0x0001;

		private readonly ListView _listView;

		internal event GroupPaintEventHandler Paint;

		public ListViewGroupsPainter(ListView listView)
		{
			_listView = listView;

			AssignHandle(_listView.Handle);

			_listView.HandleCreated += HandleListViewHandleCreated;
			_listView.HandleDestroyed += HandleListViewHandleDestroyed;
		}

		private void HandleListViewHandleCreated(object sender, EventArgs e)
		{
			if (_listView != null) AssignHandle(_listView.Handle);
		}

		private void HandleListViewHandleDestroyed(object sender, EventArgs e)
		{
			ReleaseHandle();
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			if (m.Msg != WM_PAINT)
				return;

			// The first group is without header
			for (int i = 1; i <= _listView.Groups.Count; i++)
			{
				PaintGroup(i);
			}
		}

		private void PaintGroup(int i)
		{
			var groupId = GetGroupId(i);
			var rect = new RECT { Top = LVGGR_HEADER };
			var rectSize = Marshal.SizeOf(typeof(RECT));
			var lParam = Marshal.AllocHGlobal(rectSize);
			try
			{
				Marshal.StructureToPtr(rect, lParam, false);
				var retVal = SendMessage(_listView.Handle, LVM_GETGROUPRECT, new IntPtr(groupId), lParam);
				if (retVal == IntPtr.Zero)
				{
					return;
				}

				var retRc = (RECT)Marshal.PtrToStructure(lParam, typeof(RECT));
				using (var g = Graphics.FromHwnd(_listView.Handle))
				{
					var groupRect =
						new Rectangle(retRc.Left, retRc.Top, retRc.Right - retRc.Left, retRc.Bottom - retRc.Top);

					var args = new GroupPaintEventArgs(i - 1, g, groupRect);
					OnPaint(args);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(lParam);
			}
		}

		private int GetGroupId(int i)
		{
			var lvGroupSize = Marshal.SizeOf(typeof(LVGROUP_V6));
			var groupInfo = new LVGROUP_V6(lvGroupSize) { mask = LVGF_GROUPID };
			var lParam = Marshal.AllocHGlobal(lvGroupSize);
			try
			{
				Marshal.StructureToPtr(groupInfo, lParam, false);
				var retVal = SendMessage(_listView.Handle, LVM_GETGROUPINFOBYINDEX, (IntPtr)i, lParam);
				var retGroupInfo = (LVGROUP_V6)Marshal.PtrToStructure(lParam, typeof(LVGROUP_V6));
				if (retVal == IntPtr.Zero)
				{
					return -1;
				}

				return retGroupInfo.iGroupID;
			}
			finally
			{
				Marshal.FreeHGlobal(lParam);
			}
		}

		protected virtual void OnPaint(GroupPaintEventArgs e)
		{
			if (Paint != null)
				Paint.Invoke(this, e);
		}
	}

	internal delegate void GroupPaintEventHandler(object sender, GroupPaintEventArgs args);

	internal class GroupPaintEventArgs : PaintEventArgs
	{
		public int GroupId { get; private set; }

		public GroupPaintEventArgs(int groupId, Graphics graphics, Rectangle clipRect)
			: base(graphics, clipRect)
		{
			GroupId = groupId;
		}
	}
}
