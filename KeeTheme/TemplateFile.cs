namespace KeeTheme
{
	public class TemplateFile
	{
		public string Name { get; set; }
		public string Path { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}