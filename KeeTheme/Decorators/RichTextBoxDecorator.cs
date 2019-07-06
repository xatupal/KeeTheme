using System;
using System.Windows.Forms;
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
			Controls.Add(richTextBox);

			richTextBox.TextChanged += HandleRichTextBoxTextChanged;
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
