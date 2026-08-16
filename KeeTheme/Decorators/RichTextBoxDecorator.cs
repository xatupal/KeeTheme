using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using KeePass.Forms;
using KeePass.UI;
using KeeTheme.Theme;

namespace KeeTheme.Decorators
{
	sealed class RichTextBoxDecorator : Panel
	{
		private class Link
		{
			public int Index { get; set; }
			public string Text { get; set; }
		}

		private readonly List<Link> _detectedLinks = new List<Link>();
		private string _lastText;
		
		private readonly RichTextBox _richTextBox;
		private readonly RichTextBoxNativeWindow _richTextBoxNativeWindow;

		private ITheme _theme;
		private bool _enabled;

		public RichTextBoxDecorator(RichTextBox richTextBox, ITheme theme)
		{
			_theme = theme;
			_richTextBox = richTextBox;

			// The panel reparenting that used to wrap the RichTextBox to draw a themed
			// border is disabled: moving the control between parents destroys and
			// re-creates its native window, which breaks the IME input context so that
			// Chinese/Japanese/Korean composition no longer works (e.g. in the Notes
			// field). The RichTextBox keeps its own default border instead.
			EnabledChanged += HandleEnabledChanged;

			var parent = richTextBox.Parent;
			if (parent != null && parent.GetType() == typeof(DataEditorForm))
			{
				// Original font colors should be kept in the attachment viewer RTF document
				var customRichTextBox = richTextBox as CustomRichTextBoxEx;
				if (customRichTextBox != null && customRichTextBox.SimpleTextOnly)
					richTextBox.TextChanged += HandleRichTextBoxTextChanged;
			}
			else
			{
				// An exception for custom keystroke sequence in EditAutoTypeItemForm
				// Original font color should be kept to indicate valid and invalid placeholders
				if (richTextBox.Name != "m_rbKeySeq")
				{
					richTextBox.TextChanged += HandleRichTextBoxTextChanged;
				}
			}

			richTextBox.DockChanged += HandleRichTextBoxDockChanged;
			richTextBox.SizeChanged += HandleRichTextBoxSizeChanged;
		}

		private void HandleRichTextBoxSizeChanged(object sender, EventArgs e)
        {
			Control c = sender as Control;
			if (c == null) return;
			Size = c.Size;
        }

        private void HandleTabStopChanges(object sender, EventArgs e)
        {
			HandleTabStop(sender as RichTextBox);
        }

        private void HandleTabStop(RichTextBox richTextBox)
        {
			if (richTextBox == null) return;
			TabStop = richTextBox.TabStop;
			TabIndex = richTextBox.TabIndex;
        }

        private void HandleRichTextBoxLinkCreated(object sender, EventArgs e)
		{
			var customRichTextBox = sender as CustomRichTextBoxEx;
			if (customRichTextBox == null)
				return;

			var link = new Link();
			link.Index = customRichTextBox.SelectionStart;
			link.Text = customRichTextBox.Text.Substring(link.Index, customRichTextBox.SelectionLength);
			_detectedLinks.Add(link);
		}

		private void HandleRichTextBoxPaint(object sender, PaintEventArgs e)
		{
			var customRichTextBox = sender as CustomRichTextBoxEx;
			if (customRichTextBox == null || customRichTextBox.Text.Length == 0 || !_enabled)
				return;

			using (var font = new Font(customRichTextBox.Font, FontStyle.Underline))
			{
				foreach (var link in _detectedLinks)
				{
					DrawLink(customRichTextBox, link, e.Graphics, font);
				}
			}
		}

		private void DrawLink(CustomRichTextBoxEx customRichTextBox, Link link, Graphics graphics, Font font)
		{
			var ranges = Subtract(link.Index, link.Text.Length, customRichTextBox.SelectionStart,
				customRichTextBox.SelectionLength);

			foreach (var range in ranges)
			{
				var linkText = link.Text.Substring(range.First - link.Index, range.Length);
				var startPoint = customRichTextBox.GetPositionFromCharIndex(range.First);
				var textSize = TextRenderer.MeasureText(linkText, font, Size.Empty, TextFormatFlags.NoPrefix);
				using (var brush = new SolidBrush(customRichTextBox.BackColor))
				{
					graphics.FillRectangle(brush, new Rectangle(startPoint, textSize));
				}
				TextRenderer.DrawText(graphics, linkText, font, startPoint, _theme.LinkLabel.LinkColor, TextFormatFlags.NoPrefix);
			}
		}

