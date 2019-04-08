using System.Collections.Generic;
using System.IO;
using DarkTheme.Properties;

namespace DarkTheme.Custom
{
	class IniFile
	{
		readonly Dictionary<string, Dictionary<string, string>> _sections = new Dictionary<string, Dictionary<string, string>>();

		public Dictionary<string, string> GetSection(string name)
		{
			if (_sections.ContainsKey(name))
				return new Dictionary<string, string>(_sections[name]);

			return new Dictionary<string, string>();
		}

		public IniFile(TextReader tr)
		{
			string currentSection = null;

			string line;
			while ((line = tr.ReadLine()) != null)
			{
				line = line.Trim();

				if (string.IsNullOrEmpty(line) || line.StartsWith(";"))
					continue;

				if (line.StartsWith("[") && line.EndsWith("]"))
				{
					var sectionName = line.Substring(1, line.Length - 2);
					if (!_sections.ContainsKey(sectionName))
						_sections.Add(sectionName, new Dictionary<string, string>());

					currentSection = sectionName;
					continue;
				}

				if (currentSection == null)
					throw new InvalidDataException("Section is missing!");

				var keyValuePair = line.Split(new[] { '=' }, 2);
				if (keyValuePair.Length == 1)
					throw new InvalidDataException("Line should have format 'key = value'!");

				_sections[currentSection][keyValuePair[0].Trim()] = keyValuePair[1].Trim();
			}
		}

		internal static IniFile GetFromFile()
		{
			var location = typeof(CustomSkin).Assembly.Location;
			var path = Path.ChangeExtension(location, ".ini");
			if (!File.Exists(path))
				return null;

			try
			{
				using (var sr = File.OpenText(path))
					return new IniFile(sr);
			}
			catch (InvalidDataException)
			{
				return null;
			}
		}

		internal static IniFile GetFromResources()
		{
			using (var sr = new StringReader(Resource.DarkSkin))
			{
				return new IniFile(sr);
			}
		}

	}
}
