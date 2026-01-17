using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KeePass;
using KeePass.App;
using KeeTheme.Theme;

namespace KeeTheme
{
	public static class TemplateReader
	{
		public const string DefaultTemplatePath = "KeeTheme.Resources.DarkTheme.ini";

		internal static IniFile GetDefaultTemplate()
		{
			return GetFromResources(DefaultTemplatePath);
		}
		
		internal static List<TemplateFile> GetTemplatesFromResources()
		{
			var result = new List<TemplateFile>();
			var resourceNames = typeof(TemplateReader).Assembly.GetManifestResourceNames();
			foreach (var resourceName in resourceNames.Where(x => x.EndsWith(".ini")))
			{
				var resourceStream = typeof(TemplateReader).Assembly.GetManifestResourceStream(resourceName);
				using (var sr = new StreamReader(resourceStream))
				{
					var templateName = GetTemplateName(sr);
					if (templateName == null)
						continue;

					var template = new TemplateFile();
					template.Name = templateName;
					template.Path = resourceName;
					result.Add(template);
				}
			}

			return result;
		}

		private static string GetTemplateName(StreamReader streamReader)
		{
			try
			{
				var iniFile = new IniFile(streamReader);
				var themeSection = iniFile.GetSection("KeeTheme");
				return themeSection.ContainsKey("Name") ? themeSection["Name"] : null;
			}
			catch (Exception)
			{
				return null;
			}
		}
		
		internal static List<TemplateFile> GetTemplatesFromPluginsDir()
		{
			var result = new List<TemplateFile>();
			var templateDir = GetTemplatesDir();
			if (!Directory.Exists(templateDir))
				return result;
			
			var files = Directory.GetFiles(templateDir, "*.ini");
			foreach (var file in files)
			{
				using (var sr = File.OpenText(file))
				{
					var templateName = GetTemplateName(sr);
					if (templateName == null)
						continue;

					var template = new TemplateFile();
					template.Name = templateName;
					template.Path = "file:" + Path.GetFileName(file);
					result.Add(template);
				}
			}

			return result;
		}

		public static string GetTemplatesDir()
		{
			var exeLocation = Path.GetDirectoryName(typeof(Program).Assembly.Location);
			return Path.Combine(exeLocation, AppDefs.PluginsDir);
		}
		
		private static IniFile GetFromFile(string fileName)
		{
			var filePath = fileName;
			if (!Path.IsPathRooted(filePath))
			{
				filePath = Path.Combine(GetTemplatesDir(), fileName);
			}
			
			if (!File.Exists(filePath))
				return null;

			try
			{
				using (var sr = File.OpenText(filePath))
					return new IniFile(sr);
			}
			catch (Exception)
			{
				return null;
			}
		}

		private static IniFile GetFromResources(string resourceName)
		{
			try
			{
				var stream = typeof(TemplateReader).Assembly.GetManifestResourceStream(resourceName);
				using (var sr = new StreamReader(stream))
				{
					return new IniFile(sr);
				}
			}
			catch (Exception e)
			{
				return null;
			}
		}

		internal static IniFile Get(string templatePath)
		{
			return templatePath.StartsWith("file:")
				? GetFromFile(templatePath.Substring("file:".Length))
				: GetFromResources(templatePath);
		}

		internal static string GetTemplateName(string templatePath)
		{
			var iniFile = Get(templatePath);
			if (iniFile == null)
				return null;
			
			var themeSection = iniFile.GetSection("KeeTheme");
			return themeSection["Name"];
		}
	}
}