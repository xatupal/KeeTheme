using System;
using System.Drawing;
using System.Windows.Forms;

namespace KeeTheme.Decorators
{
	static class ControlSnapshot
	{
		public static void Make(Control control)
		{
			var disabledControlName = control.Name + "Snapshot";

			var parent = control.Parent;
			if (!parent.Enabled)
			{
				if (parent.Controls.ContainsKey(disabledControlName))
					return;

				// Can't use control.DrawToBitmap because:
				// RichTextBox - it does not draw text
				// ListView - it does not draw correct header color
				var screenCopy = TryCopyFromScreen(control);
				if (screenCopy == null)
					return;

				var controlSnapshot = new PictureBox();
				controlSnapshot.Name = disabledControlName;
				controlSnapshot.Size = control.Size;
				controlSnapshot.Location = control.Location;
				controlSnapshot.Image = screenCopy;
				parent.Controls.Add(controlSnapshot);
				parent.Controls.SetChildIndex(controlSnapshot, 0);
			}
			else
			{
				parent.Controls.RemoveByKey(disabledControlName);
			}
		}

		private static Bitmap TryCopyFromScreen(Control control)
		{
			try
			{
				return CopyFromScreen(control);
			}
			catch (ArgumentException e)
			{
				return null;
			}
		}

		private static Bitmap CopyFromScreen(Control control)
		{
			control.Update(); // Ensure control is fully painted
			var bitmap = new Bitmap(control.Bounds.Width, control.Bounds.Height);
			using (var g = Graphics.FromImage(bitmap))
			{
				// ListView.PointToScreen returns result without border
				var upperLeftSource = control is ListView 
					? control.Parent.PointToScreen(Point.Empty) 
					: control.PointToScreen(Point.Empty);

				g.CopyFromScreen(upperLeftSource, Point.Empty, control.Bounds.Size);
			}
			return bitmap;
		}
	}
}
