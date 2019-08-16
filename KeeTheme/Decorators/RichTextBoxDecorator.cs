using System;
using System.Drawing;
using System.Windows.Forms;
using KeePass.Forms;
using KeePass.UI;
using KeeTheme.Theme;

namespace KeeTheme.Decorators
{
	sealed class RichTextBoxDecorator : Panel
	{
		private readonly RichTextBox _richTextBox;

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
			}
			else
			{
				// Original font colors should be kept in the attachment viewer RTF document
				var customRichTextBox = richTextBox as CustomRichTextBoxEx;
				if (customRichTextBox != null && customRichTextBox.SimpleTextOnly)
					richTextBox.TextChanged += HandleRichTextBoxTextChanged;
			}
			richTextBox.DockChanged += HandleRichTextBoxDockChanged;
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

			BorderStyle = _theme.RichTextBox.BorderStyle;
		}
	}
}
