using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using KeeTheme.Editor;

namespace KeeTheme.Theme
{
    [TypeConverter(typeof(CheckBoxLookTypeConverter))]
    class CheckBoxLook : ControlLook
    {
        public Color BorderColor { get; set; }
        public FlatStyle FlatStyle { get; set; }
        public Color CheckedBackColor { get; set; }
        public Color MouseDownBackColor { get; set; }
        public Color MouseOverBackColor { get; set; }
    }
}