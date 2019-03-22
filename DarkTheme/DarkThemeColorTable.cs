using System.Drawing;
using System.Windows.Forms;

namespace DarkTheme
{
    public class DarkThemeColorTable : ProfessionalColorTable
    {
        public override Color ToolStripBorder
        {
            get { return Colors.ToolStripBorder; }
        }

        public override Color ToolStripDropDownBackground
        {
            get { return Colors.ToolStrip; }
        }

        public override Color ToolStripGradientBegin
        {
            get { return Colors.ToolStrip; }
        }

        public override Color ToolStripGradientEnd
        {
            get { return Colors.ToolStrip; }
        }

        public override Color ToolStripGradientMiddle
        {
            get { return Colors.ToolStrip; }
        }

        public override Color ImageMarginGradientBegin
        {
            get { return Colors.ToolStrip; }
        }

        public override Color ImageMarginGradientMiddle
        {
            get { return Colors.ToolStrip; }
        }

        public override Color ImageMarginGradientEnd
        {
            get { return Colors.ToolStrip; }
        }
    }
}