		private List<CharacterRange> Subtract(int linkStart, int linkLength, int selectionStart, int selectionLength)
		{
			var linkEnd = linkStart + linkLength;
			var selectionEnd = selectionStart + selectionLength;

			// Empty selection or selection not in range of link
			if (selectionLength == 0 || linkStart > selectionEnd || linkEnd < selectionStart)
			{
				var range = new CharacterRange(linkStart, linkLength);
				return new List<CharacterRange> { range };
			}
			
			// Selection overlaps whole link
			if (selectionStart <= linkStart && selectionEnd >= linkEnd)
				return new List<CharacterRange>();

			// Selection starts before link and ends inside link
			if (selectionStart <= linkStart && selectionEnd < linkEnd)
			{
				var result = new CharacterRange(selectionEnd, linkEnd - selectionEnd);
				return new List<CharacterRange> { result };
			}

			// Selection starts inside link and ends after link
			if (selectionStart > linkStart && selectionEnd >= linkEnd)
			{
				var result = new CharacterRange(linkStart, selectionStart - linkStart);
				return new List<CharacterRange> { result };
			}

			// Selection is inside link
			var result1 = new CharacterRange(linkStart, selectionStart - linkStart);
			var result2 = new CharacterRange(selectionEnd, linkEnd - selectionEnd);
			return new List<CharacterRange> { result1, result2 };
		}

		private void HandleRichTextBoxDockChanged(object sender, EventArgs e)
		{
			Dock = ((RichTextBox) sender).Dock;
		}

		private void HandleEnabledChanged(object sender, EventArgs e)
		{
			if (_enabled)
				ControlSnapshot.Make(_richTextBox);
		}

		private void HandleRichTextBoxTextChanged(object sender, EventArgs e)
		{
			if (!_enabled)
				return;

			var richTextBox = (RichTextBox)sender;

			// Critical: never reformat while the user is actually typing. Any formatting
			// operation (SelectAll or EM_SETCHARFORMAT/SCF_ALL) executed while an IME
			// composition is active cancels the composition, making it impossible to type
			// Chinese/Japanese/Korean. EM_SETCHARFORMAT/SCF_ALL also sets the default
			// character format, so newly typed text already inherits the theme color.
			if (richTextBox.Focused)
				return;

			// Extra safety while an IMM-reported composition is active.
			if (_richTextBoxNativeWindow != null && _richTextBoxNativeWindow.ImeComposing)
				return;

			ApplyFontColor(richTextBox);
			if (_lastText != richTextBox.Text)
			{
				_lastText = richTextBox.Text;
				_detectedLinks.Clear();
			}
		}

		private const int WM_USER = 0x400;
		private const int EM_SETCHARFORMAT = WM_USER + 68;
		private const int SCF_ALL = 0x4;
		private const uint CFM_COLOR = 0x40000000;

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, ref RichTextBoxNativeWindow.CHARFORMAT2 lParam);

		private void ApplyFontColor(RichTextBox richTextBox)
		{
			// EM_SETCHARFORMAT with SCF_ALL recolors all existing text and sets the
			// default format for future input, without touching the selection or caret.
			// SelectAll() + SelectionColor would cancel an active IME composition on
			// every keystroke, which is why Chinese/Japanese/Korean input failed.
			var cf = new RichTextBoxNativeWindow.CHARFORMAT2();
			cf.cbSize = (uint) Marshal.SizeOf(typeof(RichTextBoxNativeWindow.CHARFORMAT2));
			cf.dwMask = CFM_COLOR;
			cf.dwEffects = 0;
			cf.crTextColor = (uint) ColorTranslator.ToWin32(_theme.RichTextBox.SelectionColor);
			SendMessage(richTextBox.Handle, EM_SETCHARFORMAT, (IntPtr) SCF_ALL, ref cf);
		}

		public void EnableTheme(bool enabled, ITheme theme)
		{
			_theme = theme;
			_enabled = enabled;
			if (!enabled)
				ApplyFontColor(_richTextBox);

			BorderStyle = _theme.RichTextBox.BorderStyle;
		}
	}
}
