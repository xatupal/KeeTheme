using System.ComponentModel;
using System.Windows.Forms;

namespace KeeTheme.Options
{
	partial class OptionsPanel
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}

			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.hotKeyLabel = new System.Windows.Forms.Label();
			this.hotKeyTextBox = new KeePass.UI.HotKeyControlEx();
			this.SuspendLayout();
			// 
			// hotKeyLabel
			// 
			this.hotKeyLabel.Location = new System.Drawing.Point(9, 11);
			this.hotKeyLabel.Name = "hotKeyLabel";
			this.hotKeyLabel.Size = new System.Drawing.Size(110, 23);
			this.hotKeyLabel.TabIndex = 0;
			this.hotKeyLabel.Text = "KeeTheme hot key:";
			// 
			// hotKeyTextBox
			// 
			this.hotKeyTextBox.Location = new System.Drawing.Point(125, 8);
			this.hotKeyTextBox.Name = "hotKeyTextBox";
			this.hotKeyTextBox.Size = new System.Drawing.Size(166, 20);
			this.hotKeyTextBox.TabIndex = 1;
			// 
			// OptionsPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.hotKeyTextBox);
			this.Controls.Add(this.hotKeyLabel);
			this.Name = "OptionsPanel";
			this.Size = new System.Drawing.Size(645, 200);
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		private System.Windows.Forms.Label hotKeyLabel;
		private KeePass.UI.HotKeyControlEx hotKeyTextBox;

		#endregion
	}
}