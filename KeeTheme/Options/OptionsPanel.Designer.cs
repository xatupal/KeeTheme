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
			this.autoSyncWin10ThemeCheckBox = new System.Windows.Forms.CheckBox();
			this.autoSyncWin10ThemeLabel = new System.Windows.Forms.Label();
			this.themeTemplate = new System.Windows.Forms.Label();
			this.themeTemplateComboBox = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// hotKeyLabel
			// 
			this.hotKeyLabel.Location = new System.Drawing.Point(9, 38);
			this.hotKeyLabel.Name = "hotKeyLabel";
			this.hotKeyLabel.Size = new System.Drawing.Size(110, 23);
			this.hotKeyLabel.TabIndex = 0;
			this.hotKeyLabel.Text = "KeeTheme hot key:";
			// 
			// hotKeyTextBox
			// 
			this.hotKeyTextBox.Location = new System.Drawing.Point(125, 35);
			this.hotKeyTextBox.Name = "hotKeyTextBox";
			this.hotKeyTextBox.Size = new System.Drawing.Size(166, 20);
			this.hotKeyTextBox.TabIndex = 1;
			// 
			// autoSyncWin10ThemeCheckBox
			// 
			this.autoSyncWin10ThemeCheckBox.Location = new System.Drawing.Point(189, 61);
			this.autoSyncWin10ThemeCheckBox.Name = "autoSyncWin10ThemeCheckBox";
			this.autoSyncWin10ThemeCheckBox.Size = new System.Drawing.Size(102, 24);
			this.autoSyncWin10ThemeCheckBox.TabIndex = 2;
			this.autoSyncWin10ThemeCheckBox.UseVisualStyleBackColor = true;
			// 
			// autoSyncWin10ThemeLabel
			// 
			this.autoSyncWin10ThemeLabel.Location = new System.Drawing.Point(9, 66);
			this.autoSyncWin10ThemeLabel.Name = "autoSyncWin10ThemeLabel";
			this.autoSyncWin10ThemeLabel.Size = new System.Drawing.Size(174, 23);
			this.autoSyncWin10ThemeLabel.TabIndex = 3;
			this.autoSyncWin10ThemeLabel.Text = "Auto-sync with Windows 10 theme:";
			// 
			// themeTemplate
			// 
			this.themeTemplate.Location = new System.Drawing.Point(9, 11);
			this.themeTemplate.Name = "themeTemplate";
			this.themeTemplate.Size = new System.Drawing.Size(110, 23);
			this.themeTemplate.TabIndex = 4;
			this.themeTemplate.Text = "KeeTheme template:";
			// 
			// themeTemplateComboBox
			// 
			this.themeTemplateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.themeTemplateComboBox.FormattingEnabled = true;
			this.themeTemplateComboBox.Location = new System.Drawing.Point(125, 8);
			this.themeTemplateComboBox.Name = "themeTemplateComboBox";
			this.themeTemplateComboBox.Size = new System.Drawing.Size(166, 21);
			this.themeTemplateComboBox.TabIndex = 6;
			// 
			// OptionsPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.themeTemplateComboBox);
			this.Controls.Add(this.themeTemplate);
			this.Controls.Add(this.autoSyncWin10ThemeLabel);
			this.Controls.Add(this.autoSyncWin10ThemeCheckBox);
			this.Controls.Add(this.hotKeyTextBox);
			this.Controls.Add(this.hotKeyLabel);
			this.Name = "OptionsPanel";
			this.Size = new System.Drawing.Size(645, 200);
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		private System.Windows.Forms.Label themeTemplate;
		private System.Windows.Forms.ComboBox themeTemplateComboBox;

		private System.Windows.Forms.CheckBox autoSyncWin10ThemeCheckBox;

		private System.Windows.Forms.Label autoSyncWin10ThemeLabel;

		private System.Windows.Forms.Label syncWin10ThemeLabel;
		private System.Windows.Forms.CheckBox syncWin10ThemeCheckBox;

		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Label syncWinThemeLabel;

		private System.Windows.Forms.Label hotKeyLabel;
		private KeePass.UI.HotKeyControlEx hotKeyTextBox;

		#endregion
	}
}