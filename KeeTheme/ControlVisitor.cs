using System;
using System.Windows.Forms;

namespace KeeTheme
{
	internal class ControlVisitor
	{
		private readonly Action<Control> _onVisit;

		public ControlVisitor(Action<Control> onVisit)
		{
			_onVisit = onVisit;
		}
		
		public void Visit(Control control)
		{
			_onVisit(control);
			Visit(control.Controls);
		}
		
		private void Visit(Control.ControlCollection formControls)
		{
			foreach (Control control in formControls)
			{
				_onVisit(control);
				Visit(control.Controls);
			}
		}
	}
}