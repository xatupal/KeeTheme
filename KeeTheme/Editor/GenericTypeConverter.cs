using System;
using System.ComponentModel;
using KeeTheme.Theme;

namespace KeeTheme.Editor
{
	internal class GenericTypeConverter<T> : TypeConverter
	{
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			return TypeDescriptor.GetProperties(typeof(T));
		}
	}

	internal class OtherLookTypeConverter : GenericTypeConverter<OtherLook>
	{
	}	
	
	internal class MenuLookTypeConverter : GenericTypeConverter<MenuLook>
	{
	}	
	
	internal class ListViewLookTypeConverter : GenericTypeConverter<ListViewLook>
	{
	}	
	
	internal class LinkLabelLookTypeConverter : GenericTypeConverter<LinkLabelLook>
	{
	}
	
	internal class RichTextBoxLookTypeConverter : GenericTypeConverter<RichTextBoxLook>
	{
	}
	
	internal class TreeViewLookTypeConverter : GenericTypeConverter<TreeViewLook>
	{
	}
	
	internal class TabControlTypeConverter : GenericTypeConverter<TabControlLook>
	{
	}

	internal class ControlLookTypeConverter : GenericTypeConverter<ControlLook>
	{
	}
	
	internal class CheckBoxLookTypeConverter : GenericTypeConverter<CheckBoxLook>
	{
	}
	
	internal class CheckBoxButtonLookTypeConverter : GenericTypeConverter<CheckBoxButtonLook>
	{
	}
	
	internal class ButtonLookTypeConverter : GenericTypeConverter<ButtonLook>
	{
	}	
	
	internal class ToolStripLookTypeConverter : GenericTypeConverter<ToolStripLook>
	{
	}

	internal class PropertyGridLookTypeConverter : GenericTypeConverter<PropertyGridLook>
	{
	}

	internal class ScrollBarLookTypeConverter : GenericTypeConverter<ScrollBarLook>
	{
	}
}