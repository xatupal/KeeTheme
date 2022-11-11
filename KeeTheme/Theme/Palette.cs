using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;

namespace KeeTheme.Theme
{
	class Palette
	{
		private readonly Dictionary<string, Color> _colorsByName = new Dictionary<string, Color>();

		public Palette()
		{
		}
		
		public Palette(Dictionary<string, string> colors)
		{
			foreach (var colorName in colors.Keys)
			{
				_colorsByName[colorName] = ParseColor(colors[colorName]);
			}
		}

		public Color[] GetColors()
		{
			return _colorsByName.Values.ToArray();
		}

		public Color GetColor(string color)
		{
			return _colorsByName.ContainsKey(color) ? _colorsByName[color] : ParseColor(color);
		}

		public static Color ParseColor(string color)
		{
			if (string.IsNullOrEmpty(color))
				return Color.Empty;

			// Format: #aabbcc
			if (color.StartsWith("#") && color.Length == 7)
				return Color.FromArgb(int.Parse("FF" + color.Substring(1), NumberStyles.AllowHexSpecifier));

			// Format: #abc
			if (color.StartsWith("#") && color.Length == 4)
				return Color.FromArgb(int.Parse("FF" + color[1] + color[1] + color[2] + color[2] + color[3] + color[3],
					NumberStyles.AllowHexSpecifier));

			// Format: (64,64,64)
			if (color.StartsWith("(") && color.EndsWith(")"))
			{
				var rgb = color.Substring(1, color.Length - 2).Split(',');
				return Color.FromArgb(int.Parse(rgb[0]), int.Parse(rgb[1]), int.Parse(rgb[2]));
			}

			throw new InvalidDataException("Unknown color '" + color + "'!");
		}

		public void FromBgrArray(int[] colors)
		{
			_colorsByName.Clear();
			for (int i = 0; i < colors.Length; i++)
			{
				var colorBgr = BitConverter.GetBytes(colors[i]);
				var colorRgb = Color.FromArgb(colorBgr[2], colorBgr[1], colorBgr[0]);
				_colorsByName.Add("Color" + i, colorRgb);
			}
		}

		public int[] ToBgrArray()
		{
			var colors = new List<int>();
			foreach (var color in _colorsByName.Values)
			{
				var bgrColor = (color.B << 16) + (color.G << 8) + color.R; 
				colors.Add(bgrColor);
			}

			return colors.ToArray();
		}
	}
}
