using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using KeeTheme.Theme;

namespace KeeTheme.Editor
{
	public class ColorConverter : TypeConverter
	{
		// This is used, for example, by DefaultValueAttribute to convert from string to Color.
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value.GetType() == typeof(string))
				return Palette.ParseColor((string)value);
			
			return base.ConvertFrom(context, culture, value);
		}
		
		// This is used, for example, by the PropertyGrid to convert Color to a string.
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
		{
			if (destType == typeof(string) && value is Color)
			{
				var color = (Color) value;
				if (color == Color.Empty)
					return "";
				
				return '#' + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
			}
			return base.ConvertTo(context, culture, value, destType);
		}
	}
}