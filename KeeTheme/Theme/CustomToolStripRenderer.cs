using System.Windows.Forms;
using KeePass.UI.ToolStripRendering;

namespace KeeTheme.Theme
{
	class CustomToolStripRenderer : ProExtTsr
	{
		private readonly CustomTheme _customTheme;

		protected override bool EnsureTextContrast
		{
			get { return false; }
		}

		public CustomToolStripRenderer(CustomTheme customTheme, ProfessionalColorTable ct) : base(ct)
		{
			_customTheme = customTheme;
		}

		protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
		{
			if (e.Item.Pressed || e.Item.Selected)
			{
				e.TextColor = _customTheme.MenuItem.HighlightColor;
			}

			base.OnRenderItemText(e);
		}
	}
}
