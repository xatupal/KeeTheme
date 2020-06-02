using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using KeeTheme.Theme;

namespace KeeTheme.Decorators
{
	internal class ObjectListViewDecorator
	{
		private static Assembly _brightIdeasSoftwareAssembly;
		private static Type _objectListViewType;
		private static PropertyInfo _alternateRowBackColorProperty;
		private static PropertyInfo _headerUsesThemesProperty;
		private static PropertyInfo _headerFormatStyleProperty;

		private static Type _headerFormatStyleType;
		private static MethodInfo _setForeColorMethod;
		private static MethodInfo _setBackColorMethod;

		public static void Initialize()
		{
			if (_objectListViewType != null)
				return;

			_objectListViewType = GetType("BrightIdeasSoftware.ObjectListView");
			if (_objectListViewType == null)
				return;

			_brightIdeasSoftwareAssembly = _objectListViewType.Assembly;
			_alternateRowBackColorProperty = _objectListViewType.GetProperty("AlternateRowBackColor");
			 _headerFormatStyleProperty = _objectListViewType.GetProperty("HeaderFormatStyle");
			 _headerUsesThemesProperty = _objectListViewType.GetProperty("HeaderUsesThemes");

			_headerFormatStyleType = _brightIdeasSoftwareAssembly.GetType("BrightIdeasSoftware.HeaderFormatStyle");
			_setForeColorMethod = _headerFormatStyleType.GetMethod("SetForeColor");
			_setBackColorMethod = _headerFormatStyleType.GetMethod("SetBackColor");
		}

		private static Type GetType(string name)
		{
			return AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(TryGetTypes).FirstOrDefault(x => x.FullName.StartsWith(name));
		}

		private static IEnumerable<Type> TryGetTypes(Assembly assembly)
		{
			try
			{
				return assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException)
			{
				// If assembly types cannot be loaded - ignore.
				return new Type[0];
			}
		}

		public static bool CanDecorate(ListView listView)
		{
			return _objectListViewType != null && _objectListViewType.IsInstanceOfType(listView);
		}

		public static void Apply(ListView listView, ITheme theme)
		{
			_alternateRowBackColorProperty.SetValue(listView, theme.ListView.EvenRowColor, null);
			_headerUsesThemesProperty.SetValue(listView, false, null);

			var headerFormatStyle = Activator.CreateInstance(_headerFormatStyleType);
			_setForeColorMethod.Invoke(headerFormatStyle, new object[] { theme.ListView.HeaderForeColor });
			_setBackColorMethod.Invoke(headerFormatStyle, new object[] { theme.ListView.HeaderBackColor });
			_headerFormatStyleProperty.SetValue(listView, headerFormatStyle, null);
		}
	}
}
