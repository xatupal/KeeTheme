using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using KeePassLib.Utility;
using KeeTheme.Options;
using KeeTheme.Theme;

namespace KeeTheme.Editor
{
	public partial class TemplateEditorForm : Form
	{
		private static TemplateEditorForm _instance;
		private static Thread _thread;
		
		public static TemplateEditorForm Instance
		{
			get { return _instance; }
		}
		
		public event EventHandler PreviewButtonClick
		{
			add { previewButton.Click += value; }
			remove { previewButton.Click -= value; }
		}	
		
		private readonly KeeThemeOptions _options;
		private readonly string _template;
		private readonly string _tempFileName;

		private CustomThemeTemplate _customTheme;
		private bool _previewMode;
		
		private TemplateEditorForm(CustomThemeTemplate customTheme, KeeThemeOptions options)
		{
			_customTheme = customTheme;
			_options = options;
			_template = options.Template;
			_tempFileName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

			InitializeComponent();

			propertyGrid.SelectedObject = _customTheme;
			
			Closed += HandleClosed;
		}

		private void HandleClosed(object sender, EventArgs e)
		{
			if (_previewMode)
			{
				_options.Template = _template;
			}

			_instance = null;
		}

		private void HandleLoadButtonClick(object sender, EventArgs e)
		{
			using (var fileDialog = new OpenFileDialog())
			{
				fileDialog.DefaultExt = "ini";
				fileDialog.Filter = @"KeeTheme template (*.ini)|*.ini";
				fileDialog.InitialDirectory = TemplateReader.GetTemplatesDir();
				if (fileDialog.ShowDialog() == DialogResult.OK)
				{
					var template = TemplateReader.Get("file:" + fileDialog.FileName);
					if (template == null || TemplateReader.GetTemplateName(fileDialog.FileName) == null)
					{
						MessageService.ShowWarning("Unable to load template from file:", fileDialog.FileName);
						return;
					}
					
					_customTheme = new CustomThemeTemplate(template);
					propertyGrid.SelectedObject = _customTheme;
					_previewMode = false;
				}
			}		
		}

		private void HandleSaveButtonClick(object sender, EventArgs e)
		{
			using (var fileDialog = new SaveFileDialog())
			{
				fileDialog.DefaultExt = "ini";
				fileDialog.Filter = @"KeeTheme template (*.ini)|*.ini";
				fileDialog.InitialDirectory = TemplateReader.GetTemplatesDir();
				if (fileDialog.ShowDialog() == DialogResult.OK)
				{
					var iniFile = _customTheme.GetIniFile();
					iniFile.SaveFile(fileDialog.FileName);
					_options.Template = _template;
					_options.Template = "file:" + fileDialog.FileName;
					_previewMode = false;
					Close();
				}
			}
		}

		private void HandlePreviewButtonClick(object sender, EventArgs e)
		{
			const string previewSuffix = "(preview)";
			var name = _customTheme.Name;
			if (!_customTheme.Name.EndsWith(previewSuffix))
				_customTheme.Name += " " + previewSuffix;
			
			var iniFile = _customTheme.GetIniFile();
			_customTheme.Name = name;
			iniFile.SaveFile(_tempFileName);
			_options.Template = _template;
			_options.Template = "file:" + _tempFileName;
			
			_previewMode = true;
		}

		public static TemplateEditorForm Create(KeeThemeOptions options)
		{
			if (_instance != null)
				return _instance;
			
			var theme = new CustomThemeTemplate(TemplateReader.Get(options.Template));
			_instance = new TemplateEditorForm(theme, options);
			return _instance;
		}
	}
}