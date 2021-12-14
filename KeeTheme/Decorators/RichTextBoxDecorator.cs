using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using KeePass.Forms;
using KeePass.UI;
using KeePassLib.Utility;
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
			
			Padding = new Padding(1);
			Location = richTextBox.Location;
			Size = richTextBox.Size;
			Dock = richTextBox.Dock;

			// RichTextBox.BorderStyle.FixedSingle doesn't work
			var parent = richTextBox.Parent;
			var index = parent.Controls.GetChildIndex(richTextBox);
			parent.Controls.Remove(richTextBox);
			parent.Controls.Add(this);
			parent.Controls.SetChildIndex(this, index);
			BorderStyle = _theme.RichTextBox.BorderStyle;
			richTextBox.BorderStyle = BorderStyle.None;
			richTextBox.Location = Point.Empty;
			Controls.Add(richTextBox);
			EnabledChanged += HandleEnabledChanged;

			if (Parent.GetType() != typeof(DataEditorForm))
			{
				richTextBox.TextChanged += HandleRichTextBoxTextChanged;
				_richTextBoxNativeWindow = new RichTextBoxNativeWindow(richTextBox);
				_richTextBoxNativeWindow.Paint += HandleRichTextBoxPaint;
				_richTextBoxNativeWindow.LinkCreated += HandleRichTextBoxLinkCreated;
				HandleTabStop(richTextBox);
				richTextBox.TabStopChanged += HandleTabStopChanges;
				richTextBox.TabIndexChanged += HandleTabStopChanges;
			}
			else
			{
				// Original font colors should be kept in the attachment viewer RTF document
				var customRichTextBox = richTextBox as CustomRichTextBoxEx;
				if (customRichTextBox != null && customRichTextBox.SimpleTextOnly)
					richTextBox.TextChanged += HandleRichTextBoxTextChanged;
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
				var startPointPadded = new Point(startPoint.X - 3, startPoint.Y);
				TextRenderer.DrawText(graphics, linkText, font, startPointPadded, _theme.LinkLabel.LinkColor);
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
			ApplyFontColor(richTextBox);
			if (_lastText != richTextBox.Text)
			{
				_lastText = richTextBox.Text;
				_detectedLinks.Clear();
			}
		}

		private void ApplyFontColor(RichTextBox richTextBox)
		{
			var selectionStart = richTextBox.SelectionStart;
			var selectionLength = richTextBox.SelectionLength;

			richTextBox.SelectAll();
			richTextBox.SelectionColor = _theme.RichTextBox.SelectionColor;
			richTextBox.Select(selectionStart, selectionLength);
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
