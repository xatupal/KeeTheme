using System.Drawing;
using System.Windows.Forms;

namespace KeeTheme.Theme
{
    class CheckBoxLook : ControlLook
    {
        public Color BorderColor { get; set; }
        public FlatStyle FlatStyle { get; set; }
        public Color CheckedBackColor { get; set; }
        public Color MouseDownBackColor { get; set; }
        public Color MouseOverBackColor { get; set; }
    }
}