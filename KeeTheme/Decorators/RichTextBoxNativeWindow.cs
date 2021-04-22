using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeeTheme.Decorators
{
    class RichTextBoxNativeWindow : NativeWindow
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct CHARFORMAT2
        {
            public UInt32 cbSize;
            public UInt32 dwMask;
            public UInt32 dwEffects;
            public Int32 yHeight;
            public Int32 yOffset;
            public UInt32 crTextColor;
            public Byte bCharSet;
            public Byte bPitchAndFamily;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szFaceName;

            public UInt16 wWeight;
            public UInt16 sSpacing;
            public Int32 crBackColor;
            public Int32 lcid;
            public UInt32 dwReserved;
            public Int16 sStyle;
            public Int16 wKerning;
            public Byte bUnderlineType;
            public Byte bAnimation;
            public Byte bRevAuthor;
            public Byte bReserved1;
        }
        
        internal const int WM_PAINT = 0x000F;
        internal const int WM_USER = 0x0400;
        internal const int EM_SETCHARFORMAT = WM_USER + 68;
        internal const uint CFE_LINK = 0x0020;
        
        private readonly RichTextBox _richTextBox;
        
        internal event PaintEventHandler Paint;
        internal event EventHandler LinkCreated; 

        public RichTextBoxNativeWindow(RichTextBox richTextBox)
        {
            _richTextBox = richTextBox;
            if (TryAssignHandle(_richTextBox.Handle))
            {
                _richTextBox.HandleCreated += HandleRichTextBoxHandleCreated;
                _richTextBox.HandleDestroyed += HandleRichTextBoxHandleDestroyed;
            }
        }

        private bool TryAssignHandle(IntPtr handle)
        {
            try
            {
                AssignHandle(handle);
                return true;
            }
            catch (InvalidOperationException e)
            {
                return false;
            }
        }
        
        private void HandleRichTextBoxHandleCreated(object sender, EventArgs e)
        {
            if (_richTextBox != null) 
                AssignHandle(_richTextBox.Handle);
        }

        private void HandleRichTextBoxHandleDestroyed(object sender, EventArgs e)
        {
            ReleaseHandle();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == EM_SETCHARFORMAT)
            {
                var cf = (CHARFORMAT2) Marshal.PtrToStructure(m.LParam, typeof(CHARFORMAT2));
                if ((cf.dwEffects & CFE_LINK) != 0)
                {
                    var args = new EventArgs();
                    OnLinkCreated(args);
                }
            }
            
            if (m.Msg == WM_PAINT)
            {
                using (var g = Graphics.FromHwnd(m.HWnd))
                {
                    var rect = new Rectangle(0, 0, _richTextBox.ClientSize.Width, _richTextBox.ClientSize.Height);
                    var args = new PaintEventArgs(g, rect);
                    OnPaint(args);
                }
            }
        }

        protected virtual void OnLinkCreated(EventArgs e)
        {
            if (LinkCreated != null)
                LinkCreated.Invoke(_richTextBox, e);
        }
        
        protected virtual void OnPaint(PaintEventArgs e)
        {
            if (Paint != null)
                Paint.Invoke(_richTextBox, e);
        }
    }
}