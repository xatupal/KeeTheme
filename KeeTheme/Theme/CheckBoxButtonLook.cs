using System.ComponentModel;
using KeeTheme.Editor;

namespace KeeTheme.Theme
{
    [TypeConverter(typeof(CheckBoxButtonLookTypeConverter))]
    class CheckBoxButtonLook : CheckBoxLook
    {
    }
}