using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using KeeTheme.Theme;

namespace KeeTheme.Editor
{
	public class ColorEditor : UITypeEditor
	{
		private IWindowsFormsEditorService _service;

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			// This tells it to show the [...] button which is clickable firing off EditValue below.
			return UITypeEditorEditStyle.Modal;
		}
		
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
				_service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

			if (_service != null && context != null)
			{
				var colorDialog = new ColorDialog();
				colorDialog.AllowFullOpen = true;
				colorDialog.FullOpen = true;
				colorDialog.AnyColor = true;
				colorDialog.Color = (Color) value;
				colorDialog.CustomColors = GetPaletteColors(context);
				if (colorDialog.ShowDialog() == DialogResult.OK)
				{
					value = colorDialog.Color;
					SetPaletteColors(context, colorDialog.CustomColors);
				}
			}

			return value;
		}

		private void SetPaletteColors(ITypeDescriptorContext context, int[] colorDialogCustomColors)
		{
			var ownerGridProperty = context.GetType().GetProperty("OwnerGrid");
			if (ownerGridProperty == null)
				return;
			
			var propertyGrid = (PropertyGrid) ownerGridProperty.GetValue(context, null);
			var customThemeTemplate = (CustomThemeTemplate) propertyGrid.SelectedObject;
			var palette = customThemeTemplate.Palette;
			palette.FromBgrArray(colorDialogCustomColors);
		}

		private int[] GetPaletteColors(ITypeDescriptorContext context)
		{
			var ownerGridProperty = context.GetType().GetProperty("OwnerGrid");
			if (ownerGridProperty == null)
				return new int[0];

			var propertyGrid = (PropertyGrid) ownerGridProperty.GetValue(context, null);
			var customThemeTemplate = (CustomThemeTemplate) propertyGrid.SelectedObject;
			var palette = customThemeTemplate.Palette;
			return palette.ToBgrArray();
		}

		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override void PaintValue(PaintValueEventArgs e)
		{
			using (var brush = new SolidBrush((Color)e.Value))
				e.Graphics.FillRectangle(brush, e.Bounds);
		}
	}
}