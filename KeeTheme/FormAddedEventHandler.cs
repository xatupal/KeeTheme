using System.Windows.Forms;

namespace KeeTheme
{
	public delegate void FormAddedEventHandler(object sender, FormAddedEventArgs args);
	
	public class FormAddedEventArgs
	{
		public Form Form { get; set; }

		public FormAddedEventArgs(Form form)
		{
			Form = form;
		}
	}
}