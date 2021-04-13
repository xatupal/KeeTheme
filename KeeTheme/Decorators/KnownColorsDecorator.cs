using System;
using System.Drawing;
using System.Reflection;
using KeePassLib.Utility;
using KeeTheme.Theme;

namespace KeeTheme.Decorators
{
    internal static class KnownColorsDecorator
    {
        private static int[] _originalColorTable;
        
        public static void Apply(ITheme theme, bool enabled)
        {
            var colorTableFieldName = MonoWorkarounds.IsRequired() ? "s_colorTable" : "colorTable";
            var colorTableField = typeof(Color).Assembly.GetType("System.Drawing.KnownColorTable")
                .GetField(colorTableFieldName, BindingFlags.Static | BindingFlags.NonPublic);

            if (colorTableField == null)
                return;
			
            var colorTable = (int[]) colorTableField.GetValue(null);
            if (_originalColorTable == null)
            {
                _originalColorTable = new int[colorTable.Length];
                Array.Copy(colorTable, _originalColorTable, colorTable.Length);
            }
            
            if (enabled)
            {
                colorTable[(int) KnownColor.ControlText] = theme.Control.ForeColor.ToArgb();
                colorTable[(int) KnownColor.Control] = theme.Control.BackColor.ToArgb();
            }
            else
            {
                Array.Copy(_originalColorTable, colorTable, colorTable.Length);
            }
        }
    }
}
